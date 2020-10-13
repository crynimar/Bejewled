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
    [SerializeField] private float eraseSpeed;

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
                cell.Grid = this;
            }
        }

        PositionCells();
        PopulateAdjacentCells();
        CheckGridMatchs();        
    }

    private void PositionCells()
    {
        int index= 0;
        int row = 0;

        foreach(Cell c in CellinGrid)
        {
            c.RectTransfom.anchoredPosition = new Vector3(pieceSize * index, row, 0);
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
    public void FillGrid()
    {
        foreach (Cell c in CellinGrid)
        {
            if (c.CurrentPiece == null)
            {
                c.InitMatch();
            }
        }

        CheckGridMatchs();
    }

    public void GenerateNewPiece(Cell c)
    {
        Piece p = PiecePooling.Instance.GetPiece();
        c.CurrentPiece = p;

        p.Init(c);

        p.transform.SetParent(c.transform);
        p.RectTransform.anchoredPosition = new Vector2(0, pieceSize); 
        p.GoDownAnimation();
    }

    public void CheckGridMatchs()
    {
       GameManager.Instance.StillLookingForMatch = true;
        List<Cell> matchList = new List<Cell>();
        foreach (Cell c in CellinGrid)
        {
            matchList = c.CheckCombinations();
            if (matchList.Count > 0)
            {
                StartCoroutine(CallResolveMatch(matchList));
                return;
            }                
        }
        GameManager.Instance.StillLookingForMatch = false;
    }

    IEnumerator CallResolveMatch(List<Cell> matchList)
    {
        yield return new WaitForSeconds(eraseSpeed);
        GameManager.Instance.ResolveMatch(matchList);       
    }

   
}
