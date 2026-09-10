#region _____________________________/ INFOS
//  AUTHOR : Splash&Dash (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyCallback, MyStruct
#endregion

using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    #region _________________________/ REFERENCES
    [SerializeField] private PlayerMovement _PlayerMovementSystem;
    [SerializeField] private PlayerTrailEmitter _PlayerTrailEmitter;
    [SerializeField] private FishAnimator _PlayerAnimator;
    [SerializeField] private Slider _DashChargeSlider;

    #endregion

    #region _________________________/ TUNING VALUES
    [SerializeField] private bool _UseChargedDash;
    [SerializeField, Min(0.01f)] private float _MinDashDuration = 0.2f;
    [SerializeField, Min(0.01f)] private float _MaxDashDuration = 0.6f;
    [SerializeField, Min(0.01f)] private float _MaxChargeDuration = 1f;

    #endregion

    #region _________________________/ RUNTIME VALUES
    private bool _IsOnCooldown;
    private float _ChargeStartTime;

    public bool IsCharging { get; private set; }
    public bool IsDashing { get; private set; }

    public float ChargeRatio => GetChargeRatio();

    public bool CanDash => CanStartDash();

    private FMOD.Studio.EventInstance _chargeEvent;

    #endregion

    #region _________________________| INIT

    private void Awake()
    {
        if (_PlayerMovementSystem == null)
            _PlayerMovementSystem = GetComponent<PlayerMovement>();

        if (_PlayerTrailEmitter == null)
            _PlayerTrailEmitter = GetComponent<PlayerTrailEmitter>();

        ConfigureDashChargeSlider();
    }

    private void Update()
    {
        if (!IsCharging) return;

        _PlayerMovementSystem.SetChargeSpeed(ChargeRatio);
        _PlayerAnimator.SetCharge(ChargeRatio);

        if (_DashChargeSlider == null) return;

        _DashChargeSlider.value = Mathf.Lerp(
            _MinDashDuration,
            _MaxDashDuration,
            ChargeRatio);
    }

    #endregion

    #region _________________________| GAME FLOW METHODS

    private void Start()
    {
        _chargeEvent = RuntimeManager.CreateInstance("event:/SFX/Dash/Charge");
        RuntimeManager.AttachInstanceToGameObject(_chargeEvent, gameObject);
    }

    public void PressDash()
    {
        if (!CanDash)
            return;

        if (!_UseChargedDash)
        {
            if (_chargeEvent.isValid())
            {
                _chargeEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }

            StartDash(_MinDashDuration);
            return;
        }

        _chargeEvent.start();
        IsCharging = true;
        _ChargeStartTime = Time.time;
        _PlayerMovementSystem.SetChargeSpeed(0f);
        ShowDashChargeSlider();
    }

    public void ReleaseDash()
    {
        if (!IsCharging)
            return;

        float lDashDuration = GetDashDuration();

        IsCharging = false;
        _PlayerMovementSystem.ResetChargeSpeed();
        _PlayerAnimator.SetCharge(0f);
        HideDashChargeSlider();
        StartDash(lDashDuration);
    }

    public void CancelCharge()
    {
        if (!IsCharging)
            return;

        _chargeEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        IsCharging = false;
        _ChargeStartTime = 0f;
        _PlayerMovementSystem.ResetChargeSpeed();
        HideDashChargeSlider();
    }

    private float GetDashDuration() =>
        Mathf.Lerp(_MinDashDuration, _MaxDashDuration, ChargeRatio);

    private float GetChargeRatio()
    {
        if (!IsCharging) return 0f;
        return Mathf.Clamp01((Time.time - _ChargeStartTime) / _MaxChargeDuration);
    }

    private bool CanStartDash()
    {
        if (_IsOnCooldown) return false;
        if (IsCharging) return false;
        if (IsDashing) return false;
        return _PlayerMovementSystem.AreControlsEnabled;
    }

    private void StartDash(float pDashDuration)
    {
        PlayerStats lStats = _PlayerMovementSystem.DashStats;
        Vector2 lDirection = transform.up;

        if(pDashDuration == 1)
        {
            RuntimeManager.PlayOneShot("event:/SFX/Dash/Big", transform.position);
        }
        else
        {
            RuntimeManager.PlayOneShot("event:/SFX/Dash/Normal", transform.position);
        }

        _IsOnCooldown = true;
        IsDashing = true;
    
        _PlayerMovementSystem.BeginDash(lDirection, lStats.maxMoveSpeed);
        _PlayerMovementSystem.SetMoveInput(Vector2.zero);
    
        _PlayerTrailEmitter.BeginDash(lDirection);
    
        StartCoroutine(DashCooldownCoroutine(lStats.jumpCooldown));
        StartCoroutine(DashDurationCoroutine(pDashDuration));
    }

    private IEnumerator DashCooldownCoroutine(float pCooldown)
    {
        yield return new WaitForSeconds(Mathf.Max(0f, pCooldown));
        _IsOnCooldown = false;
    }

    private IEnumerator DashDurationCoroutine(float pDashDuration)
    {
        yield return new WaitForSeconds(Mathf.Max(0.01f, pDashDuration));
        EndDash();
    }

    private void EndDash()
    {
        IsDashing = false;
        _PlayerMovementSystem.EndDash();
        _PlayerTrailEmitter.EndDash();
    }

    public void ResetDash()
    {
        StopAllCoroutines();

        IsCharging = false;
        _ChargeStartTime = 0f;
        _PlayerMovementSystem.ResetChargeSpeed();
        _IsOnCooldown = false;
        HideDashChargeSlider();

        EndDash();

        _PlayerMovementSystem.ClearInput();
    }

    private void OnDisable() => ResetDash();

    private void ConfigureDashChargeSlider()
    {
        if (_DashChargeSlider == null) return;

        _DashChargeSlider.minValue = _MinDashDuration;
        _DashChargeSlider.maxValue = _MaxDashDuration;
        _DashChargeSlider.value = _MinDashDuration;
        _DashChargeSlider.gameObject.SetActive(false);
    }

    private void ShowDashChargeSlider()
    {
        if (_DashChargeSlider == null) return;

        _DashChargeSlider.value = _MinDashDuration;
        _DashChargeSlider.gameObject.SetActive(true);
    }

    private void HideDashChargeSlider()
    {
        if (_DashChargeSlider != null)
            _DashChargeSlider.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        if (_chargeEvent.isValid())
        {
            _chargeEvent.release();
        }
    }
    #endregion
}
