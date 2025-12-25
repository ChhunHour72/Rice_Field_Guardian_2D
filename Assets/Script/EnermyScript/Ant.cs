using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ant : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 0.004f;
    
    [Header("Health")]
    public int maxHealth = 30;
    private int currentHealth;
    
    [Header("Type")]
    public string enemyType = "Ant"; // "Ant", "Grasshopper", "Snail"
    
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        Debug.Log("🐜 " + enemyType + " spawned with " + maxHealth + " HP");
    }
    
    private void FixedUpdate()
    {
        // Move left
        transform.position -= new Vector3(speed, 0, 0);
        
        // Check if reached the end (adjust -10f based on your scene)
        if(transform.position.x < -10f)
        {
            ReachEnd();
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        Debug.Log("❤️ " + enemyType + " took " + damage + " damage. HP: " + currentHealth + "/" + maxHealth);
        
        // Flash red when hit
        StartCoroutine(FlashRed());
        
        if(currentHealth <= 0)
        {
            Die();
        }
    }
    
    IEnumerator FlashRed()
    {
        if(spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }
    
    void Die()
    {
        Debug.Log("💀 " + enemyType + " died!");
        
        // Play death animation if exists
        if(animator != null)
        {
            animator.SetTrigger("Die");
        }
        
        // Destroy after small delay
        Destroy(gameObject, 0.2f);
    }
    
    void ReachEnd()
    {
        
        
        // Damage rice field (implement later)
        if(GameManager.instance != null)
        {
            // GameManager.instance.TakeDamage(10);
        }

        Destroy(gameObject);
        SceneManager.LoadScene("LooseScene_1");
        
    }
    
}