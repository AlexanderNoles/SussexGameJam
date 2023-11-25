using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public Vector3Int gridPos;

    private void OnValidate() {
        transform.localPosition = new Vector3(gridPos.x, gridPos.y, 0.0f) * RepairScreenManagement.cellSize;
    }

    protected virtual void Start() {
        RepairScreenManagement.AddGridObject(transform.localPosition, this);
    }
}
