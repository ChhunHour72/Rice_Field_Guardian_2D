using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    [Header("Frog Stats")]
    public float attackRange = 1.2f;      // Close range for eating
    public int biteDamage = 100;          // Instant kill damage
    
    [Header("Digestion System")]
    public float digestionTime = 3f;      // How long to digest (seconds)
    private bool isDigesting = false;     // Is frog currently digesting?
    private float digestionEndTime = 0f;
    
    [Header("Target Types")]
    public List<string> canEat = new List<string> { "Ant", "Grasshopper" };
    
    [Header("Detection Settings")]
    public float detectionRangeY = 0.3f;  // Strict same lane detection
    
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log("🐸 Frog ready! Can eat: " + string.Join(", ", canEat) + " | Digestion time: " + digestionTime + "s");
    }
    
    void Update()
    {
        // Check if digestion is complete
        if(isDigesting)
        {
            if(Time.time >= digestionEndTime)
            {
                FinishDigestion();
            }
            else
            {
                // Still digesting - can't attack
                return;
            }
        }
        
        // Look for food
        DetectAndEat();
    }
    
    void DetectAndEat()
    {
        // Don't look for food while digesting
        if(isDigesting) return;
        
        // Find enemies in range
        GameObject targetEnemy = FindClosestEdibleEnemy();
        
        // If we found food, eat it!
        if(targetEnemy != null)
        {
            EatEnemy(targetEnemy);
        }
    }
    
    GameObject FindClosestEdibleEnemy()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        GameObject closestEnemy = null;
        float closestDistance = attackRange;
        
        foreach(GameObject enemy in allEnemies)
        {
            // Get enemy type - check for any enemy script
            string enemyTypeStr = "";
            
            Ant ant = enemy.GetComponent<Ant>();
            if(ant != null) enemyTypeStr = ant.enemyType;
            
            Grasshopper grasshopper = enemy.GetComponent<Grasshopper>();
            if(grasshopper != null) enemyTypeStr = grasshopper.enemyType;
            
            Snail snail = enemy.GetComponent<Snail>();
            if(snail != null) enemyTypeStr = snail.enemyType;
            
            if(string.IsNullOrEmpty(enemyTypeStr)) continue;
            
            // Check if frog can eat this type
            if(!canEat.Contains(enemyTypeStr))
            {
                continue;
            }
            
            float yDifference = Mathf.Abs(enemy.transform.position.y - transform.position.y);
            
            if(yDifference <= detectionRangeY)
            {
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
        if(animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
        // Try each enemy type
        Ant ant = enemy.GetComponent<Ant>();
        if(ant != null)
        {
            ant.TakeDamage(biteDamage);
            Debug.Log("🐸 Frog ate " + ant.enemyType + "! *CHOMP*");
            return;
        }
        
        Grasshopper grasshopper = enemy.GetComponent<Grasshopper>();
        if(grasshopper != null)
        {
            grasshopper.TakeDamage(biteDamage);
            Debug.Log("🐸 Frog ate " + grasshopper.enemyType + "! *CHOMP*");
            return;
        }
        
        Snail snail = enemy.GetComponent<Snail>();
        if(snail != null)
        {
            snail.TakeDamage(biteDamage);
            Debug.Log("🐸 Frog ate " + snail.enemyType + "! *CHOMP*");
            return;
        }
    }
    
    void StartDigestion()
    {
        isDigesting = true;
        digestionEndTime = Time.time + digestionTime;
        
        // Visual feedback: Change color during digestion
        if(spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.7f, 1f, 0.7f); // Light green tint
        }
        
        Debug.Log("😴 Frog is digesting... (will be ready at " + digestionEndTime + ")");
    }
    
    void FinishDigestion()
    {
        isDigesting = false;
        
        // Return to normal color
        if(spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }

            if(animator != null)
        {
        animator.SetTrigger("Burp");
        }
        
        Debug.Log("✅ Frog finished digesting! Ready to eat again!");
    }
    
    // Visualize detection range in Scene view
    void OnDrawGizmosSelected()
    {
        // Draw attack range circle
        Gizmos.color = isDigesting ? Color.gray : Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Draw detection lane (Y range)
        Gizmos.color = isDigesting ? Color.gray : Color.cyan;
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
        Gizmos.color = isDigesting ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
        
        // Show digestion status text in Scene view
        if(isDigesting)
        {
            UnityEditor.Handles.Label(transform.position + Vector3.up, 
                "DIGESTING: " + (digestionEndTime - Time.time).ToString("F1") + "s");
        }
    }
}