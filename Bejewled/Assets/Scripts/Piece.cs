using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    private Sprite currentSprite;
    private UnityAction finishSwipeAnimation;

    public void RandomizePiece()
    {
        int rand = UnityEngine.Random.Range(0, possibleSprites.Length);
        currentSprite = possibleSprites[rand];
        imageComponent.sprite = currentSprite;

        pieceCandyType = (PieceType)rand;
    }

    public void Init(Cell c)
    {
        this.rectTransform.anchoredPosition = Vector3.zero;
        currentCell = c;
        RandomizePiece();
        finishSwipeAnimation += currentCell.PieceFinishSwipeAnimation;
    }

    public void Animate()
    {
        //use move toward to next cell
    }

    public void SwipeAnimation(bool returningAnimation)
    {
        StartCoroutine(SwipeAnimationCoroutine(returningAnimation));
    }

    IEnumerator SwipeAnimationCoroutine(bool returningAnimation)
    {
        while (rectTransform.anchoredPosition != Vector2.zero)
        {
            float step = animationSpeed * Time.deltaTime; // calculate distance to move
            rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, Vector2.zero, step);
            yield return null;
        }

        if(!returningAnimation)
        finishSwipeAnimation.Invoke();
    }
 
}
