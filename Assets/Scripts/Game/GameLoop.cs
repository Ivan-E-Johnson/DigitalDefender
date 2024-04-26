using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemys;
using Maps;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace Game
{
    public class GameLoopManager : MonoBehaviour
    {
        public bool loopShouldEnd;
        public Transform NodeParent;
        
        
        public static Vector3[] NodePositions;
        public static Vector3 StartPositionVector3;
        public static Vector3 EndPostitionVector3;

        private static Queue<Enemy> _enemiesToRemove;
        private static Queue<int> _enemysToSpawn;
        private static Queue<int> _towerIDsToSpawn;

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
            
            // Set up NodePositions
            NodePositions = mapGenerator.GetNodePositions();
            Debug.Log($"NodePositions: {NodePositions.Length}\n" +
                      $"NodePositions[0]: {NodePositions[0]}\n" );

            // NodePositions = new Vector3[NodeParent.childCount];
            // for (var i = 0; i < NodeParent.childCount; i++)
            // {
            //     NodePositions[i] = NodeParent.GetChild(i).position; // Can we use MapCell instead of Vector3? What are performance implications?
            // }
        }

        private void Start()
        {
            // Set up EntitySummoner
            _enemiesToRemove = new Queue<Enemy>();
            _enemysToSpawn = new Queue<int>();
            EntitySummoner.Initialize();
            StartCoroutine(GameLoop());

            // spawn 1 enemy
            _enemysToSpawn.Enqueue(1);
           
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            { 
                loopShouldEnd = true;
            }
        }

        
        public static void EnqueueEnemyToSpawns(int enemyID)
        {
            _enemysToSpawn.Enqueue(enemyID);
        }
        
        public static void EnqueueEnemyToRemove(Enemy enemy)
        {
            _enemiesToRemove.Enqueue(enemy);
        }
        
        
        
        private IEnumerator GameLoop()
        {
            while (!loopShouldEnd)
            {
                // Game logic goes here

                //Spawn Enemies
                if (_enemysToSpawn.Count > 0)
                {
                    EntitySummoner.SummonEnemy(_enemysToSpawn.Dequeue());
                    // EntitySummoner.EnemiesInGame.Add(enemy); // Do we need this?
                }
                

                //Spawn Towers

                //Move Enemies

                NativeArray<Vector3>  NodesToUse = new NativeArray<Vector3>(NodePositions.Length, Allocator.TempJob);
                NativeArray<float> EnemySpeeds = new NativeArray<float>(EntitySummoner.EnemiesInGame.Count, Allocator.TempJob);
                NativeArray<int> NodeIndexes = new NativeArray<int>(EntitySummoner.EnemiesInGame.Count, Allocator.TempJob);
                TransformAccessArray EnemyAcess = new TransformAccessArray(EntitySummoner.EnemyInGameTransforms.ToArray(), 2); // Why 2
                
                for (int i = 0; i < NodePositions.Length; i++)
                {
                    NodesToUse[i] = NodePositions[i];
                    Debug.Log($"NodePositions[{i}]: {NodePositions[i]}");
                    
                }
                for ( int i = 0; i < EntitySummoner.EnemiesInGame.Count; i++)
                {
                    EnemySpeeds[i] = EntitySummoner.EnemiesInGame[i].speed;
                    NodeIndexes[i] = EntitySummoner.EnemiesInGame[i].nodeIndex;
                }
                
                MoveEnemyJob moveEnemyJob = new MoveEnemyJob
                {
                    NodePositions = NodesToUse,
                    EnemySpeeds = EnemySpeeds,
                    NodeIndexes = NodeIndexes,
                    DeltaTime = Time.deltaTime // Cant use Time.deltaTime in a job
                };
                JobHandle moveJobHandle = moveEnemyJob.Schedule(EnemyAcess);
                moveJobHandle.Complete();
                Debug.Log($"Number of Enemies: {EntitySummoner.EnemiesInGame.Count}");
                
                for( int i = 0; i < EntitySummoner.EnemiesInGame.Count; i++)
                {
                    
                    
                    EntitySummoner.EnemiesInGame[i].nodeIndex = NodeIndexes[i];
                    
                    if (NodeIndexes[i] >= NodePositions.Length)
                    {
                        EnqueueEnemyToRemove(EntitySummoner.EnemiesInGame[i]); // Remove the enemy if it has reached the end
                    }
                }
                
                
                
                NodesToUse.Dispose();
                EnemySpeeds.Dispose();
                NodeIndexes.Dispose();

                //Tick Towers

                //Apply Effects

                //Damage Enemies

                //Remove Enemies
                if (_enemiesToRemove.Count > 0)
                {
                    EntitySummoner.RemoveEnemy(_enemiesToRemove.Dequeue());
                    // EntitySummoner.EnemiesInGame.Add(enemy); // Do we need this?
                }

                //Remove Towers


                yield return null;
            }
        }
    }
    public struct MoveEnemyJob : IJobParallelForTransform
    {
        // Todo Try this with a NativeArray of enemies
        [NativeDisableParallelForRestriction]
        public NativeArray<Vector3> NodePositions;
        [NativeDisableParallelForRestriction]
        public NativeArray<float> EnemySpeeds;
        [NativeDisableParallelForRestriction]
        public NativeArray<int> NodeIndexes;
        
        public float DeltaTime;
        public void Execute(int index, TransformAccess transform)
        {


            if (NodeIndexes[index] < NodePositions.Length)
            {
                var moveDirection = (NodePositions[NodeIndexes[index]] - transform.position).normalized;
                Debug.Log($"Current Position: {transform.position}");
                Debug.Log($"Next Position: {NodePositions[NodeIndexes[index]]}");
                
                transform.position += moveDirection * EnemySpeeds[index] * DeltaTime;
                // transform.position = Vector3.MoveTowards(transform.position, NodePositions[NodeIndexes[index]], EnemySpeeds[index] * DeltaTime);
                // Debug.Log($"We Are close enough {Vector3.Distance(transform.position, NodePositions[NodeIndexes[index]]) < 0.1f} ");
                if (transform.position == NodePositions[NodeIndexes[index]])
                {
                    Debug.Log("We are at the node");
                    // Debug.Log($"NodeIndexes[{index}]: {NodeIndexes[index]}");
                    NodeIndexes[index] += 1;
                    // Debug.Log($"NodeIndexes[{index}]: {NodeIndexes[index]}");
                }
                
            }

        }
    }
}