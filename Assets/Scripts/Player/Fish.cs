using UnityEngine;

public class Fish : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Vector2 lAxis = InputManager.Instance.axis;
        transform.position += new Vector3(lAxis.x, lAxis.y) * Time.deltaTime;
    }
}
