using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChooseTurn
{
    None,
    White,
    Black
}

public enum TurnOfMove
{
    None,
    WhiteToMove,
    BlackToMove
}

public class Board : MonoBehaviour
{
    List<Grid> grids = new List<Grid>();
    Dictionary<int, List<Grid>> graph = new Dictionary<int, List<Grid>>();

    private List<GameObject> highlightedList = new List<GameObject>();

    private List<Grid> highlitedGrids = new List<Grid>();

    private List<Piece> stonesToGet = new List<Piece>();

    [SerializeField]
    private Piece whiteStonePrefab;

    [SerializeField]
    private Piece blackStonePrefab;

    [SerializeField]
    private GameObject highlitedPrefab;

    public Grid selectedGrid { get; set; }

    public Grid gridWhileTaking { get; set; }

    private ChooseTurn turn = ChooseTurn.None;
    public ChooseTurn Turn{get{return turn;} set{turn = value;}}

    public int whiteScore { get; set; }
    private int blackScore { get; set; }
    public bool turnSwitcherAfterGetting { get; set; }

    private TurnOfMove turnOfMove = TurnOfMove.None;
    public TurnOfMove TurnOfMoveInGame { get { return turnOfMove; } set { turnOfMove = value; } }

    void Start()
    {

    var findSceneGrids = GetComponentsInChildren<Grid>();
    grids.AddRange(findSceneGrids);

 
      for(int i = 0; i < 32 ; i ++)
      {
          graph[i] = new List<Grid>();

          if(grids[i].topLeft != null)
          {
              graph[i].Add(grids[i].topLeft);
          }
          if(grids[i].topRight != null)
          {
              graph[i].Add(grids[i].topRight);
          }
          if(grids[i].bottomRight != null)
          {
              graph[i].Add(grids[i].bottomRight);
          }
          if(grids[i].bottomLeft != null)
          {
              graph[i].Add(grids[i].bottomLeft);
          }
      } 
      

      turn = ChooseTurn.White;
      CreatePiece(turn);

    }



    //////////// _____________METHOD____

    public void CreatePiece(ChooseTurn turn)
    {
        if(turn == ChooseTurn.White)
        {
            turnOfMove = TurnOfMove.WhiteToMove;
            for (var i = 0; i < 12; i++)
            {
                var piece = Instantiate(whiteStonePrefab, grids[i].transform);
                grids[i].GetTurn = ChooseTurn.White;
                grids[i].GetPiece = piece;
                stonesToGet.Add(grids[i].GetPiece);
            }
            for(var i = 31; i >= 20; i--)
            {
                var piece = Instantiate(blackStonePrefab, grids[i].transform);
                grids[i].GetTurn = ChooseTurn.Black;
                grids[i].GetPiece = piece;
                stonesToGet.Add(grids[i].GetPiece);
            }
        }
        else
        {
            turnOfMove = TurnOfMove.BlackToMove;
           for (var i = 0; i < 12; i++)
            {
                var piece = Instantiate(blackStonePrefab, grids[i].transform);
                grids[i].GetTurn = ChooseTurn.Black;
                grids[i].GetPiece = piece;
                stonesToGet.Add(grids[i].GetPiece);
            }
            for(var i = 31; i >= 20; i--)
            {
                var piece = Instantiate(whiteStonePrefab, grids[i].transform);
                grids[i].GetTurn = ChooseTurn.White;
                grids[i].GetPiece = piece;
                stonesToGet.Add(grids[i].GetPiece);
            } 
        }
    }





    //////////// _____________METHOD____

