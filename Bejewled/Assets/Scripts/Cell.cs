using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour,  IPointerClickHandler, IPointerUpHandler,IBeginDragHandler, IDragHandler, IEndDragHandler
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
    private void SwipePieces(Cell c, bool needCallAction = true) // Action will tell if need to return animation
    {
        lastSwipedCell = c;

        if (!needCallAction)
            lastSwipedCell = null;

        Piece tempPiece = c.CurrentPiece;

        c.CurrentPiece.transform.SetParent(transform);
        CurrentPiece.transform.SetParent(c.transform);

        c.CurrentPiece = CurrentPiece;
        CurrentPiece = tempPiece;

        currentPiece.CurrentCell = this;
        c.currentPiece.CurrentCell = c;

        CurrentPiece.SwipeAnimation(needCallAction);
        c.CurrentPiece.SwipeAnimation(needCallAction);
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
                SwipePieces(lastSwipedCell, false); //Swipe back  
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

        #region Check Left and Right
        bool LeftIsMatch = CheckLeftCombination();
        bool RightIsMatch = CheckRightCombination();

        if(LeftIsMatch)
        {
            if (LeftCell.CheckLeftCombination())
            {
                MatchedList.Add(this);
                MatchedList.Add(LeftCell);
                MatchedList.Add(LeftCell.LeftCell);                
                return MatchedList;
            }
            else if (RightIsMatch)
            {
                MatchedList.Add(this);
                MatchedList.Add(LeftCell);
                MatchedList.Add(RightCell);
                return MatchedList;
            }
        }

        if(RightIsMatch)
        {
            if (RightCell.CheckRightCombination())
            {
                MatchedList.Add(this);
                MatchedList.Add(RightCell);
                MatchedList.Add(RightCell.RightCell);
                return MatchedList;
            }
        }
        #endregion

        #region Check Up and Down
        bool UpIsMacth = CheckUpCombination();
        bool DownIsMacth = CheckDownCombination();

        if (UpIsMacth)
        {
            if (UpCell.CheckUpCombination())
            {
                MatchedList.Add(this);
                MatchedList.Add(UpCell);
                MatchedList.Add(UpCell.UpCell);
                return MatchedList;
            }
            else if (DownIsMacth)
            {
                MatchedList.Add(this);
                MatchedList.Add(UpCell);
                MatchedList.Add(DownCell);
                return MatchedList;
            }
        }

        if (DownIsMacth)
        {
            if (DownCell.CheckDownCombination())
            {
                MatchedList.Add(this);
                MatchedList.Add(DownCell);
                MatchedList.Add(DownCell.DownCell);
                return MatchedList;
            }
        }
        #endregion


        return MatchedList;
      
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
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin drag");
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("on drag");
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end drag");
    }     

    public void OnPointerClick(PointerEventData eventData){}
    #endregion
}
