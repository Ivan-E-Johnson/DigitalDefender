using UnityEngine;
using System.Collections.Generic;

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
      EnemiesInGame = new List<Enemy>();

      // Note: path give must match path to the scriptable object IE "Path/to/Ememies"
      EnemySummonData[] Enemies = Resources.LoadAll<EnemySummonData>("Ememies");
      _isInitialized = true;

      // Test to see if the scriptable object was loaded
      // Debug.Log(Enemies.name);

      //Create distinct object pools for each enemy type
      foreach (EnemySummonData enemy in Enemies)
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

  public static Enemy SummonEnemy(int enemyID)
  {
    Enemy SummonedEnemy = null;
    
    if(EnemyPrefabs.ContainsKey(enemyID))
    {
      Queue<Enemy> enemyObjectPool = EnemyObjectPools[enemyID];
      
      if(enemyObjectPool.Count > 0)
      {
        // 
        SummonedEnemy = enemyObjectPool.Dequeue();
        SummonedEnemy.Initialize();
      }
      else
      {
        GameObject enemyPrefab = EnemyPrefabs[enemyID];
        GameObject newEnemyObject = Instantiate(enemyPrefab, Vector3Int.zero, Quaternion.identity);
        SummonedEnemy = newEnemyObject.GetComponent<Enemy>();
        SummonedEnemy.Initialize();
        
      }
      SummonedEnemy.ID = enemyID;
      EnemiesInGame.Add(SummonedEnemy);
    }
    else
    {
      Debug.Log($"Enemy with ID {enemyID} does not exist");
    }

    return SummonedEnemy;
  }
  public static void RemoveEnemy(Enemy enemyToRemove)
  {
    EnemyObjectPools[enemyToRemove.ID].Enqueue(enemyToRemove);
    enemyToRemove.gameObject.SetActive(false);
    EnemiesInGame.Remove(enemyToRemove);
  }

}
