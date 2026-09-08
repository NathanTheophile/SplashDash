#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DashTrail : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _Box;
    [SerializeField, Min(0.05f)] private float _SquareSize;
    [SerializeField] private Color _DebugColor = Color.magenta;

    private Vector2 _StartPosition;
    private Vector2 _Direction;

    public Vector2 Direction => _Direction;

    private void Awake()
    {
        if (_Box == null)
            _Box = GetComponent<BoxCollider2D>();

        _Box.isTrigger = true;
    }

    public void Begin(Vector2 pPosition, Vector2 pDashDirection)
    {
        _StartPosition = pPosition;
        _Direction = pDashDirection.normalized;

        transform.SetPositionAndRotation(pPosition, Quaternion.FromToRotation(Vector2.right, _Direction));
        _Box.size = Vector2.one * _SquareSize;
        _Box.offset = Vector2.right * (_SquareSize * 0.5f);
        
        SetEnd(pPosition);
    }

    public void SetEnd(Vector2 pPosition)
    {
        float lDistance = Mathf.Max(0f, Vector2.Dot(pPosition - _StartPosition, _Direction));
        int lSquareCount = Mathf.Max(1, Mathf.CeilToInt(lDistance / _SquareSize));

        while (transform.childCount < lSquareCount - 1)
        {
            GameObject lSquare = new($"Square {transform.childCount}");
            lSquare.transform.SetParent(transform, false);

            BoxCollider2D lCollider = lSquare.AddComponent<BoxCollider2D>();
            lCollider.isTrigger = true;
            lCollider.size = Vector2.one * _SquareSize;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform lSquare = transform.GetChild(i);
            lSquare.gameObject.SetActive(i < lSquareCount - 1);
            if (i < lSquareCount - 1)
                lSquare.localPosition = Vector2.right * ((i + 1.5f) * _SquareSize);
        }
    }

    private void Update()
    {
        DrawSquare(_Box);

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform lSquare = transform.GetChild(i);
            if (lSquare.gameObject.activeSelf)
                DrawSquare(lSquare.GetComponent<BoxCollider2D>());
        }
    }

    private void DrawSquare(BoxCollider2D pCollider)
    {
        Vector2 lHalfSize = pCollider.size * 0.5f;
        Vector2 lOffset = pCollider.offset;
        Vector3 lTopLeft = pCollider.transform.TransformPoint(lOffset + new Vector2(-lHalfSize.x, lHalfSize.y));
        Vector3 lTopRight = pCollider.transform.TransformPoint(lOffset + new Vector2(lHalfSize.x, lHalfSize.y));
        Vector3 lBottomRight = pCollider.transform.TransformPoint(lOffset + new Vector2(lHalfSize.x, -lHalfSize.y));
        Vector3 lBottomLeft = pCollider.transform.TransformPoint(lOffset + new Vector2(-lHalfSize.x, -lHalfSize.y));

        Debug.DrawLine(lTopLeft, lTopRight, _DebugColor);
        Debug.DrawLine(lTopRight, lBottomRight, _DebugColor);
        Debug.DrawLine(lBottomRight, lBottomLeft, _DebugColor);
        Debug.DrawLine(lBottomLeft, lTopLeft, _DebugColor);
    }
}
