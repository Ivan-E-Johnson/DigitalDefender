using System.Collections.Generic;
using Maps;
using UnityEngine;

namespace Enemys
{
    public class EntitySummoner : MonoBehaviour
    {
        public static List<Enemy> EnemiesInGame;
        public static Dictionary<int, GameObject> EnemyPrefabs;
        public static Dictionary<int, Queue<Enemy>> EnemyObjectPools;
        private static bool _isInitialized;


        // Start is called before the first frame update
        public static void Initialize()
        {
            if (!_isInitialized)
            {
                EnemyPrefabs = new Dictionary<int, GameObject>();
                EnemyObjectPools = new Dictionary<int, Queue<Enemy>>();
                EnemiesInGame = new List<Enemy>(); // list of Enemies alive currently

                // Note: path give must match path to the scriptable object IE "Path/to/Ememies"
                var enemies = Resources.LoadAll<EnemySummonData>($"Enemies");
                _isInitialized = true;

                // Test to see if the scriptable object was loaded
                // Debug.Log(Enemies.name);

                //Create distinct object pools for each enemy type
                foreach (var enemy in enemies)
                {
                    EnemyPrefabs.Add(enemy.enemyID, enemy.enemyPrefab);
                    EnemyObjectPools.Add(enemy.enemyID, new Queue<Enemy>());
                }
            }
            else
            {
                Debug.Log("EntitySummoner has already been initialized!!!!");
                Debug.Log("EntitySummoner has already been initialized!!!!");
                Debug.Log("EntitySummoner has already been initialized!!!!");
                Debug.Log("EntitySummoner has already been initialized!!!!");
            }
        }

        public static Enemy SummonEnemy(int enemyID,Vector3 spawnLocation)
        {
            
            Enemy summonedEnemy = null;

            if (EnemyPrefabs.ContainsKey(enemyID))
            {
                var enemyObjectPool = EnemyObjectPools[enemyID];

                if (enemyObjectPool.Count > 0)
                {
                    // Debug.Log("Summoning enemy from object pool queue");
                    summonedEnemy = enemyObjectPool.Dequeue();
                    summonedEnemy.Initialize();
                }
                else
                {
                    var enemyPrefab = EnemyPrefabs[enemyID];
                    Debug.Log($"Summoning new enemy with ID {enemyID} at {spawnLocation}");
                    var newEnemyObject = Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
                    summonedEnemy = newEnemyObject.GetComponent<Enemy>();
                    summonedEnemy.Initialize();
                }

                summonedEnemy.id = enemyID;
                EnemiesInGame.Add(summonedEnemy);
            }
            else
            {
                Debug.Log($"Enemy with ID {enemyID} does not exist");
            }

            return summonedEnemy;
        }

        public static void RemoveEnemy(Enemy enemyToRemove)
        {
            EnemyObjectPools[enemyToRemove.id].Enqueue(enemyToRemove);
            enemyToRemove.gameObject.SetActive(false);
            EnemiesInGame.Remove(enemyToRemove);
        }
    }
}