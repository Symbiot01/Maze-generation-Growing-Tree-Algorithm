using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour{
    public IntVector2 coordinates;
    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.count];
    private int initializedCellCount = 0;
    public void SetEdge(MazeCellEdge edge, MazeDirection direction)
    {
        edges[(int)direction] = edge;
        initializedCellCount += 1;
    }

    public MazeCellEdge GetEdge(MazeDirection direction)
    {
        return edges[(int)direction];
    }

    public MazeDirection RandomUninitilisedDirection
    {
        get
        {
            int skips = Random.Range(0, MazeDirections.count - initializedCellCount);
            for (int i = 0; i < MazeDirections.count; i++)
            {
                if (edges[i] == null)
                {
                    if (skips == 0)
                    {
                        return (MazeDirection)i;
                    }
                    else
                    {
                        skips -= 1;
                    }
                }
            }
            throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
        }
    }
    public bool IsFullyInitialized {
        get
        {
            return initializedCellCount == MazeDirections.count;
        }
    }
}
