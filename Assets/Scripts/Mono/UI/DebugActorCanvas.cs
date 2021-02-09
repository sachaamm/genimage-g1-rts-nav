using UnityEngine;

// Contraindre la rotation de l'actorCanvas qui contient la barre de vie
public class DebugActorCanvas : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90,0,0);
    }
}
