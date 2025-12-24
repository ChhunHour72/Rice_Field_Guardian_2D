using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    [Header("Frog Stats")]
    public float attackRange = 1.2f;      // Close range for eating
    public float attackRate = 0.5f;       // Fast attacks (2 per second)
    public int biteDamage = 100;          // Instant kill damage
    
    [Header("Target Types")]
    public List<string> canEat = new List<string> { "Ant", "Grasshopper" };
    
    [Header("Detection Settings")]
    public float detectionRangeY = 0.3f;  // Strict same lane detection
    
    private float nextAttackTime = 0f;
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log("🐸 Frog ready! Can eat: " + string.Join(", ", canEat));
    }
    
    void Update()
    {
        DetectAndEat();
    }
    
    void DetectAndEat()
    {
        // Find enemies in range
        GameObject targetEnemy = FindClosestEdibleEnemy();
        
        // If we found food and cooldown is ready
        if(targetEnemy != null && Time.time >= nextAttackTime)
        {
            EatEnemy(targetEnemy);
            nextAttackTime = Time.time + (1f / attackRate);
        }
    }
    
    GameObject FindClosestEdibleEnemy()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        GameObject closestEnemy = null;
        float closestDistance = attackRange;
        
        foreach(GameObject enemy in allEnemies)
        {
            // Get enemy type
            Ant antScript = enemy.GetComponent<Ant>();
            if(antScript == null) continue;
            
            // Check if frog can eat this type
            if(!canEat.Contains(antScript.enemyType))
            {
                continue; // Skip enemies frog can't eat
            }
            
            // Check if in same lane (straight line)
            float yDifference = Mathf.Abs(enemy.transform.position.y - transform.position.y);
            
            if(yDifference <= detectionRangeY)
            {
                // Check distance (frog can eat enemies from both sides)
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
    
    void EatEnemy(GameObject enemy)
    {
        // Play attack/eat animation
        if(animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
        Ant antScript = enemy.GetComponent<Ant>();
        if(antScript != null)
        {
            // Instant kill - massive damage
            antScript.TakeDamage(biteDamage);
            
            Debug.Log("🐸 Frog ate " + antScript.enemyType + "! *CHOMP*");
        }
    }
    
    // Visualize detection range in Scene view
    void OnDrawGizmosSelected()
    {
        // Draw attack range circle
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Draw detection lane (Y range)
        Gizmos.color = Color.cyan;
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