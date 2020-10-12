using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public RectTransform rectTransform;

    [SerializeField] private Vector2 currentPosInBoard;
    [SerializeField] private Piece currentPiece;

    public void Init(int posX, int posY)
    {
        currentPosInBoard = new Vector2(posX, posY);
        ReceivePiece();
    }

    public void ReceivePiece()
    {
        Piece piece = PiecePooling.Instance.GetPiece();

        if (piece != null)
        {
            piece.transform.SetParent(transform);
            piece.Init(this);
            currentPiece = piece;
        }
        else
            Debug.Log("Failed in receive Piece");
    }
}
