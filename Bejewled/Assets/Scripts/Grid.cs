using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{    
    [SerializeField] private Piece PiecePrefab;
    [SerializeField] private Cell CellPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int PieceSize;

    private Piece[,] PieceInGrid;
    private Cell[,] CellinGrid;
    private int GridSize;


    private void CreateCells(int posX, int posY)
    {
       
    }

    public void InitGrid()
    {
        GridSize = width * height;
        CellinGrid = new Cell[width, height];
        PieceInGrid = new Piece[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //Create cell
                Cell cell = Instantiate(CellPrefab, transform);
                cell.name= string.Format("Cell-{0},{1}", i, j);
                CellinGrid[i, j] = cell;
                cell.Init(i, j);  
            }
        }

        PositionCells();
    }

    private void PositionCells()
    {
        int index= 0;
        int row = 0;
        Debug.Log(CellinGrid.Length);
        foreach(Cell c in CellinGrid)
        {
            c.rectTransform.anchoredPosition = new Vector3(PieceSize * index, row, 0);
            index++;

            if (index % width == 0)
            {
                row -= PieceSize;
                index = 0;
            }
        }
    }

    private void CheckCombinations()
    {

    }
}