    public void IdentifyHighlited( Grid grid)
    {
        Debug.Log("Im in higlighting");
        RemoveHighlighted();
        selectedGrid = grid;
        if ((selectedGrid.GetTurn == ChooseTurn.White && turnOfMove == TurnOfMove.WhiteToMove) ||
            (selectedGrid.GetTurn == ChooseTurn.Black && turnOfMove == TurnOfMove.BlackToMove))
        {
            if ((grid.GetTurn == ChooseTurn.White && turn == ChooseTurn.White)
            || (grid.GetTurn == ChooseTurn.Black && turn == ChooseTurn.Black))
            {

                if (grid.bottomLeft != null)
                {
                    if (grid.bottomLeft.GetTurn == ChooseTurn.None)
                    {
                        var obj = Instantiate(highlitedPrefab, grid.bottomLeft.transform);
                        highlightedList.Add(obj);
                        grid.bottomLeft.isHighlighted = true;
                        highlitedGrids.Add(grid.bottomLeft);
                    }
                    else if (grid.bottomLeft.bottomLeft != null &&  grid.bottomLeft.GetTurn != grid.GetTurn
                        && grid.bottomLeft.bottomLeft.GetTurn == ChooseTurn.None)
                    {
                        var obj = Instantiate(highlitedPrefab, grid.bottomLeft.bottomLeft.transform);
                        highlightedList.Add(obj);
                        grid.bottomLeft.bottomLeft.isHighlighted = true;
                        highlitedGrids.Add(grid.bottomLeft.bottomLeft);
                    }
                    
                }

                if (grid.bottomRight != null)
                {
                 
                    if (grid.bottomRight.GetTurn == ChooseTurn.None)
                    {
                        var obj = Instantiate(highlitedPrefab, grid.bottomRight.transform);
                        highlightedList.Add(obj);
                        grid.bottomRight.isHighlighted = true;
                        highlitedGrids.Add(grid.bottomRight);
                    }
                    else if (grid.bottomRight.bottomRight != null && grid.bottomRight.GetTurn != grid.GetTurn
                        && grid.bottomRight.bottomRight.GetTurn == ChooseTurn.None)
                    {
                        var obj = Instantiate(highlitedPrefab, grid.bottomRight.bottomRight.transform);
                        highlightedList.Add(obj);
                        grid.bottomRight.bottomRight.isHighlighted = true;
                        highlitedGrids.Add(grid.bottomRight.bottomRight);
                    }
                }
                ////// new testing
                if (grid.topLeft != null && grid.topLeft.topLeft != null && grid.topLeft.GetPiece != null
                    && grid.topLeft.GetTurn != grid.GetTurn && grid.topLeft.topLeft.GetTurn == ChooseTurn.None)
                {
                    var obj = Instantiate(highlitedPrefab, grid.topLeft.topLeft.transform);
                    highlightedList.Add(obj);
                    grid.topLeft.topLeft.isHighlighted = true;
                    highlitedGrids.Add(grid.topLeft.topLeft);

                }
                if (grid.topRight != null && grid.topRight.topRight != null && grid.topRight.GetTurn != grid.GetTurn
                       && grid.topRight.GetPiece != null && grid.topRight.topRight.GetTurn == ChooseTurn.None)
                {

                    var obj = Instantiate(highlitedPrefab, grid.topRight.topRight.transform);
                    highlightedList.Add(obj);
                    grid.topRight.topRight.isHighlighted = true;
                    highlitedGrids.Add(grid.topRight.topRight);

                }
            }
            else
            {
                if (grid.topLeft != null)
                {
                   
                    if (grid.topLeft.GetTurn == ChooseTurn.None)
                    {
                        var obj = Instantiate(highlitedPrefab, grid.topLeft.transform);
                        highlightedList.Add(obj);
                        grid.topLeft.isHighlighted = true;
                        highlitedGrids.Add(grid.topLeft);
                    }
                    else if(grid.topLeft.topLeft != null && grid.topLeft.GetTurn != grid.GetTurn
                        && grid.topLeft.topLeft.GetTurn == ChooseTurn.None)
                    {
                        var obj = Instantiate(highlitedPrefab, grid.topLeft.topLeft.transform);
                        highlightedList.Add(obj);
                        grid.topLeft.topLeft.isHighlighted = true;
                        highlitedGrids.Add(grid.topLeft.topLeft);
                    }
                }
                if (grid.topRight != null)
                {
                    if (grid.topRight.GetTurn == ChooseTurn.None)
                    {
                        var obj = Instantiate(highlitedPrefab, grid.topRight.transform);
                        highlightedList.Add(obj);
                        grid.topRight.isHighlighted = true;
                        highlitedGrids.Add(grid.topRight);
                    }
                    else if (grid.topRight.topRight != null && grid.topRight.GetTurn != grid.GetTurn 
                        && grid.topRight.topRight.GetTurn == ChooseTurn.None)
                    {
                        var obj = Instantiate(highlitedPrefab, grid.topRight.topRight.transform);
                        highlightedList.Add(obj);
                        grid.topRight.topRight.isHighlighted = true;
                        highlitedGrids.Add(grid.topRight.topRight);
                    }
                }
                ////  new testing
                if (grid.bottomRight != null && grid.bottomRight.GetPiece != null && grid.bottomRight.bottomRight != null
                    && grid.bottomRight.GetTurn != grid.GetTurn && grid.bottomRight.bottomRight.GetTurn == ChooseTurn.None)
                {

                    var obj = Instantiate(highlitedPrefab, grid.bottomRight.bottomRight.transform);
                    highlightedList.Add(obj);
                    grid.bottomRight.bottomRight.isHighlighted = true;
                    highlitedGrids.Add(grid.bottomRight.bottomRight);

                }

                if (grid.bottomLeft != null && grid.bottomLeft.bottomLeft != null && grid.bottomLeft.GetPiece != null
                    && grid.bottomLeft.GetTurn != grid.GetTurn && grid.bottomLeft.bottomLeft.GetTurn == ChooseTurn.None)
                {

                    var obj = Instantiate(highlitedPrefab, grid.bottomLeft.bottomLeft.transform);
                    highlightedList.Add(obj);
                    grid.bottomLeft.bottomLeft.isHighlighted = true;
                    highlitedGrids.Add(grid.bottomLeft.bottomLeft);

                }
            }

        }
        else
        {
            return;
        }

    }


