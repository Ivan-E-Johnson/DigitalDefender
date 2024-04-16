using UnityEngine;
using UnityEngine.Serialization;

namespace Enemys
{
    public class Enemy: MonoBehaviour
    {
        public float maxHealth = 100;
        public float currentHealth = 100;
        public float speed = 10;
        [FormerlySerializedAs("ID")] public int id;
    
        public void Initialize()
        {
            currentHealth = maxHealth;//
        
        }
        
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            throw new System.NotImplementedException();
        }
    }
}