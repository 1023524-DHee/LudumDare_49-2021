using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Image healthBar;

    private float currentHealth;
    private float drainAmount = 0.5f;

    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();

        GameManager.current.onChangeHealthDrain += SetDrainAmount;
        GameManager.current.onPlayerHeal += HealPlayer;

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        DrainHealth();
    }

    private void HealPlayer(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    private void DrainHealth()
    {
        currentHealth -= drainAmount * Time.deltaTime;

        healthBar.fillAmount = currentHealth/maxHealth;
    }

    public void SetDrainAmount(float amount)
    {
        drainAmount = amount;
    }
}
