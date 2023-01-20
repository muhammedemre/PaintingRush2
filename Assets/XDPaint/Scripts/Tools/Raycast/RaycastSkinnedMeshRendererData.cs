using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using XDPaint.Controllers;
using XDPaint.Core;
using XDPaint.Tools.Raycast.Base;
using XDPaint.Tools.Raycast.Data;

namespace XDPaint.Tools.Raycast
{
    public class RaycastSkinnedMeshRendererData : RaycastMeshDataBase
    {
	    private SkinnedMeshRenderer skinnedMeshRenderer;
	    private Transform[] bones;
	    private SkinnedMeshRendererBonesData bonesLocalData;
	    private bool isTrianglesDataUpdated;

	    public override void Init(Component paintComponent, Component rendererComponent)
	    {
		    base.Init(paintComponent, rendererComponent);
		    skinnedMeshRenderer = rendererComponent as SkinnedMeshRenderer;
		    if (skinnedMeshRenderer != null)
		    {
			    bones = skinnedMeshRenderer.bones;
			    var bonesCount = bones.Length;
			    bonesLocalData = new SkinnedMeshRendererBonesData
			    {
				    BonesLocalPosition = new Vector3[bonesCount],
				    BonesLocalRotation = new Quaternion[bonesCount],
				    BonesLocalScale = new Vector3[bonesCount]
			    };
			    for (var i = 0; i < bonesCount; i++)
			    {
				    var boneTransform = bones[i];
				    bonesLocalData.BonesLocalPosition[i] = boneTransform.localPosition;
				    bonesLocalData.BonesLocalRotation[i] = boneTransform.localRotation;
				    bonesLocalData.BonesLocalScale[i] = boneTransform.localScale;
			    }
		    }
		    else
		    {
			    Debug.LogError("Can't find SkinnedMeshRenderer component!");
		    }
	    }

	    public override void AddPaintManager(IPaintManager paintManager)
	    {
		    base.AddPaintManager(paintManager);
		    skinnedMeshRenderer.BakeMesh(Mesh);
		    InitUVs(paintManager, Mesh);
		    InitTriangles(paintManager, Mesh);
	    }
	    
	    public override IEnumerable<Triangle> GetNeighborsRaycasts(IPaintManager sender, Triangle currentTriangle, Ray ray)
		{
			var results = new List<Triangle>();
			foreach (var paintManager in PaintManagers)
			{
				if (paintManager != sender)
					continue;

				var intersects = new List<Triangle>();
				if (Settings.Instance.UseJobsForRaycasts)
				{
					var paintManagerComponent = (PaintManager)paintManager;
					var trianglesArray = new TriangleData[currentTriangle.N.Count];
					if (paintManagerComponent.gameObject.activeInHierarchy && paintManagerComponent.enabled)
					{
						for (var i = 0; i < currentTriangle.N.Count; i++)
						{
							var triangleIndex = currentTriangle.N[i];
							var triangle = paintManager.Triangles[triangleIndex];
							var simplified = new TriangleData
							{
								Id = triangle.Id,
								Position0 = triangle.Position0,
								Position1 = triangle.Position1,
								Position2 = triangle.Position2,
								UV0 = triangle.UV0,
								UV1 = triangle.UV1,
								UV2 = triangle.UV2
							};
							trianglesArray[i] = simplified;
						}
					}

					var triangles = new NativeArray<TriangleData>(trianglesArray, Allocator.TempJob);
					var hits = new NativeArray<RaycastTriangleHit>(trianglesArray.Length, Allocator.TempJob);
					var hitsCount = new NativeArray<int>(1, Allocator.TempJob);
					hitsCount[0] = 0;

					var job = new RaycastJob
					{
						Triangles = triangles,
						Hits = hits,
						HitsCount = hitsCount,
						RayOrigin = ray.origin,
						RayDirection = ray.direction
					};
					job.Run();

					var raycastsCount = job.HitsCount[0];
					for (var i = 0; i < raycastsCount; i++)
					{
						var hit = job.Hits[i];
						var triangleId = hit.Id;
						var triangle = paintManagerComponent.Triangles[triangleId];
						triangle.UVHit = hit.UV;
						intersects.Add(triangle);
					}

					triangles.Dispose();
					hits.Dispose();
					hitsCount.Dispose();
				}
				else
				{
					foreach (var triangleId in currentTriangle.N)
					{
						var triangle = paintManager.Triangles[triangleId];
						var isIntersected = IsIntersected(triangle, ray, false);
						if (isIntersected)
						{
							intersects.Add(triangle);
						}
					}
				}

				if (intersects.Count > 0)
				{
					var sortedIntersect = SortIntersects(intersects);
					results.Add(sortedIntersect);
				}
			}
			return results;
		}

