using UnityEngine;

namespace Enemys
{
    [CreateAssetMenu(fileName = "New EnemySummonData", menuName = "EnemySummoner")]
    public class EnemySummonData : ScriptableObject
    {
        public GameObject enemyPrefab;
        public int enemyID;
    
        // public int enemyCount;
        // public float spawnInterval;
        // public float spawnRadius;
        // public float spawnDelay;
    }
}