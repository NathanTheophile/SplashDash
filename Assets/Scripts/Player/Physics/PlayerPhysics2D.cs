#region _____________________________/ INFOS
//  AUTHOR : Splash&Dash (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics2D : MonoBehaviour
{
    #region _________________________/ REFERENCES
    private readonly Dictionary<Collider2D, WaterTrail> _WaterContactsByCollider = new();
    private readonly Dictionary<Collider2D, Vector2> _CurrentDirectionsByCollider = new();
    private readonly HashSet<Collider2D> _DeadzoneContacts = new();
    private Collider2D _SelectedCurrentCollider;
    private Fish _Fish;
    private PlayerTrailEmitter _TrailEmitter;

    #endregion

    #region _________________________/ STATE VALUES

    public bool IsOnWater => HasWaterOverlap();
    public Vector2 CurrentDirection => GetCurrentDirection();
    public bool IsInDeadzone => _DeadzoneContacts.Count > 0;
    public float DeadzoneRatio => Mathf.Clamp01(_DeadzoneTimer / _DeadzoneDuration);
    public event Action OnPlayerDied;
    private int _LastEnemyIndex = -1;

    #endregion

    #region _________________________/ TUNING VALUES

    [SerializeField, Min(0.1f)] private float _DeadzoneDuration = 3f;

    #endregion

    #region _________________________/ RUNTIME VALUES

    private float _DeadzoneTimer;
    private bool _IsDead;

    #endregion

    private void OnDisable() => ClearContacts();

    private void Awake()
    {
        _Fish = GetComponent<Fish>();
        _TrailEmitter = GetComponent<PlayerTrailEmitter>();
    }

    private void Update()
    {
        if (!IsInDeadzone || _IsDead) return;

        _DeadzoneTimer += Time.deltaTime;
        if (_DeadzoneTimer >= _DeadzoneDuration)
            Kill();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Deadzone"))
        {
            _DeadzoneContacts.Add(other);
            return;
        }

        DashTrail trail = other.GetComponentInParent<DashTrail>();
        if (trail != null)
        {
            HandleCurrentContact(other, trail);
            return;
        }

        WaterTrail waterTrail = other.GetComponent<WaterTrail>();
        if (other.CompareTag("Water") || waterTrail != null)
            _WaterContactsByCollider[other] = waterTrail;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        DashTrail trail = other.GetComponentInParent<DashTrail>();
        if (trail != null)
            HandleCurrentContact(other, trail);
    }

    private void HandleCurrentContact(Collider2D pCollider, DashTrail pTrail)
    {
        if (_Fish.IsDashing && pTrail != _TrailEmitter.ActiveDash)
        {
            pTrail.RemoveSquare(pCollider);
            _CurrentDirectionsByCollider.Remove(pCollider);
            return;
        }

        if (!_Fish.IsDashing)
            _CurrentDirectionsByCollider[pCollider] = pTrail.Direction;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_DeadzoneContacts.Remove(other) && _DeadzoneContacts.Count == 0)
            ResetDeadzone();

        _CurrentDirectionsByCollider.Remove(other);
        _WaterContactsByCollider.Remove(other);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Fish otherFish = collision.collider.GetComponentInParent<Fish>();
        if (otherFish != null)
        {
            _LastEnemyIndex = otherFish.FishIndex;
            if (_Fish.IsDashing)
            {
                otherFish.Stun();
                RuntimeManager.PlayOneShot("event:/SFX/Dash/Hit", transform.position);
            }

            return;
        }

        if (collision.collider.CompareTag("Obstacle") && _Fish.IsDashing)
            _Fish.Stun();
    }
    
    

    public void ClearContacts()
    {
        _WaterContactsByCollider.Clear();
        _CurrentDirectionsByCollider.Clear();
        _DeadzoneContacts.Clear();
        _SelectedCurrentCollider = null;
        ResetDeadzone();
    }

    private void ResetDeadzone()
    {
        _DeadzoneTimer = 0f;
    }

    private void Kill()
    {
        if (_IsDead) return;

        RuntimeManager.PlayOneShot("event:/SFX/Death", transform.position);
        _IsDead = true;
        _Fish.PlayerDeath(_LastEnemyIndex);
    }

    private bool HasWaterOverlap()
    {
        foreach (var waterContact in _WaterContactsByCollider)
        {
            if (waterContact.Key == null) continue;
            if (waterContact.Value != null && _TrailEmitter.IsRecentTrail(waterContact.Value)) continue;
            return true;
        }

        return false;
    }

    private Vector2 GetCurrentDirection()
    {
        if (_SelectedCurrentCollider != null && _SelectedCurrentCollider.isActiveAndEnabled &&
            _CurrentDirectionsByCollider.TryGetValue(_SelectedCurrentCollider, out Vector2 currentDirection))
            return currentDirection;

        _SelectedCurrentCollider = null;
        foreach (var currentContact in _CurrentDirectionsByCollider)
        {
            Collider2D currentCollider = currentContact.Key;
            if (currentCollider == null || !currentCollider.isActiveAndEnabled) continue;
            _SelectedCurrentCollider = currentCollider;
            return currentContact.Value;
        }

        return Vector2.zero;
    }
}
