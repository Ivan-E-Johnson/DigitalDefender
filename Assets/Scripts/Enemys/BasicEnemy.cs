using Game;
using UnityEngine;
using UnityEngine.Serialization;
using Player;
using Unity.Mathematics;

namespace Enemys
{
    public enum EnemyState
    {
        Moving,
        Attacking,
        dying
    }
    public class Enemy : MonoBehaviour
    {
        public int nodeIndex;
        public EnemyState state;
        public float maxHealth = 100;
        public float currentHealth = 100;
        public float speed = 1;
        
        [SerializeField] private int reward = 1;
        private static readonly Player.Player CurrentPlayer = Player.Player.Instance;

        private static readonly int TotalNumberNodes = GameLoopManager.NodePositions.Length; // is it better to do it this way rather than make hundreds of calls to GameLoopManager.NodePositions.Length?
        

        

        public int id;

        public void Initialize()
        {
            state = EnemyState.Moving; 
            currentHealth = maxHealth; 
            // Get Spawn position for the map (Should be set by the map)
            transform.position = GameLoopManager.NodePositions[0];
            nodeIndex = 0;
            
        }
        
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0) 
                Die();
        }
        

        private void Die()
        {
            state = EnemyState.dying;
            // Add to score
            CurrentPlayer.getReward(reward);  

            // Remove from list of enemies
            EntitySummoner.EnemiesInGame.Remove(this);
            // Remove from list of transforms
            EntitySummoner.EnemyInGameTransforms.Remove(transform);
            // Return to object pool
            gameObject.SetActive(false);
        }
        
        private void Update()
        {
            if (state == EnemyState.Moving)
            {
                Move();
            }
            
            if (state == EnemyState.Attacking)
            {
                // Attack();
                Debug.Log("Attacking");
            }
            
            if (state == EnemyState.dying)
            {
                Die();
            }
            
        }

        private void Move()
        {
            
            if (nodeIndex < TotalNumberNodes)
            {
                var targetPosition = GameLoopManager.NodePositions[nodeIndex];
                var movementThisFrame = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementThisFrame);
                if (math.abs(Vector3.Distance(transform.position, targetPosition)) < 0.1f)
                {
                    nodeIndex++;
                }
            }
            else
            {
                // Damage Level Health 
                
                // Reached the end of the path
                Die();
            }
        }
    }
}