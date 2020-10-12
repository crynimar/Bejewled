﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;   

    [SerializeField] private Grid GridPanel;

    [SerializeField] private Cell lastCellClicked;
    [SerializeField] private float pieceAnimationDethTime;
    [SerializeField] private bool canPlay = false;

    void Awake()
    {
        //Create Singleton instance
        if (Instance == null || Instance != this)
            Instance = this; 
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        GridPanel.InitGrid();
        SetCanPlay(true);
    }

    public void CellWasClicked(Cell c)
    {
        if (canPlay)
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
            c.CurrentPiece.CurrentCell = null;
            PiecePooling.Instance.PoolOnePiece(c.CurrentPiece);
            c.CurrentPiece = null;
        }

       
        GridPanel.FillGrid();
    }

    public void SetCanPlay(bool value) => canPlay = value;

}
