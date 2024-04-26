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
        

        [FormerlySerializedAs("ID")] public int id;

        public void Initialize()
        {
            currentHealth = maxHealth; //
            // Get Spawn position for the map (Should be set by the map)
            transform.position = GameLoopManager.NodePositions[0];
            nodeIndex = 0;
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
        
    }
}