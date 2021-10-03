using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    public GameObject door;
    
    private void Awake()
    {
        current = this;
    }

    public event Action onEnemyDeath;
    public void EnemyDeath()
    {
        if (onEnemyDeath != null)
        {
            onEnemyDeath();
        }
    }

    public event Action<int> onPlayerHeal;
    public void PlayerHeal(int amount)
    {
        if (onPlayerHeal != null)
        {
            onPlayerHeal(amount);
        }
    }

    public event Action<float> onPlayerTakeDamage;
    public void PlayerTakeDamage(float amount)
    {
        if (onPlayerTakeDamage != null)
        {
            onPlayerTakeDamage(amount);
        }
    }

    public event Action onPlayerDeath;
    public void PlayerDeath()
    {
        if (onPlayerDeath != null)
        {
            onPlayerDeath();
            StartCoroutine(RestartScene());
        }
    }

    public event Action<float> onChangeHealthDrain;
    public void ChangeHealthDrain(float amount)
    {
        if (onChangeHealthDrain != null)
        {
            onChangeHealthDrain(amount);
        }
    }

    public event Action onPhaseChange;
    public void PhaseChange()
    {
        if (onPhaseChange != null)
        {
            onPhaseChange();
		}
	}

    public event Action onGameFinish;
    public void OnGameFinish()
    {
        if (onGameFinish != null)
        {
            onGameFinish();

            door.SetActive(true);
        }
    }

    public event Action onExitDoorEntered;
    public void ExitDoorEntered()
    {
        if (onExitDoorEntered != null)
        {
            onExitDoorEntered();

            LoadCreditsScene();
        }
    }

	private IEnumerator RestartScene()
	{
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadCreditsScene()
    {
        StartCoroutine(CreditsScene_Coroutine());
    }

    private IEnumerator CreditsScene_Coroutine()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Credits");
    }
}
