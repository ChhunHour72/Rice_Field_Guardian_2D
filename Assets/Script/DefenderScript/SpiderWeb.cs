using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 8f;              // FASTER: Easier to see
    public int damage = 10;
    
    private Transform target;
    private Vector3 direction;
    private bool isInitialized = false;
    
    void Start()
    {
        // ADDED: Force visibility settings
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if(renderer != null)
        {
            renderer.sortingOrder = 10;
            renderer.color = Color.white;
            
            Debug.Log("🕸️ Web sprite: " + (renderer.sprite != null ? renderer.sprite.name : "MISSING!"));
        }
        else
        {
            Debug.LogError("⚠️ SpiderWeb has no SpriteRenderer!");
        }
        
        // Auto-destroy after 3 seconds
        Destroy(gameObject, 3f);
    }
    
    public void Initialize(Transform enemyTarget, int webDamage)
    {
        target = enemyTarget;
        damage = webDamage;
        
        // Calculate STRAIGHT direction (no Y movement for straight lane)
        if(target != null)
        {
            direction = (target.position - transform.position).normalized;
            
            // OPTIONAL: Force perfectly horizontal
            // direction = Vector3.right; // Uncomment for PERFECTLY straight
        }
        else
        {
            direction = Vector3.right; // Default right
        }
        
        // Rotate sprite to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
        isInitialized = true;
        
        Debug.Log("🕸️ Web initialized at " + transform.position + " flying " + direction);
    }
    
    void Update()
    {
        if(!isInitialized)
        {
            Debug.LogWarning("⚠️ Web not initialized!");
            return;
        }
        
        // Move the web in STRAIGHT line
        Vector3 movement = direction * speed * Time.deltaTime;
        transform.position += movement;
        
        // Debug: Show position every frame
        // Debug.Log("🕸️ Web position: " + transform.position);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("🔥 Web collided with: " + collision.gameObject.name + " | Tag: " + collision.tag);
        
        // Check if we hit an enemy
        if(collision.CompareTag("Enemy"))
        {
            Ant enemy = collision.GetComponent<Ant>();
            if(enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("💥 Web hit " + enemy.enemyType + " for " + damage + " damage!");
            }
            
            // Destroy the web after hitting
            Destroy(gameObject);
        }
    }
    
    // Visualize in Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}