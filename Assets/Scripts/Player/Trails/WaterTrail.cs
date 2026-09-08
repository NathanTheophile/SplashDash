#region _____________________________/ INFOS
//  AUTHOR : Splash&Dash (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyCallback, MyStruct
#endregion

using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class WaterTrail : MonoBehaviour
{
    #region _________________________/ TUNING VALUES
    [SerializeField] private Color _DebugColor = Color.cyan;

    #endregion

    #region _________________________/ REFERENCES
    private CircleCollider2D _Collider;

    #endregion

    private void Awake()
    {
        _Collider = GetComponent<CircleCollider2D>();
        _Collider.isTrigger = true;
    }

    private void Update()
    {
        Vector3 lCenter = _Collider.transform.TransformPoint(_Collider.offset);
        float lRadius = _Collider.radius * transform.lossyScale.x;
        const int lSegments = 16;

        for (int i = 0; i < lSegments; i++)
        {
            float lCurrentAngle = i * Mathf.PI * 2f / lSegments;
            float lNextAngle = (i + 1) * Mathf.PI * 2f / lSegments;
            Vector3 lCurrentPoint = lCenter + new Vector3(Mathf.Cos(lCurrentAngle), Mathf.Sin(lCurrentAngle)) * lRadius;
            Vector3 lNextPoint = lCenter + new Vector3(Mathf.Cos(lNextAngle), Mathf.Sin(lNextAngle)) * lRadius;

            Debug.DrawLine(lCurrentPoint, lNextPoint, _DebugColor);
        }
    }
}
