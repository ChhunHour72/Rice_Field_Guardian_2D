using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [Header("Spider Stats")]
    public float attackRange = 8f;        // INCREASED: Now shoots 8 units
    public float attackRate = 1f;         // 1 shot per second
    public int webDamage = 10;
    
    [Header("Projectile Setup")]
    public GameObject spiderWebPrefab;    // Drag SpiderWeb prefab here
    public Transform shootPoint;          // Where web spawns from
    
    [Header("Detection Settings")]
    public float detectionRangeY = 0.3f;  // STRAIGHT LANE: Very narrow Y detection
    
    private float nextAttackTime = 0f;
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        
        // If no shoot point, create one
        if(shootPoint == null)
        {
            GameObject shootObj = new GameObject("ShootPoint");
            shootObj.transform.parent = transform;
            shootObj.transform.localPosition = new Vector3(0.5f, 0f, 0f);
            shootPoint = shootObj.transform;
        }
        
        Debug.Log("🕷️ Spider ready! Range: " + attackRange + " units");
    }
    
    void Update()
    {
        DetectAndShoot();
    }
    
    void DetectAndShoot()
    {
        // Find the closest enemy in STRAIGHT line
        GameObject targetEnemy = FindClosestEnemyInStraightLine();
        
        // If we found an enemy and cooldown is ready
        if(targetEnemy != null && Time.time >= nextAttackTime)
        {
            ShootWeb(targetEnemy);
            nextAttackTime = Time.time + (1f / attackRate);
        }
    }
    
    GameObject FindClosestEnemyInStraightLine()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        GameObject closestEnemy = null;
        float closestDistance = attackRange;
        
        foreach(GameObject enemy in allEnemies)
        {
            // STRICT: Check if enemy is in EXACT same row (straight line)
            float yDifference = Mathf.Abs(enemy.transform.position.y - transform.position.y);
            
            if(yDifference <= detectionRangeY)
            {
                // Check if enemy is to the RIGHT of spider
                float xDistance = enemy.transform.position.x - transform.position.x;
                
                // Enemy must be to the right (positive X) and within range
                if(xDistance > 0 && xDistance <= attackRange)
                {
                    if(xDistance < closestDistance)
                    {
                        closestDistance = xDistance;
                        closestEnemy = enemy;
                    }
                }
            }
        }
        
        return closestEnemy;
    }
    
    void ShootWeb(GameObject target)
    {
        // Play attack animation if exists
        if(animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
        // Create the spider web projectile
        if(spiderWebPrefab != null)
        {
            Vector3 spawnPosition = shootPoint != null ? shootPoint.position : transform.position;
            
            // FIXED: Force spawn position to be visible
            spawnPosition.z = 0; // Make sure it's at Z = 0
            
            GameObject web = Instantiate(spiderWebPrefab, spawnPosition, Quaternion.identity);
            
            // ADDED: Make sure it's on the correct sorting layer
            SpriteRenderer webRenderer = web.GetComponent<SpriteRenderer>();
            if(webRenderer != null)
            {
                webRenderer.sortingOrder = 10; // High value to appear on top
                webRenderer.color = Color.white; // Ensure visible
            }
            
            // Set up the web script
            SpiderWeb webScript = web.GetComponent<SpiderWeb>();
            if(webScript != null)
            {
                webScript.Initialize(target.transform, webDamage);
            }
            
            Debug.Log("🕷️ Spider shot web at " + target.name + " | Distance: " + 
                      Vector3.Distance(transform.position, target.transform.position).ToString("F2"));
        }
        else
        {
            Debug.LogWarning("⚠️ Spider: spiderWebPrefab is not assigned!");
        }
    }
    
    // Visualize detection range in Scene view
    void OnDrawGizmosSelected()
    {
        // Draw attack range - STRAIGHT LINE
        Gizmos.color = Color.yellow;
        Vector3 start = transform.position;
        Vector3 end = transform.position + Vector3.right * attackRange;
        Gizmos.DrawLine(start, end);
        
        // Draw detection lane (Y range) - VERY NARROW for straight line
        Gizmos.color = Color.green;
        Vector3 topLeft = transform.position + new Vector3(0, detectionRangeY, 0);
        Vector3 topRight = transform.position + new Vector3(attackRange, detectionRangeY, 0);
        Vector3 bottomLeft = transform.position + new Vector3(0, -detectionRangeY, 0);
        Vector3 bottomRight = transform.position + new Vector3(attackRange, -detectionRangeY, 0);
        
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(topRight, bottomRight);
        
        // Draw center line
        Gizmos.color = Color.red;
        Gizmos.DrawLine(start, end);
    }
}