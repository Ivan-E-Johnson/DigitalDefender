using UnityEngine;
using UnityEngine.Serialization;

namespace Enemys
{
    public class Enemy : MonoBehaviour
    {
        public float maxHealth = 100;
        public float currentHealth = 100;
        public float speed = 10;
        public Vector3Int position;
        public bool isAlive = true;


        [FormerlySerializedAs("ID")] public int id;

        public void Initialize()
        {
            currentHealth = maxHealth; //
            // Get Spawn position for the map (Should be set by the map)
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

        public void MoveTowardPosition(Vector3Int targetPosition)
        {
            var moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
        }
    }
}