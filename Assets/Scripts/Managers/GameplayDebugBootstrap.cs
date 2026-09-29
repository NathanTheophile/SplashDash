using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayDebugBootstrap : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Create()
    {
        if (SceneManager.GetActiveScene().name != "GameplayDebug") return;

        new GameObject("GameplayDebugBootstrap").AddComponent<GameplayDebugBootstrap>();
    }

    private void Start()
    {
        Scene lGameplayScene = SceneManager.GetActiveScene();
        PlayerManager.Instance.SetupGameplay(lGameplayScene);
        FindFirstObjectByType<LevelManager>().SpawnLevel(lGameplayScene);
        InputManager.Instance.EnableDeviceConnection(true);
    }
}
