using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RtsCamera : MonoBehaviour
{
    public Vector3 MousePos;
    public float moveSpeed;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        MousePos = Input.mousePosition;
        float width = Screen.width;
        float height = Screen.height;

        if ( MousePos.x <= (Screen.width / 10) )
        {
            Scroll(new Vector3(1, 0, 0));
        }

        if ( MousePos.x >= (Screen.width - Screen.width / 10) )
        {
            Scroll(new Vector3(-1, 0, 0));
        }

    }
    void Scroll(Vector3 direction)
    {
        transform.position += direction * moveSpeed;
    }
}
