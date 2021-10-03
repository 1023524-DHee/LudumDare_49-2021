using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, ITakeDamage
{
    private SpriteRenderer spriteRenderer;

    private int currentHealth;

    private bool isFlashing;

    public List<GameObject> sprites;

    public int maxHealth;

	// Start is called before the first frame update
	void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

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

        if(!isFlashing) StartCoroutine(OnDamageFlash());
    }

    private IEnumerator OnDamageFlash()
    {
        isFlashing = true;

        spriteRenderer.color = Color.black;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.black;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;

        isFlashing = false;
    }

    public void TakeDamage(int amount)
    {
        DamageTaken(amount);
    }
}