    //////////// _____________METHOD____
    private void GetStoneAndRemove(Grid grid)
    {
        if (selectedGrid.topRight != null && selectedGrid.topRight.topRight != null && selectedGrid.topRight.topRight == grid
            && selectedGrid.topRight.GetPiece != null && selectedGrid.GetTurn != selectedGrid.topRight.GetTurn)
        {
            if (stonesToGet.Contains(selectedGrid.topRight.GetPiece))
            {
                Destroy(selectedGrid.topRight.GetPiece.gameObject);
                ScoreAdder(selectedGrid.topRight);
                selectedGrid.topRight.GetPiece = null;
                selectedGrid.topRight.GetTurn = ChooseTurn.None;
                stonesToGet.Remove(selectedGrid.topRight.GetPiece);
                selectedGrid = grid;
                ContinueHighlighting(selectedGrid);
            }
          

        }
        else if (selectedGrid.topLeft != null && selectedGrid.topLeft.topLeft != null && selectedGrid.topLeft.topLeft == grid
            && selectedGrid.topLeft.GetPiece != null && selectedGrid.GetTurn != selectedGrid.topLeft.GetTurn)
        {
            if (stonesToGet.Contains(selectedGrid.topLeft.GetPiece))
            {
                Destroy(selectedGrid.topLeft.GetPiece.gameObject);
                ScoreAdder(selectedGrid.topLeft);
                selectedGrid.topLeft.GetPiece = null;
                selectedGrid.topLeft.GetTurn = ChooseTurn.None;
                stonesToGet.Remove(selectedGrid.topLeft.GetPiece);
                selectedGrid = grid;
                ContinueHighlighting(selectedGrid);
            }
           
        }
        else if (selectedGrid.bottomRight != null && selectedGrid.bottomRight.bottomRight != null && selectedGrid.bottomRight.bottomRight == grid
            && selectedGrid.bottomRight.GetPiece != null && selectedGrid.GetTurn != selectedGrid.bottomRight.GetTurn)
        {
            if (stonesToGet.Contains(selectedGrid.bottomRight.GetPiece))
            {
                Destroy(selectedGrid.bottomRight.GetPiece.gameObject);
                ScoreAdder(selectedGrid.bottomRight);
                selectedGrid.bottomRight.GetPiece = null;
                selectedGrid.bottomRight.GetTurn = ChooseTurn.None;
                stonesToGet.Remove(selectedGrid.bottomRight.GetPiece);
                selectedGrid = grid;
                ContinueHighlighting(selectedGrid);
            }
           
           
        }
        else if (selectedGrid.bottomLeft != null && selectedGrid.bottomLeft.bottomLeft != null && selectedGrid.bottomLeft.bottomLeft == grid
            && selectedGrid.bottomLeft.GetPiece != null && selectedGrid.GetTurn != selectedGrid.bottomLeft.GetTurn)
        {
            if (stonesToGet.Contains(selectedGrid.bottomLeft.GetPiece))
            {
                Destroy(selectedGrid.bottomLeft.GetPiece.gameObject);
                ScoreAdder(selectedGrid.bottomLeft);
                selectedGrid.bottomLeft.GetPiece = null;
                selectedGrid.bottomLeft.GetTurn = ChooseTurn.None;
                stonesToGet.Remove(selectedGrid.bottomLeft.GetPiece);
                selectedGrid = grid;
                ContinueHighlighting(selectedGrid);
            }
            
        }
        else
        {
            return;
        }
    }





    //////////// _____________METHOD____

