using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PieceType
{
    Simple,
    Special
}
public class Piece : MonoBehaviour
{
    private PieceType piecetype = PieceType.Simple;
    public PieceType pieceType { get { return piecetype; } set { piecetype = value; } }
    void Start()
    {
        
    }

   
}
