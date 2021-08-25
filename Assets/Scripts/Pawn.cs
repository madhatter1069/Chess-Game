using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{

    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[8,8];
        ChessPiece c, c2;
        int [] e = BoardManager.Instance.EnPassantMove;
        //white team move
        if(isWhite)
        {
            //diagonal left
            if(CurrentX != 0 && CurrentY != 7)
            {
                if(e[0] == CurrentX-1 &&  e[1] == CurrentY+1)
                    moves[CurrentX-1, CurrentY+1] = BoardManager.Instance.validateMove(CurrentX-1, CurrentY+1,king, this);
                c = BoardManager.Instance.Chesspieces[CurrentX-1, CurrentY+1];
                if(c != null && !c.isWhite)
                {
                    moves[CurrentX-1, CurrentY+1] = BoardManager.Instance.validateMove(CurrentX-1, CurrentY+1,king,this);
                }
            }

            //diagonal right
            if(CurrentX != 7 && CurrentY != 7)
            {
                if(e[0] == CurrentX+1 &&  e[1] == CurrentY+1)
                    moves[CurrentX+1, CurrentY+1] = BoardManager.Instance.validateMove(CurrentX+1, CurrentY+1,king,this);
                c = BoardManager.Instance.Chesspieces[CurrentX+1, CurrentY+1];
                if(c != null && !c.isWhite)
                {
                    moves[CurrentX+1, CurrentY+1] = BoardManager.Instance.validateMove(CurrentX+1, CurrentY+1,king,this);
                }
            }

            //middle
            if(CurrentY != 7)
            {
                c = BoardManager.Instance.Chesspieces[CurrentX,CurrentY+1];
                if(c == null)
                {
                    moves[CurrentX,CurrentY+1] = BoardManager.Instance.validateMove(CurrentX, CurrentY+1,king,this);
                }
            }

            //middle first move
            if(CurrentY == 1)
            {
                c = BoardManager.Instance.Chesspieces[CurrentX,CurrentY+1];
                c2 = BoardManager.Instance.Chesspieces[CurrentX,CurrentY+2];
                if(c == null && c2 == null)
                {
                    moves[CurrentX,CurrentY+2] = BoardManager.Instance.validateMove(CurrentX, CurrentY+2,king,this);
                }
            }
        }
        //black team move
        else 
        {
            //diagonal left
            if(CurrentX != 0 && CurrentY != 0)
            {
                if(e[0] == CurrentX-1 &&  e[1] == CurrentY-1)
                    moves[CurrentX-1, CurrentY-1] = BoardManager.Instance.validateMove(CurrentX-1, CurrentY-1,king, this);
                c = BoardManager.Instance.Chesspieces[CurrentX-1, CurrentY-1];
                if(c != null && c.isWhite)
                {
                    moves[CurrentX-1, CurrentY-1] = BoardManager.Instance.validateMove(CurrentX-1, CurrentY-1,king, this);
                }
            }

            //diagonal right
            if(CurrentX != 7 && CurrentY != 0)
            {
                if(e[0] == CurrentX+1 &&  e[1] == CurrentY-1)
                    moves[CurrentX+1, CurrentY-1] = BoardManager.Instance.validateMove(CurrentX+1, CurrentY-1,king,this);
                c = BoardManager.Instance.Chesspieces[CurrentX+1, CurrentY-1];
                if(c != null && c.isWhite)
                {
                    moves[CurrentX+1, CurrentY-1] = BoardManager.Instance.validateMove(CurrentX+1, CurrentY-1,king,this);
                }
            }

            //middle
            if(CurrentY != 0)
            {
                c = BoardManager.Instance.Chesspieces[CurrentX,CurrentY-1];
                if(c == null)
                {
                    moves[CurrentX,CurrentY-1] = BoardManager.Instance.validateMove(CurrentX, CurrentY-1,king,this);
                }
            }

            //middle first move
            if(CurrentY == 6)
            {
                c = BoardManager.Instance.Chesspieces[CurrentX,CurrentY-1];
                c2 = BoardManager.Instance.Chesspieces[CurrentX,CurrentY-2];
                if(c == null && c2 == null)
                {
                    moves[CurrentX,CurrentY-2] = BoardManager.Instance.validateMove(CurrentX, CurrentY-2,king,this);
                }
            }
        }
        return moves;
    }
}
