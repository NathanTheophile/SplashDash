#region _____________________________/ INFOS
//  AUTHOR : Splash&Dash (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider2D))]
public class DashTrail : MonoBehaviour
{
    #region _________________________/ REFERENCES
    [FormerlySerializedAs("_Box")]
    [SerializeField] private BoxCollider2D _RootCollider;
    [SerializeField] private SpriteRenderer _VisualRenderer;

    #endregion

    #region _________________________/ TUNING VALUES
    [SerializeField, Min(0.05f)] private float _SquareSize;
    [SerializeField] private Color _DebugColor = Color.magenta;

    #endregion

    #region _________________________/ RUNTIME VALUES

    private Vector2 _StartPosition;
    private Vector2 _Direction;
    private readonly List<BoxCollider2D> _SquareColliders = new();

    public Vector2 Direction => _Direction;

    #endregion

    private void Awake()
    {
        _RootCollider.isTrigger = true;
        _SquareColliders.Add(_RootCollider);
    }

    public void Begin(Vector2 pPosition, Vector2 pDashDirection)
    {
        _StartPosition = pPosition;
        _Direction = pDashDirection.normalized;

        transform.SetPositionAndRotation(pPosition, Quaternion.FromToRotation(Vector2.right, _Direction));
        _RootCollider.size = Vector2.one * _SquareSize;
        _RootCollider.offset = Vector2.right * (_SquareSize * 0.5f);
        
        SetEnd(pPosition);
    }

    public void SetEnd(Vector2 pPosition)
    {
        float lDistance = Mathf.Max(0f, Vector2.Dot(pPosition - _StartPosition, _Direction));
        int lSquareCount = Mathf.Max(1, Mathf.CeilToInt(lDistance / _SquareSize));

        // Keep destroyed slots so extending the dash never recreates a removed square.
        while (_SquareColliders.Count < lSquareCount)
        {
            GameObject lSquare = new GameObject($"Square {_SquareColliders.Count}");
            lSquare.transform.SetParent(transform, false);
            lSquare.transform.localPosition = Vector2.right * ((_SquareColliders.Count + 0.5f) * _SquareSize);

            BoxCollider2D squareCollider = lSquare.AddComponent<BoxCollider2D>();
            squareCollider.isTrigger = true;
            squareCollider.size = Vector2.one * _SquareSize;

            AddVisualRenderer(lSquare);
            _SquareColliders.Add(squareCollider);
        }
    }

    private void AddVisualRenderer(GameObject pSquare)
    {
        SpriteRenderer lRenderer = pSquare.AddComponent<SpriteRenderer>();
        lRenderer.sprite = _VisualRenderer.sprite;
        lRenderer.sharedMaterial = _VisualRenderer.sharedMaterial;
        lRenderer.color = _VisualRenderer.color;
        lRenderer.flipX = _VisualRenderer.flipX;
        lRenderer.flipY = _VisualRenderer.flipY;
        lRenderer.drawMode = _VisualRenderer.drawMode;
        lRenderer.size = _VisualRenderer.size;
        lRenderer.maskInteraction = _VisualRenderer.maskInteraction;
        lRenderer.sortingLayerID = _VisualRenderer.sortingLayerID;
        lRenderer.sortingOrder = _VisualRenderer.sortingOrder;

        Vector2 lSpriteSize = _VisualRenderer.sprite.bounds.size;
        lRenderer.transform.localScale = new Vector3(
            _SquareSize / lSpriteSize.x,
            _SquareSize / lSpriteSize.y,
            1f);
    }

    public void RemoveSquare(Collider2D pCollider)
    {
        // Disable immediately so other players stop receiving its current this frame.
        pCollider.enabled = false;
        // The first square shares the container, which must keep the other squares alive.
        if (pCollider != _RootCollider)
            Destroy(pCollider.gameObject);
    }

    private void Update()
    {
        foreach (BoxCollider2D squareCollider in _SquareColliders)
        {
            if (squareCollider == null || !squareCollider.isActiveAndEnabled) continue;
            DrawSquare(squareCollider);
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
