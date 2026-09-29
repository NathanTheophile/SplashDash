using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public VisualKillSetter KillSetter { get => _killerSetter; private set => _killerSetter = value; }

    [SerializeField] private VisualKillSetter _killerSetter;
    [SerializeField] private TextMeshProUGUI _timeText;

    public void SetTime(int time)
    {
        _timeText.text = $"{time} s";
    }
}
