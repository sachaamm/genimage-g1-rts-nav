using DefaultNamespace.Element;
using UnityEngine;

// Le script qui va gérer la partie
public class Game : MonoBehaviour
{
    public static Game Singleton;

    void Awake()
    {
        Singleton = this;    
    }

    void Start()
    {
        CreateTeam(new Vector3(0,0,0));
    }

    void CreateTeam(Vector3 pos)
    {
        ElementManager.Singleton.InstantiateElement(ElementReference.Element.House, pos + new Vector3(0,0,0));
        ElementManager.Singleton.InstantiateElement(ElementReference.Element.Worker, pos + new Vector3(2,0,0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
