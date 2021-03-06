using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCounter : MonoBehaviour
{
    private TMP_Text enemyCounter;

    private int currentPhase = 1;

    private int baseNumberEnemiesReq = 40;
    private int currentNumberEnemiesKilled;
    private int currentNumberEnemiesReq;

    public GameObject destroyer;
    public GameObject centerObject;

    // Start is called before the first frame update
    void Start()
    {
        enemyCounter = GetComponent<TMP_Text>();

        GameManager.current.onEnemyDeath += EnemiesKilled;
        GameManager.current.onPlayerDeath += OnPlayerDeath;

        

        currentNumberEnemiesKilled = -1;
        EnemiesKilled();
        UpdateCounter();
    }

	private void EnemiesKilled()
    {
        currentNumberEnemiesKilled++;
        UpdateCounter();

        if (currentNumberEnemiesKilled >= currentNumberEnemiesReq)
        {
            switch (currentPhase)
            {
                case 1:
                    GameManager.current.PhaseChange();
                    currentNumberEnemiesKilled = 0;
                    currentNumberEnemiesReq = baseNumberEnemiesReq * 1;
                    currentPhase = 2;
                    break;
                case 2:
                    CameraFollow.current.ChangeVignetteValue(0.5f);
                    GameManager.current.PhaseChange();
                    DestroyEnemies();
                    currentNumberEnemiesKilled = 0;
                    currentNumberEnemiesReq = Mathf.RoundToInt(baseNumberEnemiesReq * 1.5f);
                    currentPhase = 3;
                    break;
                case 3:
                    CameraFollow.current.ChangeVignetteValue(0f);
                    CameraFollow.current.ChangeChromaticValue(0.5f);
                    GameManager.current.PhaseChange();
                    DestroyEnemies();
                    currentNumberEnemiesKilled = 0;
                    currentNumberEnemiesReq = baseNumberEnemiesReq * 2;
                    currentPhase = 4;
                    break;
                case 4:
                    CameraFollow.current.ChangeVignetteValue(1f);
                    CameraFollow.current.ChangeChromaticValue(0f);
                    GameManager.current.PhaseChange();
                    DestroyEnemies();
                    currentNumberEnemiesKilled = 0;
                    currentNumberEnemiesReq = Mathf.RoundToInt(baseNumberEnemiesReq * 2.5f);
                    currentPhase = 5;
                    break;
                case 5:
                    CameraFollow.current.ChangeVignetteValue(0f);
                    CameraFollow.current.ChangeChromaticValue(1f);
                    GameManager.current.PhaseChange();
                    DestroyEnemies();
                    currentNumberEnemiesKilled = 0;
                    currentNumberEnemiesReq = baseNumberEnemiesReq * 3;
                    currentPhase = 6;
                    break;
                case 6:
                    CameraFollow.current.ChangeVignetteValue(0f);
                    CameraFollow.current.ChangeChromaticValue(0f);
                    GameManager.current.PhaseChange();
                    GameManager.current.OnGameFinish();
                    DestroyEnemies();
                    currentNumberEnemiesKilled = 0;
                    break;
            }
        }
    }
    private void DestroyEnemies()
    {
        Instantiate(destroyer, centerObject.transform.position, Quaternion.identity);
    }

    private void UpdateCounter()
    {
        enemyCounter.text = "" + currentNumberEnemiesKilled + "/" + currentNumberEnemiesReq;
    }

    private void OnPlayerDeath()
    {
        StartCoroutine(MultipleDestroyEnemies_Coroutine());
    }

    private IEnumerator MultipleDestroyEnemies_Coroutine()
    {
        DestroyEnemies();
        yield return new WaitForSeconds(0.5f);
        DestroyEnemies();
        yield return new WaitForSeconds(0.5f);
        DestroyEnemies();
        yield return new WaitForSeconds(0.5f);
        DestroyEnemies();
        yield return new WaitForSeconds(0.5f);
    }
}
