using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI ;

public enum PieceType
{
    Cookie,
    CottonCandy,
    GingerBreadMan,
    GumDrop,
    GummyBeard,
}

public class Piece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private PieceType pieceCandyType;
    [SerializeField] private Sprite[] possibleSprites;
    [SerializeField] private Cell currentCell;
    [SerializeField] private Image imageComponent;
    [SerializeField] private float animationSpeed;

    private Sprite currentSprite;

    public PieceType PieceCandyType { get => pieceCandyType; set => pieceCandyType = value; }
    public Cell CurrentCell { get => currentCell; set => currentCell = value; }
    public RectTransform RectTransform { get => rectTransform; set => rectTransform = value; }


    public void RandomizePiece()
    {
        int rand = UnityEngine.Random.Range(0, possibleSprites.Length);
        currentSprite = possibleSprites[rand];
        imageComponent.sprite = currentSprite;

        pieceCandyType = (PieceType)rand;
    }

    public void Init(Cell c)
    {
        transform.SetParent(c.transform);
        CurrentCell = c;
        RandomizePiece();
    }

    public void Animate()
    {
        //use move toward to next cell
    }

    public void SwipeAnimation(bool needCallAction)
    {
        StartCoroutine(GoToNewCellAnimation(needCallAction));
    }

    public void GoDownAnimation()
    {
        StartCoroutine(GoToNewCellAnimation(false));
    }

    IEnumerator GoToNewCellAnimation(bool backFromSwipe)
    {
        GameManager.Instance.PiecesAnimating++;

        while (RectTransform.anchoredPosition != Vector2.zero)
        {
            float step = animationSpeed * Time.deltaTime; // calculate distance to move
            RectTransform.anchoredPosition = Vector3.MoveTowards(RectTransform.anchoredPosition, Vector2.zero, step);
            yield return null;
        }

        GameManager.Instance.PiecesAnimating--;
        if (!backFromSwipe)
            CurrentCell.PieceFinishSwipeAnimation();

    }



    #region Interface Implementations

    public void OnBeginDrag(PointerEventData eventData){}

    public void OnDrag(PointerEventData eventData){}

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.CanPlay)
            GameManager.Instance.CheckDrag(CurrentCell);
    }
    
    #endregion
}
