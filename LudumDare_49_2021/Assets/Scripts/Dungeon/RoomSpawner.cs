using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    private RoomTemplates templates;

    private int randomRoomIndex;
    public bool hasSpawned = false;

    public int openingDirection;
    // 1 --> Up
    // 2 --> Down
    // 3 --> Left
    // 4 --> Right


    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

        Invoke("Spawn", 0.2f);
    }

    // Update is called once per frame
    private void Spawn()
    {
        if (!hasSpawned)
        {
            switch (openingDirection)
            {
                case 1:
                    randomRoomIndex = Random.Range(0, templates.bottomRooms.Length);
                    Instantiate(templates.bottomRooms[randomRoomIndex].transform, transform.position, templates.bottomRooms[randomRoomIndex].transform.rotation);
                    break;
                case 2:
                    randomRoomIndex = Random.Range(0, templates.bottomRooms.Length);
                    Instantiate(templates.bottomRooms[randomRoomIndex].transform, transform.position, templates.bottomRooms[randomRoomIndex].transform.rotation);
                    break;
                case 3:
                    randomRoomIndex = Random.Range(0, templates.topRooms.Length);
                    Instantiate(templates.topRooms[randomRoomIndex].transform, transform.position, templates.topRooms[randomRoomIndex].transform.rotation);
                    break;
                case 4:
                    randomRoomIndex = Random.Range(0, templates.rightRooms.Length);
                    Instantiate(templates.rightRooms[randomRoomIndex].transform, transform.position, templates.rightRooms[randomRoomIndex].transform.rotation);
                    break;
            }
        }
        hasSpawned = true;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag("SpawnPoint"))
        {
            if (collision.GetComponent<RoomSpawner>().hasSpawned == false && hasSpawned == false)
            {
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
            }
            hasSpawned = true;
        }
	}
}
