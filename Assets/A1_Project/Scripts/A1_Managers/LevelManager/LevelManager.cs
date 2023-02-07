using XDPaint.Controllers;
public class LevelManager : Manager
{
    public static LevelManager instance;
    public LevelCreateOfficer levelCreateOfficer;
    public LevelMoveOfficer levelMoveOfficer;
    public PaintController paintController;

    public int levelAmount;

    private void Awake()
    {
        SingletonCheck();
    }

    void SingletonCheck()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    public override void GameStartProcess()
    {
        levelCreateOfficer.CreateLevelProcess();
    }

}


