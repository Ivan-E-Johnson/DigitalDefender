using System.Collections;
using System.Collections.Generic;
using Enemys;
using UnityEngine;

namespace Game
{
    public class GameLoopManager : MonoBehaviour
    {
        public bool loopShouldEnd;

        private static Queue<int> _enemyIDsToSpawn;
        private static Queue<int> _towerIDsToSpawn;
        
        private void Start()
        {
            _enemyIDsToSpawn = new Queue<int>();
            EntitySummoner.Initialize();
            StartCoroutine(GameLoop());
            InvokeRepeating(nameof(TestSummonEnemy), 0f, 1f);
            InvokeRepeating(nameof(TestRemoveEnemy), 0f, 1f);
        }
        void TestSummonEnemy()
        {
            _enemyIDsToSpawn.Enqueue(1); // Match the ID of the enemy you want to spawn
            // Should this be an enemy with an id in the resources folder?
            
        }
        void TestRemoveEnemy()
        {
            // Remove an enemy from the game
            if (EntitySummoner.EnemiesInGame.Count > 1)
            {
                EntitySummoner.EnemiesInGame.RemoveAt(1); // Remove the enemy at index 1
            }
        }
        
        IEnumerator GameLoop()
        {
            while (!loopShouldEnd)
            {
                // Game logic goes here
                
                //Spawn Enemies
                if (_enemyIDsToSpawn.Count > 0)
                {
                    Enemy enemy = EntitySummoner.SummonEnemy(_enemyIDsToSpawn.Dequeue());
                    enemy.transform.position = new Vector3Int(0, 0, 0); //Make summon enemies in available spawn points 
                    // EntitySummoner.EnemiesInGame.Add(enemy); // Do we need this?
                }
                
                
                //Spawn Towers
                
                //Move Enemies
                
                //Tick Towers
                
                //Apply Effects
                
                //Damage Enemies
                
                //Remove Enemies
                
                //Remove Towers
                
                
                
                
                
                
                yield return null;
            }
        }
        
    }

}