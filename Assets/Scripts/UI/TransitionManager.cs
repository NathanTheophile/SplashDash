using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    public static bool IsPlaying = false;

    private void Awake()
    {
        Instance = this;
    }
}
