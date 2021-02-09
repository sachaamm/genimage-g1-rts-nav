using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Le script de la caméra
public class RtsCamera : MonoBehaviour
{
    public Vector3 MousePos;
    public float moveSpeed;
    public float boundy;
    
    void Update()
    {   
        MousePos = Input.mousePosition;
        float width = Screen.width;
        float height = Screen.height;

        if ( MousePos.x <= (Screen.width / 10) && MousePos.x >= 0 )
        {
            Scroll(new Vector3(-1, 0, 0));
        }

        if ( MousePos.x >= (Screen.width - Screen.width / 10) && MousePos.x <= Screen.width)
        {
            Scroll(new Vector3(1, 0, 0));
        }
        if (MousePos.y <= (Screen.height / 10) && MousePos.y >= 0 )
        {
            Scroll(new Vector3(0, 0, -1));
        }
  
        boundy = Screen.height - Screen.height / 10 ;
        if (MousePos.y >= (Screen.height - Screen.height / 10) && MousePos.y <= Screen.height)
        {
            Scroll(new Vector3(0, 0, 1));
        }
        
        
        
    }
    void Scroll(Vector3 direction)
    {
        transform.position += direction * moveSpeed;
    }
}
