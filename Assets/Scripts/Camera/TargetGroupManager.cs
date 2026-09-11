using Unity.Cinemachine;
using UnityEngine;


public class TargetGroupManager : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup _TargetGroup;
    [SerializeField] private float _DefaultWeight = 1f;

    [SerializeField] private float _DefaultRadius = 2f;

    void Awake()
    {
        if(_TargetGroup ==null)
        {
            _TargetGroup = FindFirstObjectByType<CinemachineTargetGroup>();
        }
    }

    public void RegisterTarget(Transform target)
    {
        if (target == null || _TargetGroup == null)
            return;
        _TargetGroup.AddMember(target, _DefaultWeight, _DefaultRadius);
    }

    public void UnregisterTarget(Transform target)
    {
        if (target == null || _TargetGroup)
            return;

        _TargetGroup.RemoveMember(target);
    }


}



