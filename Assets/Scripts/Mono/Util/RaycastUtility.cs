using UnityEngine;

    /// <summary>
    /// Un script utilitaire qui permet de récupérer la position actuelle du Raycast,
    /// en utilisant la MainCamera et la position de la souris via la méthode ScreenPointToRay
    /// </summary>
    public class RaycastUtility : MonoBehaviour
    {
        public static Vector3 RaycastPosition()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green);
            
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {               
                    return hit.point;
                }
            }

            return new Vector3();
        }
    }
