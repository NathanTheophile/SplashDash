#region _____________________________/ INFOS
//  AUTHOR : Splash&Dash (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyCallback, MyStruct
#endregion

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fish : MonoBehaviour
{
    #region _________________________/ REFERENCES
    [SerializeField] private PlayerMovement _PlayerMovementSystem;
    [SerializeField] private PlayerDash _PlayerDashSystem;
    [SerializeField] private PlayerPhysics2D _PlayerPhysicsSystem;
    [SerializeField] private PlayerPhysicsStates _PlayerPhysicsStates;
    [SerializeField] private FishAnimator _PlayerAnimator;
    [SerializeField, Min(0.01f)] private float _StunDuration = 0.5f;

    public int FishIndex;

    #endregion

    #region _________________________/ STATE VALUES

    public bool IsOnWater => _PlayerPhysicsSystem.IsOnWater;
    public bool IsDashing => _PlayerDashSystem.IsDashing;
    public bool IsCharging => _PlayerDashSystem.IsCharging;
    public bool IsStunned { get; private set; }
    public Vector2 CurrentDirection => _PlayerPhysicsSystem.CurrentDirection; 
    public bool AreControlsEnabled => !IsStunned && !IsDashing;
    public PlayerStates State => GetState();
                    
    public PlayerStats CurrentStats => GetStats(State);

    public static event Action<int,int> OnPlayerDeath;

    #endregion

    #region _________________________| INIT

    public void SetFishIndex(int pFishIndex)
    {
        FishIndex = pFishIndex;
        _PlayerAnimator.SetSkin(FishIndex);
    }

    public PlayerStats GetStats(PlayerStates state) => _PlayerPhysicsStates.GetStats(state);

    private PlayerStates GetState()
    {
        if (IsStunned) return PlayerStates.STUNNED;
        if (IsDashing) return PlayerStates.IS_DASHING;
        if (IsOnWater) return PlayerStates.ON_WATER;
        return PlayerStates.ON_SAND;
    }

    public void SetStunned(bool stunned)
    {
        IsStunned = stunned;
        if (!stunned) return;
        _PlayerAnimator.RemoveStun();
        _PlayerDashSystem.CancelCharge();
        _PlayerMovementSystem.SetMoveInput(Vector2.zero);
    }

    private Coroutine _StunRoutine;

    public void Stun()
    {
        SetStunned(true);

        if (_StunRoutine != null)
            StopCoroutine(_StunRoutine);

        _StunRoutine = StartCoroutine(ClearStunAfterDelay());
    }

    private IEnumerator ClearStunAfterDelay()
    {
        yield return new WaitForSeconds(_StunDuration);
        SetStunned(false);
        _PlayerAnimator.RemoveStun();
        _StunRoutine = null;
    }

    public void PlayerDeath(int pKillerIndex) => OnPlayerDeath.Invoke(FishIndex, pKillerIndex);

    #endregion

    #region _________________________| UNITY

    private Vector2 _MoveDirection;

    private void Update()
    {
        _PlayerMovementSystem.SetMoveInput(_MoveDirection);
        _PlayerAnimator.SetSpeed(CurrentStats.moveSpeed * _MoveDirection.magnitude, 1f);
    }

    #endregion

    #region _________________________| INPUTS

    public void HandleMove(InputAction.CallbackContext pContext)
    {
        _MoveDirection = pContext.ReadValue<Vector2>();
    }

    public void PressDash(InputAction.CallbackContext pContext)
    {
        if (AreControlsEnabled && pContext.performed) _PlayerDashSystem.PressDash();
        else if (pContext.canceled) _PlayerDashSystem.ReleaseDash();
    }

    private void OnDisable()
    {
        _StunRoutine = null;
        IsStunned = false;
        _MoveDirection = Vector2.zero;
        _PlayerMovementSystem.ClearInput();
        _PlayerDashSystem.ResetDash();
    }

    #endregion
}
