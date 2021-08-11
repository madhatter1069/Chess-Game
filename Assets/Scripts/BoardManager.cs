using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public ChessPiece [,] Chesspieces{set;get;}
    private ChessPiece selectedChessPiece;
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessPiecesPrefabs;
    private List<GameObject> activeChessPieces;

    public bool isWhiteTurn = true;

    private void Start()
    {
        spawnAllChessPieces();
    }
    private void Update()
    {
        UpdateSelection();
        DrawChessboard();

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
    
    }

    private void SelectChesspiece(int x, int y)
    {
        if(Chesspieces[x,y] == null)
            return;
        
        if(Chesspieces[x,y].isWhite != isWhiteTurn)
            return;
        selectedChessPiece = Chesspieces[x,y];
    }

    private void MoveChesspiece(int x, int y)
    {
        if(selectedChessPiece.PossibleMove(x,y))
        {
            Chesspieces[selectedChessPiece.CurrentX,selectedChessPiece.CurrentY] = null;
            selectedChessPiece.transform.position = getTileCenter(x,y);
            Chesspieces[x,y] = selectedChessPiece;
            isWhiteTurn = !isWhiteTurn;
        }


        selectedChessPiece = null;
    }

    private void DrawChessboard()
    ///Draws the chessboard using vectors and debug.drawline
    {
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

    private void UpdateSelection()
    //scans hitpoint of mouse to see which tile is being hovered.
    {
        if(!Camera.main)
            return;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,25.0f,LayerMask.GetMask("ChessPlane")))
        {
            //Debug.Log(hit.point);
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
        //Spawn white
        {
            //King
            SpawnChessPieces(0,4,0);

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
}
