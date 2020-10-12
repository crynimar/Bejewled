using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public class Piece : MonoBehaviour
{
    [SerializeField] private Sprite[] PossibleSprites;
    [SerializeField] private Image ImageComponent;

    private Sprite CurrentSprite;

    private void Start()
    {
        ChooseSprite();
    }
    private void ChooseSprite()
    {
        CurrentSprite = PossibleSprites[Random.Range(0, PossibleSprites.Length)];
        ImageComponent.sprite = CurrentSprite;
    }
}
