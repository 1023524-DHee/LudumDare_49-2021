using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, ITakeDamage
{
    private int currentHealth;

    public List<GameObject> sprites;

    public int maxHealth;

	// Start is called before the first frame update
	void Start()
    {
        currentHealth = maxHealth;
    }

    private void Die()
    {
        int randomSprite = Random.Range(0, sprites.Count);
        GameObject spawnedEffect = Instantiate(sprites[randomSprite], transform.position, Quaternion.Euler(0,0,Random.Range(0,360f)));

        spawnedEffect.GetComponent<SpriteRenderer>().sortingLayerName = "DeathEffects";

        GameManager.current.EnemyDeath();
        GameManager.current.PlayerHeal(1);

        Destroy(gameObject);
    }

    private void DamageTaken(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int amount)
    {
        DamageTaken(amount);
    }
}
