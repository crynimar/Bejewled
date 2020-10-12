using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int pieceSize;

    private Cell[,] CellinGrid;

    public void InitGrid()
    {
        CellinGrid = new Cell[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //Create cell
                Cell cell = Instantiate(cellPrefab, transform);
                cell.name= string.Format("Cell-{0},{1}", i, j);
                CellinGrid[i, j] = cell;
                cell.Init(i, j);  
            }
        }

        PositionCells();
        PopulateAdjacentCells();
       // CheckCombinations();
    }

    private void PositionCells()
    {
        int index= 0;
        int row = 0;

        foreach(Cell c in CellinGrid)
        {
            c.rectTransform.anchoredPosition = new Vector3(pieceSize * index, row, 0);
            index++;

            if (index % width == 0)
            {
                row -= pieceSize;
                index = 0;
            }
        }
    }

    private void PopulateAdjacentCells()
    {
        Cell left, right, up, down;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //get the adjacences
                left = (j-1) < 0 ? null : CellinGrid[i, j - 1];
                right= (j + 1) >= width ? null : CellinGrid[i, j + 1];               
                up = (i-1) < 0 ? null : CellinGrid[i - 1, j];
                down = (i + 1) >= height ? null : CellinGrid[i + 1, j];              

                CellinGrid[i, j].SetAdjacentCells(left, right, up, down);
            }
        }
    }

   // private void CheckCombinations()
   // {
   //     bool checkMatch = false;
   //     Cell currentCell;
   //     Cell nextCell;
   //     List<Cell> MatchedCells = new List<Cell>();
   //
   //     for (int i = 0; i < width; i++)
   //     {
   //        
   //         for (int j = 0; j < height; j++)
   //         {
   //             checkMatch = CellinGrid[i, j].CheckCombinations();
   //             if (checkMatch)
   //                 return;
   //             //if (currentCell.RightCell == null) continue;
   //             //
   //             //nextCell = currentCell.RightCell; //next cell is right cell
   //             //
   //             //if (currentCell.CurrentPiece.PieceCandyType == nextCell.CurrentPiece.PieceCandyType)
   //             //{
   //             //    currentCell = nextCell;
   //             //    if (currentCell.RightCell == null) continue;
   //             //
   //             //    nextCell = currentCell.RightCell;
   //             //
   //             //    if (currentCell.CurrentPiece.PieceCandyType == nextCell.CurrentPiece.PieceCandyType)
   //             //    {
   //             //        match = true;
   //             //        MatchedCells.Add(nextCell);
   //             //        MatchedCells.Add(currentCell);
   //             //        MatchedCells.Add(CellinGrid[i, j]);
   //             //        break;
   //             //    }
   //             //}
   //         }
   //     }
   // }
}
