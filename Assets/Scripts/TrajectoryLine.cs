using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryLine : MonoBehaviour
{
    public LineRenderer lr;

    private void Awake(){
        lr = GetComponent<LineRenderer>();
    }

    public void RenderLine(Vector3 startpoint, Vector3 endpoint){
        lr.positionCount = 2;
        Vector3[] points = new Vector3[2];
        points[0] = startpoint;
        points[1] = endpoint;
        lr.SetPositions(points);
    }

    public void EndLine(){
        lr.positionCount = 0;
    }
}
