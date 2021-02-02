using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugActorCanvas : MonoBehaviour
{
    
    
    void Start()
    {
        
    }

    
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90,0,0);
    }
}
