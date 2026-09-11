using UnityEngine;
using Unity.Cinemachine;
using UnityEditor;

public class TargetGroupAutoRegister : MonoBehaviour
{
    private TargetGroupManager _Manager;

    void Start()
    {
        _Manager = FindFirstObjectByType<TargetGroupManager>();

        if (_Manager != null)
        {
            _Manager.RegisterTarget(transform);
        }
    }

    void OnDestroy()
    {
        if (_Manager != null)
        {
            _Manager.UnregisterTarget(transform);
        }
    }
}