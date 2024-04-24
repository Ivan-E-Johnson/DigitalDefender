using System;
using System.Collections;
using System.Collections.Generic;
using Enemys;
using Maps;
using UnityEngine;

namespace Game
{
    public class GameLoopManager : MonoBehaviour
    {
        public bool loopShouldEnd;

        private static Queue<int> _enemyIDsToSpawn;
        private static Queue<int> _towerIDsToSpawn;
        public static Vector3 StartPositionVector3;
        public static Vector3 EndPostitionVector3;

        
        private void Awake()
        {
            loopShouldEnd = false;
                        // Set up Map 
            var mapGenerator = MapGenerator.Instance;
            mapGenerator.GenerateNewMap();

            
            Debug.Log(mapGenerator.StartPosition);
            Debug.Log(mapGenerator.EndPosition);
            if (mapGenerator.StartPosition != null) StartPositionVector3 = mapGenerator.StartPosition.Position;
            if (mapGenerator.EndPosition != null) EndPostitionVector3 = mapGenerator.EndPosition.Position;

        }
        private void Start()
        {
            // Set up EntitySummoner
            _enemyIDsToSpawn = new Queue<int>();
            EntitySummoner.Initialize();
            StartCoroutine(GameLoop());
            InvokeRepeating(nameof(TestSummonEnemy), 0f, 1f);
            InvokeRepeating(nameof(TestRemoveEnemy), 0f, 3f);
        }



        private void TestSummonEnemy()
        {
            _enemyIDsToSpawn.Enqueue(1); // Match the ID of the enemy you want to spawn
            // Should this be an enemy with an id in the resources folder?
        }

        private void TestRemoveEnemy()
        {
            // Remove an enemy from the game
            if (EntitySummoner.EnemiesInGame.Count > 1)
            {
                var enemy_to_remove = EntitySummoner.EnemiesInGame[EntitySummoner.EnemiesInGame.Count - 1];
                enemy_to_remove.gameObject.SetActive(false);
                EntitySummoner.RemoveEnemy(enemy_to_remove);
            }
               
        }

        private IEnumerator GameLoop()
        {
            while (!loopShouldEnd)
            {
                // Game logic goes here

                //Spawn Enemies
                if (_enemyIDsToSpawn.Count > 0)
                {
                    var enemy = EntitySummoner.SummonEnemy(_enemyIDsToSpawn.Dequeue(), EndPostitionVector3);
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