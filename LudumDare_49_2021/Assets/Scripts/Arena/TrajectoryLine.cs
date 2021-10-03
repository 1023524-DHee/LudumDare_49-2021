using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    public void SetRotation(Vector3 rotation)
    {
        float angle = Mathf.Atan2(rotation.y - transform.position.y, rotation.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    //// Start is called before the first frame update
    //void Start()
    //{
    //    lineRenderer = GetComponent<LineRenderer>();
    //}

    //public void RenderLine(Vector3 startPoint, Vector3 endPoint)
    //{
    //    lineRenderer.positionCount = 2;
    //    Vector3[] points = new Vector3[2];

    //    points[0] = startPoint;
    //    points[1] = endPoint;

    //    lineRenderer.SetPositions(points);
    //}

    //public void EndLine()
    //{
    //    lineRenderer.positionCount = 0;
    //}
}
