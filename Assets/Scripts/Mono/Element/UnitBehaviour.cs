using System;
using System.Resources;
using Mono.Actor;
using Mono.Element;
using Mono.Util;
using Scriptable.Scripts;
using Scriptable.Scripts.Sets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// Script placé sur chaque Element "Unit"
public class UnitBehaviour : MonoBehaviour
    {
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Unit"))
            {
                // Debug.Log("on trigger enter with " + other.name);
                UnitManager.Singleton.EnterNavStuck(gameObject, other.gameObject);
            }
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Unit"))
            {
                UnitManager.Singleton.ExitNavStuck(gameObject);
            }
        }
    }
