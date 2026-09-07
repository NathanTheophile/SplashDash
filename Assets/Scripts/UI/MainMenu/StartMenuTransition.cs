using UnityEngine;

public class StartMenuTransition : MonoBehaviour
{
    [SerializeField] private GameObject _nextMenu;

    public void Transition()
    {
        if (!gameObject.activeSelf) return;
        gameObject.SetActive(false);
        _nextMenu.SetActive(true);
    }
}
