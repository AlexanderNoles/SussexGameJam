using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReciever : GridObject
{
    public KeyCode keyCode = KeyCode.W;

    private List<Vector3> linePositions = new List<Vector3>();
    private MovementOutput targetOutput = null;

    private List<Vector3> neededPositions = new List<Vector3>();

    private List<Transform> lineSegments = new List<Transform>();

    protected override void Start() {
        base.Start();
        foreach(Transform child in transform)
        {
            neededPositions.Add(RepairScreenManagement.ConvertPositionToClosestGridPosition(child.localPosition + transform.localPosition));
        }
    }

    public void ResetLine()
    {
        linePositions.Clear();
        lineSegments = LineRenderer.ResetPath(lineSegments);
        if(targetOutput != null)
        {
            targetOutput.UnRegisterInput();
            targetOutput = null;
        }
    }

    public void NotiftyOfMovementOutput(MovementOutput output)
    {
        //First check if all the needed positions are in the line
        foreach(Vector3 pos in neededPositions)
        {
            if(!linePositions.Contains(pos))
            {          
                RepairScreenManagement._instance.ResetCurrentLineDrawer();
                return;
            }
        }


        //Tell the movement output to setup stuff in RepairScreenManagment
        if(output.RegisterInput(this))
        {
            targetOutput = output;
        }
        else
        {
            RepairScreenManagement._instance.ResetCurrentLineDrawer();
        }
    }

    public bool HasPositionInLine(Vector3 pos)
    {
        return linePositions.Contains(pos);
    }

    public void AddPositionToLine(Vector3 pos)
    {
        linePositions.Add(pos);
        lineSegments = LineRenderer.DrawPath(lineSegments, linePositions, transform.position);
    }

    private void Update() {
        Vector3 previousPos = transform.position;

        foreach(Vector3 pos in linePositions)
        {
            Debug.DrawLine(pos, previousPos, Color.white);
            previousPos = pos;
        }
    }
}
