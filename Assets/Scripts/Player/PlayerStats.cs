using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public HealthBar healthBar; 

    void Start()
    {
        float difficultyMode = PlayerPrefs.GetFloat("DifficultyMode", 1f);

        maxHealth = maxHealth / difficultyMode;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("player damage taken");
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthBar.SetHealth(currentHealth);

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
}
