using UnityEngine;

public class lololol : MonoBehaviour
{
    void OnDisable()
    {
        Debug.Log($"[{gameObject.name}] désactivé par :\n{System.Environment.StackTrace}", gameObject);
    }
}
