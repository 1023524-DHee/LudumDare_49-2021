using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

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

    public event Action<int> onPlayerTakeDamage;
    public void PlayerTakeDamage(int amount)
    {
        if (onPlayerTakeDamage != null)
        {
            onPlayerTakeDamage(amount);
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
}
