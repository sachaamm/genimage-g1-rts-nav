using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Selection
{
    public class RectSelector : MonoBehaviour
    {
        public GameObject rectSelector;

        public RectTransform rectSelectorTransform;
        
        private Vector3 startMousePos; // La position de la souris au moment ou on a commencé à cliquer
        private Vector3 startMouseWorldPos;

        public Transform elementsParent;

        public GameObject cube;
        
        private void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                startMousePos = Input.mousePosition;
                
                startMouseWorldPos = RaycastPosition();
                
            }
            
            if (Input.GetMouseButton(0))
            {
                rectSelector.SetActive(true);
                DrawRect();
            }
            else
            {
                rectSelector.SetActive(false);
            }

            if (Input.GetMouseButtonUp(0))
            {
                List<GameObject> unitsInRect = UnitsInRect();
                Debug.Log(unitsInRect.Count);
                
                Selection.Singleton.ReceiveSelection(unitsInRect);
                UiManager.Singleton.UpdateGroupLayout(unitsInRect);
            }
            
        }

        Vector3 RaycastPosition()
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

        void DrawRect()
        {
            
            Vector2 delta = startMousePos - Input.mousePosition;

            float width = delta.x;
            float absWidth = Mathf.Abs(delta.x);
            float height = delta.y;
            float absHeight = Mathf.Abs(delta.y);
            
            // On va définir la taille du rectangle selon ou on a cliqué
            rectSelectorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, absHeight);
            rectSelectorTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, absWidth);


            rectSelectorTransform.anchoredPosition = startMousePos -
                                                     new Vector3(Screen.width / 2, Screen.height / 2, 0)
                                                     -
                                                     new Vector3(width/2, height/2, 0);


        }


        List<GameObject> UnitsInRect()
        {
            List<GameObject> unitsInRect = new List<GameObject>();
            
            Vector3 screenPosToWorldPoint = RaycastPosition();

            // le point minimum entre screenPosToWorldPoint et startMouseWorldPos
            // le point maximum entre screenPosToWorldPoint et startMouseWorldPos

            Vector3 min = Vector3.Min(startMouseWorldPos, screenPosToWorldPoint);
            Vector3 max = Vector3.Max(startMouseWorldPos, screenPosToWorldPoint);
            
            foreach (Transform child in elementsParent)
            {
                if (InBounds(min, max, child.position))
                {
                    unitsInRect.Add(child.gameObject);
                }
            }
            
            return unitsInRect;
        }


        bool InBounds(Vector3 min, Vector3 max, Vector3 pos)
        {
            if (pos.x > min.x && pos.x < max.x && pos.z > min.z && pos.z < max.z) return true;
            return false;
        }

        
    }
}