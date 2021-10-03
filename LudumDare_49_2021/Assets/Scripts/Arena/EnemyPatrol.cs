using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private Rigidbody2D enemyRB;
    private Transform currentWaypoint;

    private bool waypointsSet;

    private bool finishWaypoint1;
    private bool finishWaypoint2;
    private bool finishWaypoint3;

    public Transform finalWaypoint;

    private List<Transform> waypoints1;
    private List<Transform> waypoints2;
    private List<Transform> waypoints3;

    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(waypointsSet) MoveToPoint();
    }

    public void SetWaypoints(List<Transform> newWaypoints1, List<Transform> newWaypoints2, List<Transform> newWaypoints3)
    {
        waypoints1 = newWaypoints1;
        waypoints2 = newWaypoints2;
        waypoints3 = newWaypoints3;
        waypointsSet = true;
        SelectRandomPoint();
    }

    private void SelectRandomPoint()
    {
        if (!finishWaypoint1)
        {
            currentWaypoint = waypoints1[Random.Range(0, waypoints1.Count)];
        }
        else if (finishWaypoint1 && !finishWaypoint2)
        {
            currentWaypoint = waypoints2[Random.Range(0, waypoints2.Count)];
        }
        else if (finishWaypoint2 && !finishWaypoint3)
        {
            currentWaypoint = waypoints3[Random.Range(0, waypoints3.Count)];
        }
        else if (finishWaypoint3)
        {
            currentWaypoint = finalWaypoint;
        }
    }

    private void MoveToPoint()
    {
        Vector2 newPosition = Vector2.MoveTowards(transform.position, currentWaypoint.position, Time.deltaTime * 10f);
        enemyRB.MovePosition(newPosition);

        if (Vector2.Distance(transform.position, currentWaypoint.position) <= 0.01f)
        {
            if (!finishWaypoint1)
            {
                finishWaypoint1 = true;
                SelectRandomPoint();
            }
            else if (!finishWaypoint2)
            {
                finishWaypoint2 = true;
                SelectRandomPoint();
            }
            else if (!finishWaypoint3)
            {
                finishWaypoint3 = true;
                SelectRandomPoint();
            }
            else if (finishWaypoint3)
            {
                Destroy(gameObject);
            }
        }
    }
}
