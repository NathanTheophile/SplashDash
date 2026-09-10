using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] private int _NextSceneID = 1;
    public void OnButtonStart()
    {
        GameManager.Instance.StartGame();
    }
}
