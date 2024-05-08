using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeCellEdge : MonoBehaviour{
    public MazeCell cell, othercell;

    public MazeDirection direction;

    public void Initialize(MazeCell cell, MazeCell othercell, MazeDirection direction)
    {
        this.cell = cell;
        this.othercell = othercell;
        this.direction = direction;
        cell.SetEdge(this,direction);
        transform.parent = cell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = direction.ToRotation();
    }
}
