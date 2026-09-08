#region _____________________________/ INFOS
//  AUTHOR : Splash&Dash (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyCallback, MyStruct
#endregion

using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    #region _________________________/ REFERENCES
    [SerializeField] private PlayerMovement _PlayerMovementSystem;
    [SerializeField] private PlayerTrailEmitter _PlayerTrailEmitter;

    #endregion

    #region _________________________/ TUNING VALUES
    [SerializeField, Min(0.01f)] private float _DashDuration = 1.5f;

    #endregion

    #region _________________________/ RUNTIME VALUES
    private bool canDash = true;

    public bool IsDashing { get; private set; }
    public bool CanDash => canDash && _PlayerMovementSystem.CanControl;

    #endregion

    #region _________________________| INIT

    private void Awake()
    {
        if (_PlayerMovementSystem != null && _PlayerMovementSystem.DashStats != null) return;
        enabled = false;
    }

    #endregion

    #region _________________________| GAME FLOW METHODS

    public bool TryDash()
    {
        if (!CanDash) return false;
        PlayerStats stats = _PlayerMovementSystem.DashStats;
        Vector2 direction = transform.up;
        _PlayerMovementSystem.QueueImpulse(direction * stats.jumpForce);
        canDash = false;
        IsDashing = true;
        _PlayerMovementSystem.SetMoveInput(Vector2.zero);
        if (_PlayerTrailEmitter != null && _PlayerTrailEmitter.isActiveAndEnabled) _PlayerTrailEmitter.BeginDash(direction);
        StartCoroutine(DashCooldownCoroutine(stats.jumpCooldown));
        StartCoroutine(DashDurationCoroutine());
        return true;
    }

    private System.Collections.IEnumerator DashCooldownCoroutine(float cooldown)
    {
        yield return new WaitForSeconds(Mathf.Max(0, cooldown));
        canDash = true;
    }

    private System.Collections.IEnumerator DashDurationCoroutine()
    {
        yield return new WaitForSeconds(Mathf.Max(0.01f, _DashDuration));
        EndDash();
    }

    private void EndDash()
    {
        IsDashing = false;
        if (_PlayerTrailEmitter != null) _PlayerTrailEmitter.EndDash();
    }

    public void ResetDash()
    {
        EndDash();
        canDash = true;
        if (_PlayerMovementSystem != null) _PlayerMovementSystem.ClearInput();
    }

    private void OnDisable() => ResetDash();

    #endregion
}
