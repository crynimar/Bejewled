using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePooling : MonoBehaviour
{
    public static PiecePooling Instance;

    [SerializeField] private int poolSize;
    [SerializeField] private Piece piceToPool;

    [SerializeField] private Queue<Piece> pooledPieces;
    [SerializeField] private List<Piece> inUsePieces;

    void Awake()
    {
        //Create Singleton instance
        if (Instance == null || Instance != this)
            Instance = this;

        InitPooling();
    }

    private void InitPooling()
    {
        pooledPieces = new Queue<Piece>();
        inUsePieces = new List<Piece>();

        for (int i = 0; i < poolSize; i++)
        {
            Piece piece =  Instantiate(piceToPool, transform);
            piece.gameObject.SetActive(false);
            pooledPieces.Enqueue(piece);
        }
    }

    public Piece GetPiece()
    {
        if (pooledPieces.Count > 0)
        {
            Piece piece = pooledPieces.Dequeue();
            piece.gameObject.SetActive(true);
            inUsePieces.Add(piece);

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
        if (inUsePieces.Contains(piece))
        {
            inUsePieces.Remove(piece);
            piece.gameObject.SetActive(false);
            piece.transform.SetParent(transform);
            pooledPieces.Enqueue(piece);
        }
        else
            Debug.LogError("This piece was not in use");
   }
}
