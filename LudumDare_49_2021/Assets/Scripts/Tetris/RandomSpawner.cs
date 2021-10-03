using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject boundsL;
    public GameObject boundsR;

    public List<GameObject> spawnPrefabs;

    public int spawnAmount;
    public float spawnCooldown;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObject());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnObject()
    {
        int randomPrefabIndex = Random.Range(0, spawnPrefabs.Count);
        float randomXPosition = Random.Range(boundsL.transform.position.x, boundsR.transform.position.x);
        Instantiate(spawnPrefabs[randomPrefabIndex], new Vector3(randomXPosition, 5f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(spawnCooldown);
        StartCoroutine(SpawnObject());
    }
}
