using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;   

    [SerializeField] private Grid GridPanel;

    [SerializeField] private Cell lastCellClicked;
    [SerializeField] private float pieceAnimationDethTime;
    [SerializeField] private bool canPlay;
    [SerializeField] private Cell currentMouseOverCell;

    [SerializeField] private int piecesAnimating = 0;
    [SerializeField] private bool stillLookingForMatch = false;

    public bool CanPlay{ get => canPlay; set => canPlay = value; }
    public bool StillLookingForMatch 
    {
        get => stillLookingForMatch;

        set
        {
            stillLookingForMatch = value;

            if (stillLookingForMatch)
                CanPlay = false;
            else
                CheckIfCanPlay();
        }
    }

    public int PiecesAnimating 
    {
        get => piecesAnimating;

        set
        {
            piecesAnimating = value;

            if(piecesAnimating > 0)
                CanPlay = false;
            else
                CheckIfCanPlay();
        }
    }

   

    void Awake()
    {
        //Create Singleton instance
        if (Instance == null || Instance != this)
            Instance = this; 
    }

    private void Start()
    {
        CanPlay = false;
        Init();
    }

    private void Init()
    {
        GridPanel.InitGrid();      
    }

    public void CellWasClicked(Cell c)
    {
        if (CanPlay)
        {
            if (lastCellClicked == null)
                lastCellClicked = c;

            else if (lastCellClicked == c)
            {
                lastCellClicked = null;
                return;
            }

            else
            {
                lastCellClicked.CanSwipePieces(c);
                lastCellClicked = null;
            }
        }
        else
            lastCellClicked = null;
    }

    public void ResolveMatch(List<Cell> matchedCells)
    {
        foreach (Cell c in matchedCells)
        {
            c.ChangeColorFeedBack(Color.cyan);
            c.CurrentPiece.CurrentCell = null;
            PiecePooling.Instance.PoolOnePiece(c.CurrentPiece);
            c.CurrentPiece = null;
        }

        GridPanel.FillGrid(matchedCells);
    }        

    public void HandleMouseOverCell(Cell c)
    {
        if (currentMouseOverCell == null || currentMouseOverCell != c)
            currentMouseOverCell = c;        
    }

    public void HandleMouseExitCell(Cell c)
    {
        if (currentMouseOverCell = c)
            currentMouseOverCell = null;        
    }

    public void CheckDrag(Cell dragged)
    {
        if (CanPlay)
        {
            if (dragged != currentMouseOverCell)
            {
                lastCellClicked = null;
                dragged.CanSwipePieces(currentMouseOverCell);
            }
        }
    }

    private void CheckIfCanPlay()
    {
        if (piecesAnimating == 0 && !StillLookingForMatch)
            CanPlay = true;
    }
}
