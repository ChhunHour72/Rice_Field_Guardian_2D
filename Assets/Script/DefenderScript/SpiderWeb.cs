using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 8f;
    public int damage = 10;
    
    private Transform target;
    private Vector3 direction;
    private bool isInitialized = false;
    private Rigidbody2D rb;
    
    void Start()
    {
        // Add Rigidbody2D if not present
        rb = GetComponent<Rigidbody2D>();
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        
        // Configure Rigidbody2D for projectile
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        
        // Make sure we have a collider
        Collider2D col = GetComponent<Collider2D>();
        if(col == null)
        {
            CircleCollider2D circleCol = gameObject.AddComponent<CircleCollider2D>();
            circleCol.radius = 0.3f;
            circleCol.isTrigger = true;
            Debug.Log("✅ Added Circle Collider to web");
        }
        else
        {
            col.isTrigger = true;
            Debug.Log("✅ Web collider is trigger: " + col.isTrigger);
        }
        
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
        
        Destroy(gameObject, 3f);
    }
    
    public void Initialize(Transform enemyTarget, int webDamage)
    {
        target = enemyTarget;
        damage = webDamage;
        
        if(target != null)
        {
            direction = (target.position - transform.position).normalized;
        }
        else
        {
            direction = Vector3.right;
        }
        
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
        
        Vector3 movement = direction * speed * Time.deltaTime;
        transform.position += movement;
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("🔥 Web collided with: " + collision.gameObject.name + " | Tag: " + collision.tag);
        
        // Check if we hit an enemy
        if(collision.CompareTag("Enemy"))
        {
            // Try to find ANY enemy script and call TakeDamage
            bool hitEnemy = false;
            
            // Try Ant
            Ant ant = collision.GetComponent<Ant>();
            if(ant != null)
            {
                Debug.Log("✅ Found Ant script!");
                ant.TakeDamage(damage);
                Debug.Log("💥 Web hit Ant for " + damage + " damage!");
                hitEnemy = true;
            }
            
            // Try Grasshopper
            Grasshopper grasshopper = collision.GetComponent<Grasshopper>();
            if(grasshopper != null)
            {
                Debug.Log("✅ Found Grasshopper script!");
                grasshopper.TakeDamage(damage);
                Debug.Log("💥 Web hit Grasshopper for " + damage + " damage!");
                hitEnemy = true;
            }
            
            // Try Snail
            Snail snail = collision.GetComponent<Snail>();
            if(snail != null)
            {
                Debug.Log("✅ Found Snail script!");
                snail.TakeDamage(damage);
                Debug.Log("💥 Web hit Snail for " + damage + " damage!");
                hitEnemy = true;
            }
            
            if(!hitEnemy)
            {
                Debug.LogWarning("⚠️ Hit enemy but no damage script found!");
                
                // List all components on the enemy
                MonoBehaviour[] components = collision.GetComponents<MonoBehaviour>();
                string componentNames = "";
                foreach(var comp in components)
                {
                    componentNames += comp.GetType().Name + ", ";
                }
                Debug.LogWarning("⚠️ Enemy components: " + componentNames);
            }
            
            // Destroy the web after hitting
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("⚠️ Collision but tag is: " + collision.tag + " (expected 'Enemy')");
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}