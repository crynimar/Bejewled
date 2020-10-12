using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public class Piece : MonoBehaviour
{
    [SerializeField] private Sprite[] PossibleSprites;
    [SerializeField] private Image ImageComponent;
    [SerializeField] private Vector2 currentPosInBoard;

    private Sprite CurrentSprite;

    public void RandomizePiece()
    {
        CurrentSprite = PossibleSprites[Random.Range(0, PossibleSprites.Length)];
        ImageComponent.sprite = CurrentSprite;
    }
    public void Init()
    {
        RandomizePiece();
    }
}
