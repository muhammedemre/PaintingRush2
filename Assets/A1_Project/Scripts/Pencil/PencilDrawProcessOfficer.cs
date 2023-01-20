using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilDrawProcessOfficer : MonoBehaviour
{
    [SerializeField] PencilActor pencilActor;
    [SerializeField] float pencilModelContainerXPosAtTheBeginning;

    private void Start()
    {
        pencilModelContainerXPosAtTheBeginning = pencilActor.pencilModelOfficer.modelContainer.position.x;
    }
}
