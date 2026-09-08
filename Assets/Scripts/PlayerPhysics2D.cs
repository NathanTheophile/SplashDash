#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;

public class PlayerPhysics2D : MonoBehaviour
{
    private int _WaterContacts;
    private Fish _Fish;
    private PlayerTrailEmitter _TrailEmitter;

    public bool IsOnWater => _WaterContacts > 0;

    private void Awake()
    {
        _Fish = GetComponent<Fish>();
        _TrailEmitter = GetComponent<PlayerTrailEmitter>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water") || other.GetComponent<WaterTrail>() != null)
        {
            _WaterContacts++;
            UpdateWaterState();
        }
        else if (_Fish != null && _Fish.IsDashing)
        {
            DashTrail lTrail = other.GetComponentInParent<DashTrail>();
            if (lTrail != null && (_TrailEmitter == null || lTrail != _TrailEmitter.ActiveDash))
                Destroy(lTrail.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water") || other.GetComponent<WaterTrail>() != null)
        {
            _WaterContacts = Mathf.Max(0, _WaterContacts - 1);
            UpdateWaterState();
        }
    }

    private void UpdateWaterState()
    {
        if (_Fish != null)
            _Fish.SetOnWater(IsOnWater);
    }

    public void ClearContacts()
    {
        _WaterContacts = 0;
        UpdateWaterState();
    }
}
