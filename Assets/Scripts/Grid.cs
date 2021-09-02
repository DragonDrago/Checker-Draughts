using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Grid : MonoBehaviour
{
    public Grid topRight;
    public Grid topLeft;
    public Grid bottomRight;
    public Grid bottomLeft;
    
    public bool isHighlighted { get; set; }

    private ChooseTurn turn = ChooseTurn.None;
    public ChooseTurn GetTurn { get { return turn; } set { turn = value; } }

    public Piece GetPiece { get; set; }

    private Board board;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        isHighlighted = false;
    }

    private void OnMouseDown()
    {
         if(turn != ChooseTurn.None && board.turnSwitcherAfterGetting == false)
         {
             board.IdentifyHighlited(this);
         }
        
        
            board.MoveToGrid(this);
    }   
}

