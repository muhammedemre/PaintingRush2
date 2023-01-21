using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using XDPaint.Controllers;

public class PencilDrawProcessOfficer : MonoBehaviour
{
    [SerializeField] PencilActor pencilActor;
    [SerializeField] float pencilModelContainerXPosAtTheBeginning, pencilMoveSpeed;
    public float positioningDuration;
    [SerializeField] bool canOutline = false;

    float pathOutlinedDistance;

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
        pencilActor.pencilModelOfficer.modelContainer.DOLocalMoveX(pencilModelContainerXPosAtTheBeginning, positioningDuration).OnUpdate(CheckPosition);//.SetDelay(0f).OnComplete(()=> canOutline = true);
    }

    void CheckPosition() 
    {
        if (pencilActor.pencilModelOfficer.modelContainer.localPosition.x == pencilModelContainerXPosAtTheBeginning)
        {
            if (!canOutline)
            {
                canOutline = true;
            }          
        }
    }

    public void StopOutlining() 
    {
        canOutline = false;
        pencilActor.pencilModelOfficer.modelContainer.DOLocalMoveX(2.4f, positioningDuration);
    }

    public void DrawOutline() 
    {
        pathOutlinedDistance += pencilMoveSpeed * Time.deltaTime;
        transform.position = pencilActor.relatedLevelActor.drawImageActor.activePath.path.GetPointAtDistance(pathOutlinedDistance, PathCreation.EndOfPathInstruction.Stop);
        Vector2 normalizedPosition = Camera.main.WorldToScreenPoint(transform.position);
        InputController.Instance.DrawWithoutInput(normalizedPosition);
    }
}
