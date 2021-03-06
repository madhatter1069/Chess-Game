using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance{set;get;}
    private bool[,] allowedMoves{set;get;}
    public ChessPiece [,] Chesspieces{set;get;}
    private ChessPiece selectedChessPiece;
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessPiecesPrefabs;
    private List<GameObject> activeChessPieces;

    private Material previousMat;
    public Material selectedMat;

    public int[] EnPassantMove{set;get;}

    public bool isWhiteTurn = true;
    [System.NonSerialized] public ChessPiece WhiteKing;
    [System.NonSerialized] public ChessPiece BlackKing;


    private void Start()
    {
        Instance = this; 
        spawnAllChessPieces();
    }
    private void Update()
    {
        UpdateSelection();
        //DrawChessboard();

        if (Input.GetMouseButtonDown(0))
        {
            if(selectionX>=0 && selectionY>=0)
            {
                if(selectedChessPiece == null)
                {
                    //select the chessman
                    SelectChesspiece(selectionX,selectionY);
                }
                else
                {
                    //move the chessman
                    MoveChesspiece(selectionX, selectionY);
                }
            }
        }
        if(inCheck(WhiteKing))
            if(Checkmate(true))
                EndGame();
        if(inCheck(BlackKing))
            if(Checkmate(false))
                EndGame();
    
    }

    private void CastleReset(bool isWhite)
    {
        if(isWhite)
            WhiteKing.castle = false;
        else 
            BlackKing.castle = false;
        for(int i = 0; i < 8; ++i)
        {
            for(int j = 0; j < 8; ++j)
            {
                ChessPiece c = Chesspieces[i,j];
                if(c != null && c.GetType() == typeof(Rook) && c.isWhite == isWhite)
                {
                    c.castle = false;
                }
            }
        }
    }

    private void SelectChesspiece(int x, int y)
    {
        if(Chesspieces[x,y] == null)
            return;
        
        if(Chesspieces[x,y].isWhite != isWhiteTurn)
            return;
        if(!hasAtLeastOneMove(x,y))
            return;
        
        selectedChessPiece = Chesspieces[x,y];

        previousMat = selectedChessPiece.GetComponent<MeshRenderer>().material;
        selectedMat.mainTexture = previousMat.mainTexture;
        selectedChessPiece.GetComponent<MeshRenderer >().material = selectedMat;

        BoardHighlight.Instance.HighlightAllowedMoves(allowedMoves);
    }

    private void MoveChesspiece(int x, int y)
    {
        if(allowedMoves[x,y])
        {
            ChessPiece c = Chesspieces[x,y];

            if(c != null && c.isWhite != isWhiteTurn)
            {
                //Capture piece
                //if king
                /*if(c.GetType() == typeof(King))
                {
                    //end game
                    //if(!hasAtLeastOneMove(x,y))
                    EndGame();
                    return;
                }  */

                activeChessPieces.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            if(selectedChessPiece.GetType() == typeof(King))
            {
                CastleReset(selectedChessPiece.isWhite);
                //do castle move
                if(x-selectedChessPiece.CurrentX == 2)
                {
                    c = Chesspieces[x+1,y];
                    Chesspieces[x+1,y] = null;
                    c.transform.position = getTileCenter(c.CurrentX-2,c.CurrentY);
                    c.SetPosition(c.CurrentX-2,c.CurrentY);
                    Chesspieces[c.CurrentX-2,c.CurrentY] = c;
                }
                else if(x-selectedChessPiece.CurrentX == -2)
                {
                    c = Chesspieces[x-2,y];
                    Chesspieces[x-2,y] = null;
                    c.transform.position = getTileCenter(c.CurrentX+3,c.CurrentY);
                    c.SetPosition(c.CurrentX+3,c.CurrentY);
                    Chesspieces[c.CurrentX+3,c.CurrentY] = c;
                }
            }
            if(selectedChessPiece.GetType() == typeof(Rook))
            {
                CastleReset(selectedChessPiece.isWhite);
            }
            if(x == EnPassantMove[0] && y == EnPassantMove[1])//destroy chess piece if en passant move chosen
            {
                if(isWhiteTurn)//white turn
                    c = Chesspieces[x,y-1];
                else //black turn
                    c = Chesspieces[x,y+1];

                activeChessPieces.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            EnPassantMove[0] = -1;//reset en passant move so not possible again
            EnPassantMove[1] = -1;
            if(selectedChessPiece.GetType() == typeof(Pawn))//promotion and en passant
            {
                if(y == 7)//white
                {
                    activeChessPieces.Remove(selectedChessPiece.gameObject);
                    Destroy(selectedChessPiece.gameObject);
                    SpawnChessPieces(1,x,y);
                    SelectChesspiece(x,y); 
                    selectedChessPiece = Chesspieces[x,y];
                }
                else if(y == 0)//black
                {
                    activeChessPieces.Remove(selectedChessPiece.gameObject);
                    Destroy(selectedChessPiece.gameObject);
                    SpawnChessPieces(7,x,y);
                    SelectChesspiece(x,y); 
                    selectedChessPiece = Chesspieces[x,y];
                }
                
                if(selectedChessPiece.CurrentY == 1 && y == 3)
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y-1;
                }
                else if(selectedChessPiece.CurrentY == 6 && y == 4)
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y+1;
                }
            }

            Chesspieces[selectedChessPiece.CurrentX,selectedChessPiece.CurrentY] = null;
            selectedChessPiece.transform.position = getTileCenter(x,y);
            selectedChessPiece.SetPosition(x,y);
            Chesspieces[x,y] = selectedChessPiece;

            isWhiteTurn = !isWhiteTurn;
        }
        selectedChessPiece.GetComponent<MeshRenderer>().material = previousMat;
        BoardHighlight.Instance.hideHighlights();
        selectedChessPiece = null;
    }

    private bool Checkmate(bool isWhite)
    {
        for(int i = 0; i < 8; ++i)
        {
            for(int j = 0; j < 8; ++j)
            {
                ChessPiece c = Chesspieces[i,j];
                if(c != null && c.isWhite == isWhite)//if chesspiece exist and is same color as the one needed to be checked 
                {
                    if(hasAtLeastOneMove(i,j))
                        return false;//return that not in checkmate if it has a move;
                }
            }
        }
        //Debug.Log(isWhite+" Loses");
        return true;
    }

    private void EndGame()//resets the board to start state
    {
        if(isWhiteTurn)
            Debug.Log("Black team wins");
        else
            Debug.Log("White team wins");

        foreach(GameObject go in activeChessPieces)
            Destroy(go);
        
        isWhiteTurn = true;
        BoardHighlight.Instance.hideHighlights();
        spawnAllChessPieces(); 
    }

    private bool hasAtLeastOneMove(int x, int y)//check if a piece has a possible move
    {
        allowedMoves = Chesspieces[x,y].PossibleMove();
        for(int i = 0; i< 8; ++i)
            for(int j = 0; j<8; ++j)
                if(allowedMoves[i,j])
                    return true;
        return false;
    }

    public bool inCheck(ChessPiece king)//searches in all directions to see if king is in check
    {
        bool check = false;
        ChessPiece c;
        int i,j;
        bool[,] moves;

        //right
        i = king.CurrentX;
        while(true)
        {
            i++;
            if(i >= 8)
                break;
            c = Chesspieces[i, king.CurrentY];
            if(c == null){}//keep going if nothing
            else if(c.isWhite == king.isWhite)//if same color stop this direction is good
                break;
            else 
            {
                if(c.isWhite != king.isWhite)//if not same color
                {
                    moves = c.PossibleMove();//get moves of pice found
                    //if the coordinates of king are found in the possible 
                    //moves of the piece found then king in check.
                    check = findKing(moves, king.CurrentX, king.CurrentY);
                    if(check){
                        return check;
                    }
                }
                break;
            }    
        }

        //left
        i = king.CurrentX;
        while(true)
        {
            i--;
            if(i < 0)
                break;
            c = Chesspieces[i, king.CurrentY];
            if(c == null){}//keep going if nothing
            else if(c.isWhite == king.isWhite)//if same color stop this direction is good
                break;
            else 
            {
                if(c.isWhite != king.isWhite)//if not same color
                {
                    moves = c.PossibleMove();//get moves of pice found
                    //if the coordinates of king are found in the possible 
                    //moves of the piece found then king in check.
                    check = findKing(moves, king.CurrentX, king.CurrentY);
                    if(check){
                        return check;
                    }
                }
                break;
            }    
        }

        //up
        i = king.CurrentY;
        while(true)
        {
            i++;
            if(i >= 8)
                break;
            c = Chesspieces[king.CurrentX, i];
            if(c == null){}//keep going if nothing
            else if(c.isWhite == king.isWhite)//if same color stop this direction is good
                break;
            else 
            {
                if(c.isWhite != king.isWhite)//if not same color
                {
                    moves = c.PossibleMove();//get moves of pice found
                    //if the coordinates of king are found in the possible 
                    //moves of the piece found then king in check.
                    check = findKing(moves, king.CurrentX, king.CurrentY);
                    if(check){
                        return check;
                    }
                }
                break;
            }    
        }

        //down
        i = king.CurrentY;
        while(true)
        {
            i--;
            if(i < 0)
                break;
            c = Chesspieces[king.CurrentX, i];
            if(c == null){}//keep going if nothing
            else if(c.isWhite == king.isWhite)//if same color stop this direction is good
                break;
            else 
            {
                if(c.isWhite != king.isWhite)//if not same color
                {
                    moves = c.PossibleMove();//get moves of pice found
                    //if the coordinates of king are found in the possible 
                    //moves of the piece found then king in check.
                    check = findKing(moves, king.CurrentX, king.CurrentY);
                    if(check){
                        return check;
                    }
                }
                break;
            }    
        }

        //up right
        i = king.CurrentX;
        j = king.CurrentY;
        while(true)
        {
            i++;
            j++;
            if (i>= 8 || j >= 8)
                break;
            c = Chesspieces[i, j];
            if(c == null){}//keep going if nothing
            else if(c.isWhite == king.isWhite)//if same color stop this direction is good
                break;
            else 
            {
                if(c.isWhite != king.isWhite)//if not same color
                {
                    moves = c.PossibleMove();//get moves of pice found
                    //if the coordinates of king are found in the possible 
                    //moves of the piece found then king in check.
                    check = findKing(moves, king.CurrentX, king.CurrentY);
                    if(check){
                        return check;
                    }
                }
                break;
            }    
        }

        //up left
        i = king.CurrentX;
        j = king.CurrentY;
        while(true)
        {
            i--;
            j++;
            if (i < 0 || j >= 8)
                break;
            c = Chesspieces[i, j];
            if(c == null){}//keep going if nothing
            else if(c.isWhite == king.isWhite)//if same color stop this direction is good
                break;
            else 
            {
                if(c.isWhite != king.isWhite)//if not same color
                {
                    moves = c.PossibleMove();//get moves of pice found
                    //if the coordinates of king are found in the possible 
                    //moves of the piece found then king in check.
                    check = findKing(moves, king.CurrentX, king.CurrentY);
                    if(check){
                        return check;
                    }
                }
                break;
            }    
        }

        //down left
        i = king.CurrentX;
        j = king.CurrentY;
        while(true)
        {
            i--;
            j--;
            if (i < 0 || j < 0)
                break;
            c = Chesspieces[i, j];
            if(c == null){}//keep going if nothing
            else if(c.isWhite == king.isWhite)//if same color stop this direction is good
                break;
            else 
            {
                if(c.isWhite != king.isWhite)//if not same color
                {
                    moves = c.PossibleMove();//get moves of piece found
                    //if the coordinates of king are found in the possible 
                    //moves of the piece found then king in check.
                    check = findKing(moves, king.CurrentX, king.CurrentY);
                    if(check){
                        return check;
                    }
                }
                break;
            }    
        }

        //down right
        i = king.CurrentX;
        j = king.CurrentY;
        while(true)
        {
            i++;
            j--;
            if (i >= 8 || j < 0)
                break;
            c = Chesspieces[i, j];
            if(c == null){}//keep going if nothing
            else if(c.isWhite == king.isWhite)//if same color stop this direction is good
                break;
            else 
            {
                if(c.isWhite != king.isWhite)//if not same color
                {
                    moves = c.PossibleMove();//get moves of pice found
                    //if the coordinates of king are found in the possible 
                    //moves of the piece found then king in check.
                    check = findKing(moves, king.CurrentX, king.CurrentY);
                    if(check){
                        return check;
                    }
                }
                break;
            }    
        }

        //UUR
        if(knightCheck(king.CurrentX+1, king.CurrentY+2, king))
            return true;
        //UUL
        if(knightCheck(king.CurrentX-1, king.CurrentY+2, king))
            return true;
        //URR
        if(knightCheck(king.CurrentX+2, king.CurrentY+1, king))
            return true;
        //ULL
        if(knightCheck(king.CurrentX-2, king.CurrentY+1, king))
            return true;
        //DDR
        if(knightCheck(king.CurrentX+1, king.CurrentY-2, king))
            return true;
        //DDL
        if(knightCheck(king.CurrentX-1, king.CurrentY-2, king))
            return true;
        //DRR
        if(knightCheck(king.CurrentX+2, king.CurrentY-1, king))
            return true;
        //DLL
        if(knightCheck(king.CurrentX-2, king.CurrentY-1, king))
            return true;

        return false;
    }

    private bool knightCheck(int x, int y, ChessPiece king)//checks for check if direction of a knight
    //return true if knight is at x,y
    {
        ChessPiece c;
        if(x >= 0 && x < 8 && y >=0 && y < 8)
        {
            c = Chesspieces[x,y];
            if(c == null){}
            else if(c.GetType() == typeof(Knight) && c.isWhite != king.isWhite)
                return true;
        }
        return false;
    }

    private bool findKing(bool[,] moves, int x, int y)
    //returns true if king coordinates are found in that pieces possibles move set
    {
        for(int i = 0; i< 8; ++i)
            for(int j = 0; j<8; ++j)
                if(moves[i,j] && i == x && j == y)
                    return true;
        return false;

    }

    public bool validateMove(int x, int y, ChessPiece k, ChessPiece piece)
    //checks a possible move to see if it will put same teams king in check
    {
        int prevX = piece.CurrentX;
        int prevY = piece.CurrentY;
        ChessPiece c = null;
        bool result = true;
        if(Chesspieces[x,y] != null)
            c = Chesspieces[x,y];
        Chesspieces[piece.CurrentX,piece.CurrentY] = null;
        piece.SetPosition(x,y);
        Chesspieces[x,y] = piece;

        result = !inCheck(k);

        Chesspieces[piece.CurrentX,piece.CurrentY] = null;
        piece.SetPosition(prevX,prevY);
        Chesspieces[prevX,prevY] = piece;
        if(c != null)
            Chesspieces[x,y] = c;
        return result;
    }

    private void UpdateSelection()
    //scans hitpoint of mouse to see which tile is being hovered.
    {
        if(!Camera.main)
            return;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,25.0f,LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int) hit.point.x;
            selectionY = (int) hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void SpawnChessPieces(int index, int x, int y)
    //takes index in chess pieces and the spot they need to be on and spawns the correct piece
    //sets it as child to chessboard
    {
        Quaternion orientation = chessPiecesPrefabs[index].transform.rotation;
        GameObject go = Instantiate(chessPiecesPrefabs[index], getTileCenter(x,y), orientation, transform) as GameObject;
        Chesspieces [x,y] = go.GetComponent<ChessPiece>();
        Chesspieces [x,y].SetPosition(x,y);
        activeChessPieces.Add(go);
    }

    private Vector3 getTileCenter(int x, int y)
    //gets center of selected tile so that chess piece is centered.
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    private void spawnAllChessPieces()
    //spawns all chess pieces in correct place in center of tile.
    {
        activeChessPieces = new List<GameObject>();
        Chesspieces = new ChessPiece[8,8];
        EnPassantMove = new int[2]{-1,-1};
        //Spawn white
        {
            //King
            SpawnChessPieces(0,4,0);
            WhiteKing = Chesspieces[4,0];

            //Queen
            SpawnChessPieces(1,3,0);

            //Rook
            SpawnChessPieces(2,0,0);
            SpawnChessPieces(2,7,0);
            
            //Knight
            SpawnChessPieces(3,1,0);
            SpawnChessPieces(3,6,0);
            
            //Bishop
            SpawnChessPieces(4,2,0);
            SpawnChessPieces(4,5,0);
            
            //Pawn
            for(int i = 0; i<8;++i){
                SpawnChessPieces(5,i,1);
            }
        }

        //Spawn Black
        {
            //King
            SpawnChessPieces(6,4,7);
            BlackKing = Chesspieces[4,7];

            //Queen
            SpawnChessPieces(7,3,7);

            //Rook
            SpawnChessPieces(8,0,7);
            SpawnChessPieces(8,7,7);
            
            //Knight
            SpawnChessPieces(9,1,7);
            SpawnChessPieces(9,6,7);
            
            //Bishop
            SpawnChessPieces(10,2,7);
            SpawnChessPieces(10,5,7);
            
            //Pawn
            for(int i = 0; i<8;++i){
                SpawnChessPieces(11,i,6);
            }
        }

    }





    private void DrawChessboard(){
    ///Draws the chessboard using vectors and debug.drawline
        Vector3 width = Vector3.right * 8;
        Vector3 height = Vector3.forward * 8;

        for(int i = 0; i<=8; ++i)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + width);
            for(int j = 0; j<=8; ++j)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + height);
            }
        }

        //draw an X on the selected tile
        if(selectionX >= 0 && selectionY >=0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY+1) + Vector3.right * (selectionX+1));

            Debug.DrawLine(
                Vector3.forward * (selectionY+1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX+1));
        }
    }
}
