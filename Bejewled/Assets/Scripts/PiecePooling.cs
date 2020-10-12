using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePooling : MonoBehaviour
{
    public static PiecePooling Instance;

    [SerializeField] private int PoolSize;
    [SerializeField] private Piece PiceToPool;

    [SerializeField] private Queue<Piece> PooledPieces;
    [SerializeField] private List<Piece> InUsePieces;

    void Awake()
    {
        //Create Singleton instance
        if (Instance == null || Instance != this)
            Instance = this;

        InitPooling();
    }

    private void InitPooling()
    {
        PooledPieces = new Queue<Piece>();
        InUsePieces = new List<Piece>();

        for (int i = 0; i < PoolSize; i++)
        {
            Piece piece =  Instantiate(PiceToPool, transform);
            piece.gameObject.SetActive(false);
            PooledPieces.Enqueue(piece);
        }
    }

    public Piece GetPiece()
    {
        if (PooledPieces.Count > 0)
        {
            Piece piece = PooledPieces.Dequeue();
            piece.gameObject.SetActive(true);
            InUsePieces.Add(piece);

            return piece;
        }
        else
        {
            Debug.LogError("Not enought pooled pieces");
            return null;
        }
    }

   public void PoolOnePiece(Piece piece)
   {
        if (InUsePieces.Contains(piece))
        {
            InUsePieces.Remove(piece);
            piece.gameObject.SetActive(false);
            piece.transform.SetParent(transform);
            PooledPieces.Enqueue(piece);
        }
        else
            Debug.LogError("This piece was not in use");
   }
}
