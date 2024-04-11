using UnityEngine;
using UnityEngine.Serialization;

public class Enemy: MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth = 100;
    public float speed = 10;
    public int ID;
    
    public void Initialize()
    {
        currentHealth = maxHealth;
        
    }

}