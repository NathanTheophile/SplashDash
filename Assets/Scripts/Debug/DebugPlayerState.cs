using UnityEngine;
using UnityEngine.UI;

public class DebugPlayerState : MonoBehaviour
{
    [SerializeField] Text stateText;
    Fish player;

    void Update()
    {
        player = FindFirstObjectByType<Fish>();
        stateText.text = "PLAYER STATE : " + player.State;
    }
}
