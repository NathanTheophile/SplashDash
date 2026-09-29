using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    private GameObject _myPrefab;
    void Start()
    {
        Scene myScene = SceneManager.GetSceneByName("");
        GameObject lValidObject = null;
        foreach (GameObject gameObject in myScene.GetRootGameObjects())
        {
            if (gameObject.name == "")
            {
                lValidObject = gameObject;
                break;
            }
        }
        if (lValidObject == null) return;
        Instantiate(_myPrefab, lValidObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