    public void ContinueHighlighting(Grid grid)
    {
         if(grid.topRight != null && grid.topRight.topRight != null && grid.topRight.GetPiece != null
                && grid.topRight.topRight.GetPiece == null && grid.GetPiece != grid.topRight.GetPiece
                && grid.topRight.topRight.GetTurn == ChooseTurn.None && grid.GetTurn != grid.topRight.GetTurn)
         {
            var obj = Instantiate(highlitedPrefab, grid.topRight.topRight.transform);
            highlightedList.Add(obj);
            grid.topRight.topRight.isHighlighted = true;
            highlitedGrids.Add(grid.topRight.topRight);
            turnSwitcherAfterGetting = true;
        }

        if(grid.topLeft != null && grid.topLeft.topLeft != null && grid.topLeft.GetPiece != null
                 && grid.topLeft.topLeft.GetPiece == null && grid.GetPiece != grid.topLeft.GetPiece
                 && grid.topLeft.topLeft.GetTurn == ChooseTurn.None && grid.GetTurn != grid.topLeft.GetTurn)
         {
            var obj = Instantiate(highlitedPrefab, grid.topLeft.topLeft.transform);
            highlightedList.Add(obj);
            grid.topLeft.topLeft.isHighlighted = true;
            highlitedGrids.Add(grid.topLeft.topLeft);
            turnSwitcherAfterGetting = true;
        }

         if(grid.bottomRight != null && grid.bottomRight.bottomRight != null && grid.bottomRight.GetPiece != null
                  && grid.bottomRight.bottomRight.GetPiece == null && grid.GetPiece != grid.bottomRight.GetPiece 
                  && grid.bottomRight.bottomRight.GetTurn == ChooseTurn.None && grid.GetTurn != grid.bottomRight.GetTurn)
         {
            var obj = Instantiate(highlitedPrefab, grid.bottomRight.bottomRight.transform);
            highlightedList.Add(obj);
            grid.bottomRight.bottomRight.isHighlighted = true;
            highlitedGrids.Add(grid.bottomRight.bottomRight);
            turnSwitcherAfterGetting = true;
        }

          if(grid.bottomLeft != null && grid.bottomLeft.bottomLeft != null && grid.bottomLeft.GetPiece != null
                 && grid.bottomLeft.bottomLeft.GetPiece == null && grid.GetPiece != grid.bottomLeft.GetPiece
                 && grid.bottomLeft.bottomLeft.GetTurn == ChooseTurn.None && grid.GetTurn != grid.bottomLeft.GetTurn)
          {
            
            var obj = Instantiate(highlitedPrefab, grid.bottomLeft.bottomLeft.transform);
            highlightedList.Add(obj);
            grid.bottomLeft.bottomLeft.isHighlighted = true;
            highlitedGrids.Add(grid.bottomLeft.bottomLeft);
            turnSwitcherAfterGetting = true;
        }
        else
        {
            
            turnSwitcherAfterGetting = false;
            return;
        } 
        
       
    }


    //////////// _____________METHOD____

    private void ScoreAdder(Grid grid)
    {
        if (grid.GetTurn == ChooseTurn.White)
        {
            whiteScore++;
        }
        else
        {
            blackScore++;
        }
    }




    //////////// _____________METHOD____

    public void MoveToGrid(Grid grid)
    {
        if(grid.isHighlighted == true && grid.GetTurn == ChooseTurn.None && selectedGrid != null)
        {
         
            grid.GetPiece = selectedGrid.GetPiece;
           
            grid.GetTurn = selectedGrid.GetTurn;
     
            selectedGrid.GetTurn = ChooseTurn.None;
           
            selectedGrid.GetPiece.transform.parent = grid.transform;  // bu code selecteddagi obj ni gridga child qiliqb joylashtiradi uni ichiga qoyadi
                                                                    
            selectedGrid.GetPiece.transform.localPosition = Vector2.zero; // bu esa positsiyasini togirlaydi
                                                                        
            selectedGrid.GetPiece = null;
            
                RemoveHighlighted();
                
                GetStoneAndRemove(grid);

            if (turnSwitcherAfterGetting == false)
            {
                TurnSwithcer();
            } 

        }
    }






    //////////// _____________METHOD____

    private void TurnSwithcer()
    {
        if (turnOfMove == TurnOfMove.WhiteToMove)
        {
            turnOfMove = TurnOfMove.BlackToMove;
        }
        else
        {
            turnOfMove = TurnOfMove.WhiteToMove;
        }
    }





    //////////// _____________METHOD____
    private void RemoveHighlighted()
    {
        foreach (var v in highlightedList)
        {
            Destroy(v);
        }
        highlightedList.Clear();

        foreach( var b in highlitedGrids)
        {
            b.isHighlighted = false;
        }
        highlitedGrids.Clear();
    }

    void Update()
    {
        
    }

    

   
}
