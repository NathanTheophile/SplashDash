using UnityEngine;

public class PlayerConnectPanelManager : MonoBehaviour
{
    [SerializeField] private PlayerConnectPanel[] _panels;

    public void EditPanel(int id, InputMode usedInput)
    {
        if (id >= _panels.Length) return;

        _panels[id].SetPanel(id, usedInput);
    }

    public void RemovePanel(int id)
    {
        if (id >= _panels.Length) return;

        _panels[id].RemovePanel();
    }
}