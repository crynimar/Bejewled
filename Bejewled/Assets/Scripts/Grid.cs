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
        PieceInGrid = new Piece[GridSize, GridSize];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //Create cell
                Cell cell = Instantiate(CellPrefab, transform);
                cell.name= string.Format("Cell-{0},{1}", i, j);

                //get piece from pool
                Piece piece = PiecePooling.Instance.GetPiece();

                if (piece != null)
                {
                    PieceInGrid[i, j] = piece;
                    piece.Init();
                    piece.transform.SetParent(cell.transform);

                    cell.Init(i, j, piece); //set grid position and piece to cell                    
                }
            }
        }

        PositionPieces();
    }

    private void PositionPieces()
    {
        foreach(Piece p in PieceInGrid)
        {

        }
    }
}
