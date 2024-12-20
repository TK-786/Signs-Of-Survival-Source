using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    //public HealthBar healthBar; 

    private void Start()
    {
        currentHealth = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("player demage takedn");
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        //healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
    }
    public float getHealth(){
        Debug.Log("current player health: " + currentHealth);
        return currentHealth;
    }
    public void Heal()
    {
        currentHealth = maxHealth;
        Debug.Log("player is reset to full health");
        //healthBar.SetHealth(currentHealth);
    }
    //hhh
}
