using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;   
    [SerializeField] private Grid GridPanel;

    private Cell lastCellClicked;

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
    }

    public void CellWasClicked(Cell c)
    {
        if(lastCellClicked== null)
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
}
