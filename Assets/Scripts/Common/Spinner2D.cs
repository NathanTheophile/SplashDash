using UnityEngine;

public class Spinner2D : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    void Update()
    {
        float rotation = speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, rotation) * transform.rotation;
    }
}
