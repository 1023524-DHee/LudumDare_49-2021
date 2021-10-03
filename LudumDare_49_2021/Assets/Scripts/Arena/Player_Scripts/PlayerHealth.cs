using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Image healthBar;

    private bool isDead;

    private float currentHealth;
    private float drainAmount = 0.5f;

    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();

        GameManager.current.onPlayerTakeDamage += TakeDamage;
        GameManager.current.onPhaseChange += SetDrainAmount;
        GameManager.current.onPlayerHeal += HealPlayer;
        GameManager.current.onGameFinish += StopHealthDrain;

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        DrainHealth();
    }

    private void Die()
    {
        if (!isDead)
        {
            isDead = true;
            GameManager.current.PlayerDeath();
        }
    }

    private void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
    }

    private void HealPlayer(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    private void DrainHealth()
    {
        currentHealth -= drainAmount * Time.deltaTime;

        healthBar.fillAmount = currentHealth/maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void SetDrainAmount()
    {
        drainAmount += 0.3f;
    }

    public void StopHealthDrain()
    {
        drainAmount = 0f;
    }
}
