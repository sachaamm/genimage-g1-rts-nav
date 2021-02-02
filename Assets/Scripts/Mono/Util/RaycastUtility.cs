using UnityEngine;


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
                    // Select(hit.collider.gameObject);
                    // Ind(hit.point, "start");
                    return hit.point;
                }
            }

            return new Vector3();
        }
    }
