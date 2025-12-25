using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour
{
    [Header("Crow Stats")]
    public float attackRange = 1.5f;      // Slightly longer than frog
    public float attackRate = 0.7f;       // ~1.4 attacks per second
    public int peckDamage = 20;           // Medium damage (not instant kill)
    
    [Header("Detection Settings")]
    public float detectionRangeY = 0.3f;  // Strict same lane detection
    
    private float nextAttackTime = 0f;
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log("🐦 Crow ready! Can attack ANY insect | Range: " + attackRange + " | Damage: " + peckDamage);
    }
    
    void Update()
    {
        DetectAndPeck();
    }
    
    void DetectAndPeck()
    {
        // Find any enemy in range
        GameObject targetEnemy = FindClosestEnemy();
        
        // If we found an enemy and cooldown is ready
        if(targetEnemy != null && Time.time >= nextAttackTime)
        {
            PeckEnemy(targetEnemy);
            nextAttackTime = Time.time + (1f / attackRate);
        }
    }
    
    GameObject FindClosestEnemy()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        GameObject closestEnemy = null;
        float closestDistance = attackRange;
        
        foreach(GameObject enemy in allEnemies)
        {
            // Check if in same lane (straight line)
            float yDifference = Mathf.Abs(enemy.transform.position.y - transform.position.y);
            
            if(yDifference <= detectionRangeY)
            {
                // Check distance (crow can attack from both sides)
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                
                if(distance <= attackRange)
                {
                    if(distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = enemy;
                    }
                }
            }
        }
        
        return closestEnemy;
    }
    
    void PeckEnemy(GameObject enemy)
    {
        if(animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
        // Try each enemy type
        Ant ant = enemy.GetComponent<Ant>();
        if(ant != null)
        {
            ant.TakeDamage(peckDamage);
            Debug.Log("🐦 Crow pecked " + ant.enemyType + " for " + peckDamage + " damage!");
            return;
        }
        
        Grasshopper grasshopper = enemy.GetComponent<Grasshopper>();
        if(grasshopper != null)
        {
            grasshopper.TakeDamage(peckDamage);
            Debug.Log("🐦 Crow pecked " + grasshopper.enemyType + " for " + peckDamage + " damage!");
            return;
        }
        
        Snail snail = enemy.GetComponent<Snail>();
        if(snail != null)
        {
            snail.TakeDamage(peckDamage);
            Debug.Log("🐦 Crow pecked " + snail.enemyType + " for " + peckDamage + " damage!");
            return;
        }
    }
    
    // Visualize detection range in Scene view
    void OnDrawGizmosSelected()
    {
        // Draw attack range circle
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Draw detection lane (Y range)
        Gizmos.color = Color.magenta;
        Vector3 center = transform.position;
        
        // Draw the detection box
        Vector3 topLeft = center + new Vector3(-attackRange, detectionRangeY, 0);
        Vector3 topRight = center + new Vector3(attackRange, detectionRangeY, 0);
        Vector3 bottomLeft = center + new Vector3(-attackRange, -detectionRangeY, 0);
        Vector3 bottomRight = center + new Vector3(attackRange, -detectionRangeY, 0);
        
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(topRight, bottomRight);
        
        // Draw center indicator
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}