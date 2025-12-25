using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grasshopper : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 0.03f;       // Normal movement speed
    public float jumpSpeed = 0.08f;       // Speed during jump (faster)
    public float jumpDistance = 1.5f;     // How far to jump forward
    public float jumpInterval = 2f;       // Jump every 2 seconds
    
    [Header("Health")]
    public int maxHealth = 20;
    private int currentHealth;
    
    [Header("Type")]
    public string enemyType = "Grasshopper";
    
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float nextJumpTime;
    private bool isJumping = false;
    private Vector3 jumpStartPos;
    private Vector3 jumpTargetPos;
    private float jumpProgress = 0f;
    
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Set next jump time
        nextJumpTime = Time.time + Random.Range(0.5f, jumpInterval);
        
        Debug.Log("🦗 " + enemyType + " spawned with " + maxHealth + " HP");
    }
    
    private void Update()
    {
        if(isJumping)
        {
            // Perform jump movement
            PerformJump();
        }
        else
        {
            // Normal walking movement
            transform.position -= new Vector3(moveSpeed, 0, 0);
            
            // Check if time to jump
            if(Time.time >= nextJumpTime)
            {
                StartJump();
            }
        }
        
        // Check if reached the end
        if(transform.position.x < -10f)
        {
            ReachEnd();
        }
    }
    
    void StartJump()
    {
        isJumping = true;
        jumpProgress = 0f;
        jumpStartPos = transform.position;
        
        // Calculate jump target (forward on the X axis)
        jumpTargetPos = jumpStartPos + new Vector3(-jumpDistance, 0, 0);
        
        // Play jump animation if exists
        if(animator != null)
        {
            animator.SetTrigger("Jump");
        }
        
        Debug.Log("🦗 Grasshopper jumping from " + jumpStartPos.x + " to " + jumpTargetPos.x);
    }
    
    void PerformJump()
    {
        // Increment jump progress
        jumpProgress += jumpSpeed * Time.deltaTime;
        
        if(jumpProgress >= 1f)
        {
            // Jump complete
            transform.position = jumpTargetPos;
            isJumping = false;
            nextJumpTime = Time.time + jumpInterval;
            
            Debug.Log("🦗 Grasshopper landed!");
        }
        else
        {
            // Move along arc (parabola for visual effect)
            float xPos = Mathf.Lerp(jumpStartPos.x, jumpTargetPos.x, jumpProgress);
            
            // Create arc effect: Y goes up then down
            float arc = Mathf.Sin(jumpProgress * Mathf.PI) * 0.5f; // Arc height
            float yPos = jumpStartPos.y + arc;
            
            transform.position = new Vector3(xPos, yPos, transform.position.z);
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
        
        Destroy(gameObject, 0.2f);
    }
    
    void ReachEnd()
    {
        Debug.Log("⚠️ " + enemyType + " reached the end!");
        
        // Damage rice field
        if(GameManager.instance != null)
        {
            // GameManager.instance.TakeDamage(10);
        }
        SceneManager.LoadScene("LooseScene_1");
        
        Destroy(gameObject);
    }
}