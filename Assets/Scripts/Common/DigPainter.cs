using UnityEngine;
using UnityEngine.InputSystem;

public class DigPainter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _Surface;
    [SerializeField] private RenderTexture _Mask;
    [SerializeField] private Material _BrushMaterial;
    [SerializeField] private float _BrushSize = 0.03f;
    [SerializeField] private Transform _Fish;

    private Fish _Player;
    private Vector2 _LastUV;
    private bool _HasLastUV;

    private void Start()
    {
        _Player = GetComponent<Fish>();
        ResolveSurface();
        if (_Surface == null)
        {
            Debug.LogError("DigPainter needs a surface SpriteRenderer.", this);
            enabled = false;
            return;
        }

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = _Mask;
        GL.Clear(false, true, Color.black);
        RenderTexture.active = previous;
    }

    private void ResolveSurface()
    {
        SpriteRenderer[] surfaces = FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);
        foreach (SpriteRenderer surface in surfaces)
        {
            Material material = surface.sharedMaterial;
            if (material == null || !material.HasProperty("_DigMask")) continue;
            if (material.GetTexture("_DigMask") != _Mask) continue;

            _Surface = surface;
            return;
        }
    }

    private void LateUpdate()
    {
        if (_Player.IsOnWater)
        {
            _HasLastUV = false;
            return;
        }

        Vector2 position = _Fish.position;
        Bounds bounds = _Surface.bounds;

        float u = (position.x - bounds.min.x) / bounds.size.x;
        float v = (position.y - bounds.min.y) / bounds.size.y;
        Vector2 currentUV = new(u, v);

        if (!_HasLastUV)
        {
            Paint(currentUV);
            _LastUV = currentUV;
            _HasLastUV = true;
            return;
        }

        float distance = Vector2.Distance(_LastUV, currentUV);
        int steps = Mathf.Max(1, Mathf.CeilToInt(distance / _BrushSize));
        for (int i = 1; i <= steps; i++)
            Paint(Vector2.Lerp(_LastUV, currentUV, (float)i / steps));

        _LastUV = currentUV;
    }

    private void Paint(Vector2 uv)
    {
        _BrushMaterial.SetVector("_BrushPosition", uv);
        _BrushMaterial.SetFloat("_BrushSize", _BrushSize);

        RenderTexture temp = RenderTexture.GetTemporary(_Mask.descriptor);

        Graphics.Blit(_Mask, temp, _BrushMaterial, 0);
        Graphics.Blit(temp, _Mask);

        RenderTexture.ReleaseTemporary(temp);
    }
}
