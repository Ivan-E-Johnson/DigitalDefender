using Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemys
{
    public class Enemy : MonoBehaviour
    {
        public int nodeIndex;
        public float maxHealth = 100;
        public float currentHealth = 100;
        public float speed = 1;
        public bool isAlive = true;
        public int randVal;
        

        [FormerlySerializedAs("ID")] public int id;

        public void Initialize()
        {
            currentHealth = maxHealth; //
            // Get Spawn position for the map (Should be set by the map)
            transform.position = GameLoopManager.NodePositions[0];
            nodeIndex = 0;
            randVal = Random.Range(0, 100);
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0) Die();
        }

        private void Die()
        {
            throw new System.NotImplementedException();
        }
        
        // public void Move()
        // {
        //     if (nodeIndex < GameLoopManager.NodePositions.Length - 1)
        //     {
        //         var targetPosition = GameLoopManager.NodePositions[nodeIndex + 1];
        //         var moveDirection = targetPosition - transform.position;
        //         transform.position += moveDirection.normalized * speed * Time.deltaTime;
        //         if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        //         {
        //             nodeIndex++;
        //         }
        //     }
        //     else
        //     {
        //         // Reached the end of the path
        //         GameLoopManager.Instance.TakeDamage(1);
        //         Die();
        //     }
        // }
    }
}