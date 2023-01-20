using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XDPaint;
using XDPaint.Controllers;

public class LevelActor : MonoBehaviour
{
    public int levelIndex;
    public LevelPreparationOfficer levelPreparationOfficer;
    public LevelCollectedPointsOfficer levelCollectedPointsOfficer;
    public DrawImageActor drawImageActor;
    public PaintManager paintManager;
    public PaintController paintController;
    public PencilActor pencilActor;

}
