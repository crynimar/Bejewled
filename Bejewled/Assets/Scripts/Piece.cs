using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public enum PieceType
{
    Cookie,
    CottonCandy,
    GingerBreadMan,
    GumDrop,
    GummyBeard,
    HeartLollipop,
    Macaron,
    Popsicle
}

public class Piece : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private PieceType pieceCandyType;
    [SerializeField] private Sprite[] possibleSprites;
    [SerializeField] private Cell currentCell;
    [SerializeField] private Image imageComponent;
    [SerializeField] private float animationSpeed;

    public PieceType PieceCandyType { get => pieceCandyType; set => pieceCandyType = value; }
    public Cell CurrentCell { get => currentCell; set => currentCell = value; }
    public RectTransform RectTransform { get => rectTransform; set => rectTransform = value; }

    private Sprite currentSprite;

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

    IEnumerator GoToNewCellAnimation(bool callAction)
    {
        while (RectTransform.anchoredPosition != Vector2.zero)
        {
            float step = animationSpeed * Time.deltaTime; // calculate distance to move
            RectTransform.anchoredPosition = Vector3.MoveTowards(RectTransform.anchoredPosition, Vector2.zero, step);
            yield return null;
        }

        if (callAction)
            CurrentCell.PieceFinishSwipeAnimation();
    }
 
}
