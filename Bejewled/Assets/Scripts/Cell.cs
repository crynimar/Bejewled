using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour,  IPointerClickHandler, IPointerUpHandler,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform rectTransform;
    public Cell LeftCell, RightCell, UpCell, DownCell;

    [SerializeField] private Vector2 currentPosInBoard;
    [SerializeField] private Piece currentPiece;
    public Piece CurrentPiece { get => currentPiece; set => currentPiece = value; }

    private Cell lastSwipedCell;

   
    //private bool alreadySwipedPieces = false;

    public void Init(int posX, int posY)
    {
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
            piece.transform.SetParent(transform);
            piece.Init(this);
            CurrentPiece = piece;
        }
        else
            Debug.Log("Failed in receive Piece");
    }

    public void CanSwipePieces(Cell cellToChange)
    {      
        if (cellToChange == LeftCell) SwipePieces(LeftCell);
        else if (cellToChange == RightCell) SwipePieces(RightCell);
        else if(cellToChange == UpCell) SwipePieces(UpCell);
        else if(cellToChange == DownCell) SwipePieces(DownCell);        
    }
    private void SwipePieces(Cell c, bool returningAnimation = false)
    {
        lastSwipedCell = c;

        if (returningAnimation)
            lastSwipedCell = null;

        Piece tempPiece = c.CurrentPiece;

        c.CurrentPiece.transform.SetParent(transform);
        CurrentPiece.transform.SetParent(c.transform);

        c.CurrentPiece = CurrentPiece;
        CurrentPiece = tempPiece;

        CurrentPiece.SwipeAnimation(returningAnimation);
        c.CurrentPiece.SwipeAnimation(returningAnimation);
    }

    public void PieceFinishSwipeAnimation()
    {
        if(lastSwipedCell != null)
        {
            if (!CheckCombinations())
            {      
                if (!lastSwipedCell.CheckCombinations())
                {                    
                    SwipePieces(lastSwipedCell, true);                   
                    return;
                }
      
                else
                {
                    //TODO: QUEBRAR
                    lastSwipedCell = null;
                }
            }
            else
            {
                //TODO: QUEBRAR 
                lastSwipedCell = null;
            }
        }
    }

    #region Check Combinations
    public bool CheckCombinations()
    {
        int matchs = 0;
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
                
                return true;
            }
            else if (RightIsMatch)
            {
                MatchedList.Add(this);
                MatchedList.Add(LeftCell);
                MatchedList.Add(RightCell);
                return true;
            }
        }

        if(RightIsMatch)
        {
            if (RightCell.CheckRightCombination())
            {
                MatchedList.Add(this);
                MatchedList.Add(RightCell);
                MatchedList.Add(RightCell.RightCell);
                return true;
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
                return true;
            }
            else if (DownIsMacth)
            {
                MatchedList.Add(this);
                MatchedList.Add(UpCell);
                MatchedList.Add(DownCell);
                return true;
            }
        }

        if (DownIsMacth)
        {
            if (DownCell.CheckDownCombination())
            {
                MatchedList.Add(this);
                MatchedList.Add(DownCell);
                MatchedList.Add(DownCell.DownCell);
                return true;
            }
        }
        #endregion


        return false;
      
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
