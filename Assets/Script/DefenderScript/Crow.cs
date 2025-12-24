using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour
{
    [Header("Crow Stats")]
    public float attackRange = 1.2f;
    public float attackRate = 0.7f;
    public int peckDamage = 20;
    
    [Header("Detection")]
    public float detectionRangeY = 0.5f;
    
    private float nextAttackTime;
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        DetectAndPeck();
    }
    
    void DetectAndPeck()
    {
        // Find all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        foreach(GameObject enemyObj in enemies)
        {
            // Check if in same row
            float yDifference = Mathf.Abs(enemyObj.transform.position.y - transform.position.y);
            
            if(yDifference <= detectionRangeY)
            {
                // Check distance
                float distance = Vector3.Distance(transform.position, enemyObj.transform.position);
                
                if(distance <= attackRange && Time.time >= nextAttackTime)
                {
                    PeckEnemy(enemyObj);
                    nextAttackTime = Time.time + 1f / attackRate;
                    break; // Attack one at a time
                }
            }
        }
    }
    
    void PeckEnemy(GameObject enemyObj)
    {
        if(animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
        Ant enemy = enemyObj.GetComponent<Ant>();
        if(enemy != null)
        {
            enemy.TakeDamage(peckDamage);
            Debug.Log("Crow pecked " + enemy.enemyType + "!");
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Show detection row
        Gizmos.color = Color.magenta;
        Vector3 topPos = transform.position + Vector3.up * detectionRangeY;
        Vector3 bottomPos = transform.position - Vector3.up * detectionRangeY;
        Gizmos.DrawLine(topPos + Vector3.right * attackRange, topPos + Vector3.left * attackRange);
        Gizmos.DrawLine(bottomPos + Vector3.right * attackRange, bottomPos + Vector3.left * attackRange);
    }
}