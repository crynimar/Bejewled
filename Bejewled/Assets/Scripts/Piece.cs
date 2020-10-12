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
    [SerializeField] private RectTransform RectTransform;
    [SerializeField] private PieceType PieceCandyType;
    [SerializeField] private Sprite[] PossibleSprites;
    [SerializeField] private Image ImageComponent;
    [SerializeField] private Cell currentCell;

    public PieceType pieceCandyType { get => PieceCandyType; set => PieceCandyType = value; }

    private Sprite CurrentSprite;

    public void RandomizePiece()
    {
        int rand = Random.Range(0, PossibleSprites.Length);
        CurrentSprite = PossibleSprites[rand];
        ImageComponent.sprite = CurrentSprite;

        pieceCandyType = (PieceType)rand;
    }
    public void Init(Cell cell)
    {
        currentCell = cell;
        this.RectTransform.anchoredPosition = Vector3.zero;

        RandomizePiece();
        
    }
    public void Animate()
    {
        //use move toward to next cell
    }
}
