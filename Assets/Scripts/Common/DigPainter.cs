using UnityEngine;
using UnityEngine.InputSystem;

public class DigPainter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _Surface;
    [SerializeField] private RenderTexture _Mask;
    [SerializeField] private Material _BrushMaterial;
    [SerializeField] private float _BrushSize = 0.02f;
    [SerializeField] private Transform _Fish;

    private void Start()
    {
        if (_Surface == null)
            _Surface = GameObject.FindWithTag("Sand").GetComponent<SpriteRenderer>();
            
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = _Mask;
        GL.Clear(false, true, Color.black);
        RenderTexture.active = previous;
    }

    private void Update()
    {
        Vector2 position = _Fish.position;
        Bounds bounds = _Surface.bounds;

        float u = (position.x - bounds.min.x) / bounds.size.x;
        float v = (position.y - bounds.min.y) / bounds.size.y;


        Paint(new Vector2(u, v));
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
