using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snail : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 0.008f;      // VERY SLOW
    
    [Header("Health")]
    public int maxHealth = 60;            // HIGH HP - tanky!
    private int currentHealth;
    
    [Header("Type")]
    public string enemyType = "Snail";
    
    [Header("Shell Defense")]
    public bool hasShell = true;
    public float shellDamageReduction = 0.5f;  // Takes 50% less damage
    
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        Debug.Log("🐌 " + enemyType + " spawned with " + maxHealth + " HP (TANKY!)");
    }
    
    private void FixedUpdate()
    {
        // Move left SLOWLY
        transform.position -= new Vector3(moveSpeed, 0, 0);
        
        // Check if reached the end
        if(transform.position.x < -10f)
        {
            ReachEnd();
        }
    }
    
    public void TakeDamage(int damage)
    {
        int actualDamage = damage;
        
        // Shell reduces damage
        if(hasShell)
        {
            actualDamage = Mathf.RoundToInt(damage * shellDamageReduction);
            Debug.Log("🛡️ Shell absorbed damage! " + damage + " → " + actualDamage);
        }
        
        currentHealth -= actualDamage;
        
        Debug.Log("❤️ " + enemyType + " took " + actualDamage + " damage. HP: " + currentHealth + "/" + maxHealth);
        
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
        
        Destroy(gameObject, 0.2f);
    }
    
    void ReachEnd()
    {
        Debug.Log("⚠️ " + enemyType + " reached the end!");
        
        // Damage rice field
        if(GameManager.instance != null)
        {
            // GameManager.instance.TakeDamage(15);  // More damage - tanky enemy
        }
        SceneManager.LoadScene("LooseScene_1");
        
        Destroy(gameObject);
    }
}