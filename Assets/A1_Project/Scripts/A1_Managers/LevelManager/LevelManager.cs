public class LevelManager : Manager
{
    public static LevelManager instance;
    public LevelCreateOfficer levelCreateOfficer;

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


