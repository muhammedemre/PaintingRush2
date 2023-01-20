using UnityEngine;
public class CameraManager : Manager
{
    public static CameraManager instance;
    public CameraActor cameraActor;
    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    public GameObject inGameCamera, finishSceneCamera;

    public enum CameraState     
    {
        inGame, Finish
    }

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
        CameraManager.instance.ActivateTheCamera(CameraManager.CameraState.inGame);
    }
    public override void PreLevelEndProcess()
    {
        CameraManager.instance.ActivateTheCamera(CameraManager.CameraState.Finish);
    }

    public void ActivateTheCamera(CameraState state) 
    {
        if (state == CameraState.inGame)
        {
            inGameCamera.SetActive(true);
            finishSceneCamera.SetActive(false);
        }
        else if (state == CameraState.Finish)
        {
            finishSceneCamera.SetActive(true);
            inGameCamera.SetActive(false);
        }
    }
}
