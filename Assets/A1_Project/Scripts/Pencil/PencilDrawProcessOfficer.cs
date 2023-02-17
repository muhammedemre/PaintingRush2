using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using XDPaint.Controllers;
using PathCreation;
using XDPaint;

public class PencilDrawProcessOfficer : MonoBehaviour
{
    [SerializeField] PencilActor pencilActor;
    public float pencilModelContainerXPosAtTheBeginning, pencilMoveSpeed;
    public float positioningDuration;
    [SerializeField] bool canOutline = false, failDrawing = false;
    [SerializeField] float successTolerance;

    float pathOutlinedDistance;

    public bool outlining = false;

    private void Start()
    {
        pencilModelContainerXPosAtTheBeginning = pencilActor.pencilModelOfficer.modelContainer.position.x;
    }

    public void PositionThePen(Vector2 position, float moveDuration,bool withOffset) 
    {
        transform.DOMove(position, moveDuration);

        if (withOffset)
        {
            pencilActor.pencilModelOfficer.modelContainer.DOLocalMoveX(2.4f, positioningDuration);
        }
        else
        {
            if (pencilActor.pencilModelOfficer.modelContainer.localPosition.x != pencilModelContainerXPosAtTheBeginning)
            {
                pencilActor.pencilModelOfficer.modelContainer.DOLocalMoveX(pencilModelContainerXPosAtTheBeginning, positioningDuration);
            }          
        }
    }

    public void StartOutlining() 
    {
        Vector2 normalizedPosition = Camera.main.WorldToScreenPoint(transform.position);
        pencilActor.relatedLevelActor.drawImageActor.paintManagerOutline.PaintObject.OnMouseDown(normalizedPosition);
        pencilActor.pencilModelOfficer.modelContainer.DOLocalMoveX(pencilModelContainerXPosAtTheBeginning, positioningDuration).SetDelay(0f).OnComplete(() => canOutline = true);//.OnUpdate(CheckPosition);//;
    }


    public void StopOutlining() 
    {
        canOutline = false;

        Vector2 normalizedPosition = Camera.main.WorldToScreenPoint(transform.position);
        pencilActor.relatedLevelActor.drawImageActor.paintManagerOutline.PaintObject.OnMouseUp(normalizedPosition);

        if (failDrawing)// if finished
        {
            pencilActor.relatedLevelActor.drawImageActor.CompletedOutlinePart();
            if (pencilActor.relatedLevelActor.drawImageActor.currentDrawState == DrawImageActor.DrawState.Outlining)
            {
                PositionThePen(pencilActor.relatedLevelActor.drawImageActor.GetActivePathBeginningPosition(), positioningDuration, true);
            }           
        }
        else
        {
            pencilActor.pencilModelOfficer.modelContainer.DOLocalMoveX(2.4f, positioningDuration);
        }
    }

    public void DrawOutline() 
    {
        if (!canOutline)
        {
            return;
        }

        pathOutlinedDistance += pencilMoveSpeed * Time.deltaTime;
        pencilActor.relatedLevelActor.drawImageActor.XPosStateChange(false); // bu anime edilecek

        if (pencilActor.relatedLevelActor.drawImageActor.activePath.path.length > pathOutlinedDistance)
        {
            transform.position = pencilActor.relatedLevelActor.drawImageActor.activePath.path.GetPointAtDistance(pathOutlinedDistance, PathCreation.EndOfPathInstruction.Stop);
        }
        else if (pencilActor.relatedLevelActor.drawImageActor.activePath.path.length < pathOutlinedDistance + 1 && !failDrawing)
        {
            pathOutlinedDistance = 0;
            failDrawing = true;
            StartCoroutine(pencilActor.relatedLevelActor.drawImageActor.NodePointPop()); // NodePoint kalkacak, animasyon gelecek
            // outline draw is just finished
        }
        if (failDrawing)
        {          
            FinishedButPlayerKeepsDrawing();
        }
        
        Vector2 normalizedPosition = Camera.main.WorldToScreenPoint(transform.position);
        //print("COLORING1: " + normalizedPosition);
        pencilActor.relatedLevelActor.drawImageActor.paintManagerOutline.PaintObject.OnMouseButton(normalizedPosition);
        //InputController.Instance.DrawWithoutInput(normalizedPosition);
        
    }

    public void StartColoring() 
    {
        Vector2 normalizedPosition = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y));
        pencilActor.relatedLevelActor.drawImageActor.paintManagerOutline.PaintObject.OnMouseDown(normalizedPosition);
        //transform.position = newPos;

        //Vector2 normalizedPosition = Camera.main.WorldToScreenPoint(transform.position);


        //if (pencilActor.relatedLevelActor.drawImageActor.currentImagePart.GetComponent<ImagePartActor>().taraliAlan.activeSelf)
        //{
        //    pencilActor.relatedLevelActor.drawImageActor.currentImagePart.GetComponent<ImagePartActor>().ActivateAndDeactivateTaraliAlan(false);
        //}

    }

    public void StopColoring() 
    {
        if (pencilActor.relatedLevelActor.drawImageActor == null)
        {
            return;
        }
        Vector2 normalizedPosition = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y));
        pencilActor.relatedLevelActor.drawImageActor.paintManagerOutline.PaintObject.OnMouseUp(normalizedPosition);
    }

    public void Coloring() 
    {
        //Vector3 newTouchPos = new Vector3(touchPos.x, touchPos.y, 10f);
        //Vector3 newPos = Camera.main.ScreenToWorldPoint(newTouchPos);
        Vector2 normalizedPosition = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y));
        pencilActor.relatedLevelActor.drawImageActor.paintManagerOutline.PaintObject.OnMouseButton(normalizedPosition);

        //print("COLORING2: " + normalizedPosition);
        //transform.position = newPos;
        //GridVisit(touchPos);
    }

    void FinishedButPlayerKeepsDrawing()
    {
        transform.position = pencilActor.relatedLevelActor.drawImageActor.imagePathOfficer.
            failPathContainer.GetChild(pencilActor.relatedLevelActor.drawImageActor.imageCompleteIndex).GetComponent<PathCreator>().path.GetPointAtDistance(pathOutlinedDistance, PathCreation.EndOfPathInstruction.Stop);
    }

    public void StartNewPath() 
    {
        pathOutlinedDistance = 0;
        failDrawing = false;
    }

    void GridVisit(Vector2 touchPos)
    {
        Vector3 newTouchPos = new Vector3(touchPos.x, touchPos.y, 10f);
        Vector2 drawPos = Camera.main.ScreenToWorldPoint(newTouchPos);
        RaycastHit2D[] hits;
        hits = Physics2D.RaycastAll(drawPos, Vector2.zero, 100.0F);
        Debug.DrawRay(drawPos, Vector2.zero * 100f, Color.red);
        string partName = pencilActor.relatedLevelActor.drawImageActor.currentImagePart.name;
        foreach (RaycastHit2D hit in hits)
        {
            if(hit.transform.GetComponent<GridObjectActor>())
            {
                if (hit.transform.name.Contains(partName))
                {
                    
                    hit.transform.GetComponent<GridObjectActor>().IAmVisited();
                }
            }             
        }
    }

    public void EnableOutlining(bool state)
    {
        outlining = state;
    }

}
