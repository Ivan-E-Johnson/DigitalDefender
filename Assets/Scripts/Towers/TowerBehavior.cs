using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    public float range = 30f;  // The range within which the tower detects enemies
    public float attackInterval = 1f;  // Time between attacks
    public int attackPower = 1;  // Power of each attack

    private Transform targetEnemy = null;  // Current target
    private float attackCounter = 0;  // Timer to track attack timing

    // void Update()
    // {
    //     if (targetEnemy == null || Vector3.Distance(transform.position, targetEnemy.position) > range)
    //     {
    //         FindNewTarget();
    //     }
    //
    //     if (targetEnemy != null)
    //     {
    //         Attack();
    //     }
    // }

    // void FindNewTarget()
    // {
    //     // Reset target
    //     targetEnemy = null;
    //
    //     // Find the closest enemy within range
    //     float shortestDistance = Mathf.Infinity;
    //     GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //
    //     foreach (GameObject enemy in enemies)
    //     {
    //         
    //         float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
    //         if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
    //         {
    //             shortestDistance = distanceToEnemy;
    //             targetEnemy = enemy.transform;
    //         }
    //     }
    // }

    // void Attack()
    // {
    //     if (attackCounter <= 0f)
    //     {
    //         // Implement attack logic here (e.g., instantiate bullets or damage enemy)
    //         Debug.Log("Attacking " + targetEnemy.name + " with power " + attackPower);
    //         // Reset the attack counter
    //         attackCounter = attackInterval;
    //     }
    //     else
    //     {
    //         attackCounter -= Time.deltaTime;
    //     }
    // }
}