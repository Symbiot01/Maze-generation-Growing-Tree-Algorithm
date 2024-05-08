using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Maze : MonoBehaviour{
    [SerializeField] private IntVector2 size;
    [SerializeField] private float genrationStepDelaySize;
    
    [SerializeField] private MazeCell cellPrefab;
    private MazeCell[,] cells;
    [SerializeField] private MazePassage passagePrefab;
    [SerializeField] private MazeWall wallPrefab;
    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(genrationStepDelaySize);
        cells = new MazeCell[size.x, size.z];
        IntVector2 coordinates = RandomCoordinates;
        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);
        while (activeCells.Count > 0)
        {
            yield return delay; 
            DoNextGenerationStep(activeCells);

        }
    }

    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        activeCells.Add(CreateCell(RandomCoordinates));
    }
    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        
        int currentIndex = activeCells.Count - 1;
        MazeCell currentcell = activeCells[currentIndex];
        if (currentcell.IsFullyInitialized)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        MazeDirection direction = currentcell.RandomUninitilisedDirection;
        IntVector2 coordinates = currentcell.coordinates + direction.DirectionToIntVector2();
        if (ContainsCoordinates(coordinates))
        {
            MazeCell neighbour = GetCell(coordinates);
            if (GetCell(coordinates) == null)
            {
                neighbour = CreateCell(coordinates);
                CreatePassage(currentcell, neighbour, direction); 
                activeCells.Add(neighbour);
            }
            else
            {
                CreateWall(currentcell, neighbour, direction);
            }
        }
        else
        {
            CreateWall(currentcell, null, direction);
        }
        
    }

    private void CreateWall(MazeCell cell, MazeCell othercell, MazeDirection direction)
    {
        MazeWall wall = Instantiate(wallPrefab) as MazeWall;
        wall.Initialize(cell, othercell, direction);
        if (othercell != null)
        {
            wall = Instantiate(wallPrefab) as MazeWall;
            wall.Initialize(othercell, cell, direction.GetOpposite());
        }
    }
    private void CreatePassage(MazeCell cell, MazeCell othercell, MazeDirection direction)
    {
        MazePassage wall = Instantiate(passagePrefab) as MazePassage;
        wall.Initialize(cell, othercell, direction);
        wall = Instantiate(passagePrefab) as MazePassage;
        wall.Initialize(othercell, cell, direction.GetOpposite());
    }
    
    public MazeCell GetCell(IntVector2 coordinates)
    {   
        return cells[coordinates.x, coordinates.z];
    }
    public IntVector2 RandomCoordinates {
        get {
            return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }
    public bool ContainsCoordinates (IntVector2 coordinate) {
        return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
    }
    MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell newcell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newcell;
        newcell.name = "MazeCell" + coordinates.x + "," + coordinates.z;
        newcell.coordinates = coordinates;
        newcell.transform.parent = transform;
        newcell.transform.localPosition = 
            new Vector3(coordinates.x - (size.x*0.5f), 0, coordinates.z - (size.z*0.5f));
        return newcell;

    }
}