using System.Collections.Generic;
using DefaultNamespace.Element;
using Mono.Actor;
using Mono.Entity;
using RotaryHeart.Lib.SerializableDictionary;
using Scriptable.Scripts;
using UnityEngine;
using UnityEngine.UI;


// Il va gérer l'apparition des enemy. Il contient la liste _enemies, contenant toutes les données concernant les enemy
public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Singleton;
        
        // Le dictionnaire qui contient les scriptableObjects correspondant aux ennemis.
        [SerializeField] private EnemyDictionary _enemyDictionary;

        // Un dictionnaire d'ennemi associe un EnemyType à un EnemyScriptable
        [System.Serializable]
        public class EnemyDictionary : SerializableDictionaryBase<EnemyReference.EnemyType, EnemyScriptable>
        {
        }
        
        
        public float intervalCreationEnemy = 10;
        private float compteurCreationEnemy;

        public bool autoCreateEnemy = false;

        // La classe permettant de regrouper toutes les données nécessaires ( ex: gameObject, health, type d'ennemi etc...)
        public class EnemyWithHealth
        {
            public GameObject enemyGameObject;
            private int enemyHealth;

            private EnemyReference.EnemyType EnemyType;
            private Image healthImg;

            public EnemyWithHealth(GameObject go, EnemyReference.EnemyType enemyType, Image _healthImg)
            {
                enemyGameObject = go;
                EnemyScriptable enemyScriptable = EnemyManager.GetEnemyScriptable(enemyType);
                enemyHealth = enemyScriptable.MaxHealth;
                EnemyType = enemyType;
                healthImg = _healthImg;

            }

            public void AddHealthAmount(int amount)
            {
                enemyHealth += amount;
                
                EnemyScriptable elementScriptable = EnemyManager.GetEnemyScriptable(EnemyType);
                float ratioLife = enemyHealth / (float) elementScriptable.MaxHealth;
                healthImg.fillAmount = ratioLife;

                if (enemyHealth <= 0)
                {
                    EnemyManager.Singleton.RemoveEnemy(this);
                }
            }
            
        }

        public List<EnemyWithHealth> _enemies = new List<EnemyWithHealth>();
        
        private void Awake()
        {
            Singleton = this;
        }

        void Update()
        {
            if(autoCreateEnemy) AutoCreate();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CreateEnemy();
            }
        }

        void AutoCreate()
        {
            compteurCreationEnemy += Time.deltaTime;

            if (compteurCreationEnemy >= intervalCreationEnemy)
            {
                compteurCreationEnemy = 0;
                
            }
        }

        void CreateEnemy()
        {
            InstantiateEnemy(EnemyReference.EnemyType.BigMonster, RandomSpawnPos());
        }

        public void RemoveEnemy(EnemyWithHealth enemyWithHealth)
        {
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                if (_enemies[i] == enemyWithHealth)
                {
                    Destroy(enemyWithHealth.enemyGameObject);
                    _enemies.Remove(enemyWithHealth);
                }
            }
        }

        public Vector3 RandomSpawnPos()
        {
            float e = 10 * 50;
            return new Vector3(Random.Range(-e, e), 0, Random.Range(-e,e));
        }

        public static EnemyScriptable GetEnemyScriptable(EnemyReference.EnemyType enemyType)
        {
            return Singleton._enemyDictionary.Clone()[enemyType];
        }

        public static void InstantiateEnemy(EnemyReference.EnemyType enemyType, Vector3 pos)
        {
            EnemyScriptable enemyScriptable = GetEnemyScriptable(enemyType);

            var newEnemy = Instantiate(enemyScriptable.Prefab, pos, Quaternion.identity);
            newEnemy.GetComponent<Enemy>().EnemyType = enemyType;
            newEnemy.AddComponent<EntityObject>().entityType = EntityReference.Entity.Enemy;

            ElementManager.Singleton.InstantiateLocalCanvasInActor(newEnemy);
            
            Singleton._enemies.Add(new EnemyWithHealth(newEnemy, enemyType, ElementManager.GetHealthImg(newEnemy)));
        }
    }