	    public override Triangle GetRaycast(IPaintManager sender, Ray ray, bool useWorld = true, bool useCache = true)
		{
			if (useCache && FrameIntersectionData.FrameId == Time.frameCount && 
			    FrameIntersectionData.Ray.origin == ray.origin && FrameIntersectionData.Ray.direction == ray.direction)
			{
				if (FrameIntersectionData.PaintManager == sender)
				{
					return FrameIntersectionData.Triangle;
				}
				return null;
			}

			var rayTransformed = new Ray(ray.origin, ray.direction);
			if (useWorld)
			{
				if (BakedFrame != Time.frameCount)
				{
					if (IsBonesTransformsChanged())
					{
#if UNITY_2020_2_OR_NEWER
						skinnedMeshRenderer.BakeMesh(Mesh, true);
#else
						skinnedMeshRenderer.BakeMesh(Mesh);
#endif
						Mesh.GetVertices(Vertices);
						Mesh.GetUVs(0, UV);
						isTrianglesDataUpdated = false;
					}
					BakedFrame = Time.frameCount;
				}

				var boundsIntersect = skinnedMeshRenderer.bounds.IntersectRay(ray);
				if (!boundsIntersect)
					return null;

				var origin = Transform.InverseTransformPoint(ray.origin);
				var direction = Transform.InverseTransformVector(ray.direction);
				rayTransformed = new Ray(origin, direction);
			}

			var raycasts = new Dictionary<IPaintManager, Triangle>();
			foreach (var paintManager in PaintManagers)
			{
				var paintManagerComponent = (PaintManager)paintManager;
				if (paintManagerComponent.gameObject.activeInHierarchy && paintManagerComponent.enabled)
				{
					var intersects = new List<Triangle>();
					if (Settings.Instance.UseJobsForRaycasts)
					{
						var verticesData = GetTrianglesData(paintManager.SubMesh);
						if (!isTrianglesDataUpdated)
						{						
							for (var i = 0; i < paintManager.Triangles.Length; i++)
							{
								var triangle = paintManager.Triangles[i];
								var triangleData = new TriangleData
								{
									Id = triangle.Id,
									Position0 = Vertices[triangle.I0],
									Position1 = Vertices[triangle.I1],
									Position2 = Vertices[triangle.I2],
									UV0 = UV[triangle.I0],
									UV1 = UV[triangle.I1],
									UV2 = UV[triangle.I2]
								};
								verticesData.TrianglesData[i] = triangleData;
							}
						}
						
						var triangles = new NativeArray<TriangleData>(verticesData.TrianglesData, Allocator.TempJob);
						var hits = new NativeArray<RaycastTriangleHit>(verticesData.TrianglesData.Length, Allocator.TempJob);
						var hitsCount = new NativeArray<int>(1, Allocator.TempJob);
						hitsCount[0] = 0;

						var job = new RaycastJob
						{
							Triangles = triangles,
							Hits = hits,
							HitsCount = hitsCount,
							RayOrigin = rayTransformed.origin,
							RayDirection = rayTransformed.direction
						};
						job.Run();

						var raycastsCount = job.HitsCount[0];
						for (var i = 0; i < raycastsCount; i++)
						{
							var hit = job.Hits[i];
							var triangleId = hit.Id;
							var triangle = paintManagerComponent.Triangles[triangleId];
							triangle.Hit = hit.Position;
							triangle.UVHit = hit.UV;
							intersects.Add(triangle);
						}

						triangles.Dispose();
						hits.Dispose();
						hitsCount.Dispose();
					}
					else
					{
						foreach (var triangle in paintManager.Triangles)
						{
							var isIntersected = IsIntersected(triangle, rayTransformed, useWorld);
							if (isIntersected)
							{
								intersects.Add(triangle);
							}
						}
					}

					if (intersects.Count > 0)
					{
						var sortedIntersect = SortIntersects(intersects);
						raycasts.Add(paintManager, sortedIntersect);
					}
				}
			}

			if (!isTrianglesDataUpdated)
			{
				isTrianglesDataUpdated = true;
			}
			
			KeyValuePair<IPaintManager, Triangle> closestRaycast = default;
			var minDistance = float.MaxValue;
			foreach (var raycast in raycasts)
			{
				var triangle = raycast.Value;
				var distance = Vector3.Distance(PaintController.Instance.Camera.transform.position, triangle.WorldHit);
				if (distance < minDistance)
				{
					closestRaycast = raycast;
					minDistance = distance;
				}
			}

			if (useCache)
			{
				FrameIntersectionData = new FrameIntersectionData
				{
					PaintManager = closestRaycast.Key, Triangle = closestRaycast.Value, Ray = ray, FrameId = Time.frameCount
				};
			}

			if (closestRaycast.Key != sender)
			{
				return null;
			}
			return closestRaycast.Value;
		}

	    private bool IsBonesTransformsChanged()
	    {
		    var isChanged = false;
		    for (var i = 0; i < bones.Length; i++)
		    {
			    var boneTransform = bones[i];
			    if (boneTransform.localPosition != bonesLocalData.BonesLocalPosition[i] || 
			        boneTransform.localRotation != bonesLocalData.BonesLocalRotation[i] || 
			        boneTransform.localScale != bonesLocalData.BonesLocalScale[i])
			    {
				    bonesLocalData.BonesLocalPosition[i] = boneTransform.localPosition;
				    bonesLocalData.BonesLocalRotation[i] = boneTransform.localRotation;
				    bonesLocalData.BonesLocalScale[i] = boneTransform.localScale;
				    isChanged = true;
			    }
		    }
		    return isChanged;
	    }
    }
}