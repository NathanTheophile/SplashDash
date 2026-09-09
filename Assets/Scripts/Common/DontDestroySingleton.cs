using UnityEngine;

/* User manual :
 * 
 * - First create a prfab in the ressources folder (any subfolder works)
 * 
 * - Then put this component on the prefab created and change the PATH string
 * for the actual path of the prefab inside the ressources folder (the path does no include
 * extensions or previous folders, just the subfolders and the name of the prefab)
 * 
 * - Add any other components that you want onto the prefab, they will be loaded before
 * the scene start, only once, and won't me destroyed on scene change (permanent components)
 */

public class DontDestroySingleton : MonoBehaviour
{
    // Change this fo the prefab path in the ressources folder
    private const string PATH = "Autoload";

    [HideInInspector] public static DontDestroySingleton Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInitialize()
    {
        if (Instance != null) return;

        GameObject prefab = Resources.Load<GameObject>(PATH);

        if (prefab != null)
        {
            Instantiate(prefab);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Ce que tu veux, perso j'y toucherai pas trop pour la compatibilté
        // après c'est toi qui vois hein, je vais pas te tenir la main t'es grand
        // maintenant, mais bon c'est censé etre un composant posé sur un prefab
        // et après ce prefab il a des composant qui ont besoin d'être en autoload
        // mais bon j'imagine que tu sais mieux que moi HEIN voila c'est bon t'as
        // gagné je m'en vais moi
    }
}
