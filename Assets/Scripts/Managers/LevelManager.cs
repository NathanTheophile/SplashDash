using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform[] _levelPrefabs;

    public void SpawnLevel(Scene pGameplayScene)
    {
        Instantiate(GetRandomLevel(), pGameplayScene);
    }

    private Transform GetRandomLevel()
    {
        return _levelPrefabs[Random.Range(0, _levelPrefabs.Length)];
    }
}
