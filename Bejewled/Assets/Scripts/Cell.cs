using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;

public class Cell : MonoBehaviour,  IPointerClickHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    public Cell LeftCell, RightCell, UpCell, DownCell;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image imageComponent;
    [SerializeField] private Vector2 currentPosInBoard;
    [SerializeField] private Piece currentPiece;
    [SerializeField] private Grid grid;
    [SerializeField] public float colorFeedBackTime;

    private Cell lastSwipedCell;
    private Color startColor;

    public Piece CurrentPiece { get => currentPiece; set => currentPiece = value; }
    public Grid Grid { get => grid; set => grid = value; }
    public RectTransform RectTransfom { get => rectTransform; set => rectTransform = value; }
    public Image ImageComponent { get => imageComponent; set => imageComponent = value; }

    //private bool alreadySwipedPieces = false;

    public void Init(int posX, int posY)
    {
        startColor = ImageComponent.color;
        currentPosInBoard = new Vector2(posX, posY);
        ReceivePiece();        
    }

    public void SetAdjacentCells(Cell left, Cell right, Cell up, Cell down)
    {
        LeftCell = left;
        RightCell = right;
        UpCell = up;
        DownCell = down;
    }

    public void ReceivePiece()
    {
        Piece piece = PiecePooling.Instance.GetPiece();

        if (piece != null)
        {
            piece.Init(this);
            piece.RectTransform.anchoredPosition = Vector3.zero;
            CurrentPiece = piece;
        }
        else
            Debug.LogError("Failed in receive Piece");
    }

    public void ChangeColorFeedBack(Color c)
    {
        imageComponent.color = c;
        StartCoroutine(ChangeColorToOriginal());
    }

    public void InitMatch()
    {
        if (UpCell)
            UpCell.SendPieceDown();
        else
            grid.GenerateNewPiece(this);
    }

    public void SendPieceDown()
    {
         if (CurrentPiece == null)
        {
            if(UpCell)
            {
                UpCell.SendPieceDown();
            }    
        }

        if (CurrentPiece != null)
        {
            if (DownCell)
            {
                DownCell.CurrentPiece = CurrentPiece;
                CurrentPiece.CurrentCell = DownCell;
                CurrentPiece.transform.SetParent(DownCell.transform);

                CurrentPiece.GoDownAnimation();
                CurrentPiece = null;

                if (UpCell)                
                    UpCell.SendPieceDown();                
                else                
                    grid.GenerateNewPiece(this);
            }
        }

    }

    public void CanSwipePieces(Cell cellToChange)
    {      
        if (cellToChange == LeftCell) SwipePieces(LeftCell);
        else if (cellToChange == RightCell) SwipePieces(RightCell);
        else if(cellToChange == UpCell) SwipePieces(UpCell);
        else if(cellToChange == DownCell) SwipePieces(DownCell);        
    }
    private void SwipePieces(Cell c, bool backFromSwipe = false) // Action will tell if need to return animation
    {
        lastSwipedCell = c;

        if (backFromSwipe)
            lastSwipedCell = null;

        Piece tempPiece = c.CurrentPiece;

        c.CurrentPiece = CurrentPiece;
        CurrentPiece = tempPiece;

        c.CurrentPiece.transform.SetParent(c.transform);
        CurrentPiece.transform.SetParent(transform);     

        c.currentPiece.CurrentCell = c;
        currentPiece.CurrentCell = this;

        c.CurrentPiece.SwipeAnimation(backFromSwipe);
        CurrentPiece.SwipeAnimation(backFromSwipe);
    }

    public void PieceFinishSwipeAnimation()
    {
        if (lastSwipedCell != null)        
            CheckCombinationAfterSwipe();  
    }

    private void CheckCombinationAfterSwipe()
    {
        List<Cell> MatchedList = new List<Cell>();

        MatchedList = CheckCombinations();
        if (MatchedList.Count <= 0)
        {
            MatchedList = lastSwipedCell.CheckCombinations();

            if(MatchedList.Count<=0)            
                SwipePieces(lastSwipedCell, true); //back from swipe
            else
            {
                ResolveMatch(MatchedList);
            }
        }
        else
        {
            ResolveMatch(MatchedList);
        }
    }

    private void ResolveMatch(List<Cell> MatchedList)
    {
        GameManager.Instance.ResolveMatch(MatchedList); //Resolve Match       
        lastSwipedCell = null;
    }

    IEnumerator ChangeColorToOriginal()
    {
        yield return new WaitForSeconds(colorFeedBackTime);
        ChangeColorFeedBack(startColor);

    }

    #region Check Combinations
    public List<Cell> CheckCombinations()
    {
        List<Cell> MatchedList = new List<Cell>();

        Cell cellToCheck = this;

        #region Check Horizontal

        //Checking RIGHT
        if (cellToCheck.CheckRightCombination())
        {
            MatchedList.Add(cellToCheck);
            MatchedList.Add(cellToCheck.RightCell);

            cellToCheck = cellToCheck.RightCell;         
         
            while (cellToCheck.CheckRightCombination())
            {
                cellToCheck = cellToCheck.RightCell;                
                MatchedList.Add(cellToCheck);
            }

            cellToCheck = this;
        }

        //Checking LEFT
        if (cellToCheck.CheckLeftCombination())
        {
            if(!MatchedList.Contains(cellToCheck))//If the cell exists on list
                MatchedList.Add(cellToCheck);

            MatchedList.Add(cellToCheck.LeftCell);

            cellToCheck = cellToCheck.LeftCell;

            while (cellToCheck.CheckLeftCombination())
            {
                cellToCheck = cellToCheck.LeftCell;
                MatchedList.Add(cellToCheck);
            }
            cellToCheck = this;
        }

        if (MatchedList.Count < 3)        
            MatchedList.Clear();

        else
            return MatchedList;

        #endregion

        #region Check Vertical

        //Checking UP
        if (cellToCheck.CheckUpCombination())
        {
            MatchedList.Add(cellToCheck);
            MatchedList.Add(cellToCheck.UpCell);

            cellToCheck = cellToCheck.UpCell;

            while (cellToCheck.CheckUpCombination())
            {
                cellToCheck = cellToCheck.UpCell;
                MatchedList.Add(cellToCheck);
            }

            cellToCheck = this;
        }

        //Checking Down
        if (cellToCheck.CheckDownCombination())
        {
            if (!MatchedList.Contains(cellToCheck)) //If the cell exists on list
                MatchedList.Add(cellToCheck);

            MatchedList.Add(cellToCheck.DownCell);

            cellToCheck = cellToCheck.DownCell;

            while (cellToCheck.CheckDownCombination())
            {
                cellToCheck = cellToCheck.DownCell;
                MatchedList.Add(cellToCheck);
            }
        }

        if (MatchedList.Count < 3)
            MatchedList.Clear();
        
        return MatchedList;
        #endregion
    }

    public bool CheckLeftCombination()
    {
        if (LeftCell != null)
            if (LeftCell.CurrentPiece.PieceCandyType == CurrentPiece.PieceCandyType)
                return true;
        return false;
    }

    public bool CheckRightCombination()
    {
        if (RightCell != null)
            if (RightCell.CurrentPiece.PieceCandyType == CurrentPiece.PieceCandyType)
                return true;
        return false;
    }

    public bool CheckDownCombination()
    {
        if (DownCell != null)
            if (DownCell.CurrentPiece.PieceCandyType == CurrentPiece.PieceCandyType)
                return true;
        return false;
    }

    public bool CheckUpCombination()
    {
        if (UpCell != null)
            if (UpCell.CurrentPiece.PieceCandyType == CurrentPiece.PieceCandyType)
                return true;
        return false;
    }
    #endregion

    #region Interface Implementations
    public void OnPointerUp(PointerEventData eventData)
    {
        if(GameManager.Instance.CanPlay)
            GameManager.Instance.CellWasClicked(this);
    }  

    public void OnPointerClick(PointerEventData eventData){}

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.HandleMouseExitCell(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.HandleMouseOverCell(this);
    }
    #endregion
}