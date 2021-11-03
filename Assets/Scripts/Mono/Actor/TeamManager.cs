using System;
using RotaryHeart.Lib.SerializableDictionary;
using Scriptable.Scripts;
using UnityEngine;

namespace Mono.Actor
{
    public class TeamManager : MonoBehaviour
    {
        public static TeamManager Singleton;

        [SerializeField] public TeamDictionary teamDictionary;
        
        [System.Serializable]
        public class TeamDictionary : SerializableDictionaryBase<byte, Color>
        {
        }
        
        private void Awake()
        {
            Singleton = this;
        }
        
        
    }
}