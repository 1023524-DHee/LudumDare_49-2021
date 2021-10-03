using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;

    public List<Transform> waypoints1;
    public List<Transform> waypoints2;
    public List<Transform> waypoints3;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy_Coroutine());
    }

    private void SpawnEnemy()
    {
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject spawnedEnemy = Instantiate(enemyPrefabs[randomEnemyIndex], transform.position, Quaternion.identity);

        spawnedEnemy.GetComponent<EnemyPatrol>().SetWaypoints(waypoints1, waypoints2, waypoints3);
    }

    private IEnumerator SpawnEnemy_Coroutine()
    {
        SpawnEnemy();
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnEnemy_Coroutine());
    }
}
