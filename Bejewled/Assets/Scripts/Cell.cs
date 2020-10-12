using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Vector2 currentPosInBoard;
    [SerializeField] private Piece currentPiece;

    public void Init(int posX, int posY, Piece p)
    {
        currentPosInBoard = new Vector2(posX, posY);
        currentPiece = p;
    }
}
