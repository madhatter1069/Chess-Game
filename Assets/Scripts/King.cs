using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override bool[,] PossibleMove()
    {
        bool [,] moves = new bool[8,8];
        ChessPiece c;
        int i, j;

        //top side
        i = CurrentX -1;
        j = CurrentY +1;
        if(CurrentY != 7)
        {
            for(int k = 0; k < 3; ++k)
            {
                if(i >= 0 && i < 8)
                {
                    c = BoardManager.Instance.Chesspieces[i,j];
                    if(c == null)
                        moves[i,j] = BoardManager.Instance.validateMove(i, j, this, this);
                    else 
                    {
                        if(c.isWhite != isWhite)
                            moves[i,j] = BoardManager.Instance.validateMove(i, j, this, this);
                    }
                    
                }
                ++i;
            }
        }
        //down side
        i = CurrentX -1;
        j = CurrentY -1;
        if(CurrentY != 0)
        {
            for(int k = 0; k < 3; ++k)
            {
                if(i >= 0 && i < 8)
                {
                    c = BoardManager.Instance.Chesspieces[i,j];
                    if(c == null)
                        moves[i,j] = BoardManager.Instance.validateMove(i, j, this, this);
                    else 
                    {
                        if(c.isWhite != isWhite)
                            moves[i,j] = BoardManager.Instance.validateMove(i, j, this, this);
                    }
                }
                ++i;
            }
        }
        //middle left
        if(CurrentX != 0)
        {
            c = BoardManager.Instance.Chesspieces[CurrentX-1,CurrentY];
            if(c == null)
                moves[CurrentX-1,CurrentY] = BoardManager.Instance.validateMove(CurrentX-1, CurrentY, this, this);
            else 
            {
                if(c.isWhite != isWhite)
                    moves[CurrentX-1,CurrentY] = BoardManager.Instance.validateMove(CurrentX-1, CurrentY, this, this);
            }
        }
        //middle right
        if(CurrentX != 7)
        {
            c = BoardManager.Instance.Chesspieces[CurrentX+1,CurrentY];
            if(c == null)
                moves[CurrentX+1,CurrentY] = BoardManager.Instance.validateMove(CurrentX+1, CurrentY, this, this);
            else 
            {
                if(c.isWhite != isWhite)
                    moves[CurrentX+1,CurrentY] = BoardManager.Instance.validateMove(CurrentX+1, CurrentY, this, this);
            }
        }

        if(castle)
        {
            CastleMove(CurrentX,CurrentY, ref moves);
        }

        return moves;
    }

    public void CastleMove(int x, int y, ref bool[,] moves)
    {
        ChessPiece c;
        int i;

        //castle right
        i = x;
        while (i < 8)
        {
            ++i;
            c = BoardManager.Instance.Chesspieces[i,y];
            if(c == null){}
            else if(c != null)
            {
                if(i == 7 && c.isWhite == isWhite && c.GetType() == typeof(Rook) && c.castle)
                {
                    if(!BoardManager.Instance.inCheck(this))
                    {
                        if(moves[CurrentX+1,CurrentY])
                            moves[x+2,y] = BoardManager.Instance.validateMove(x+2,y,this,this);
                    }
                }
                break;
            }
                
        }
        //castle left
        i = x;
        while (i > -1)
        {
            --i;
            c = BoardManager.Instance.Chesspieces[i,y];
            if(c == null){}
            else if(c != null)
            {
                if(i == 0 && c.isWhite == isWhite && c.GetType() == typeof(Rook) && c.castle)
                {
                    if(!BoardManager.Instance.inCheck(this))
                    {
                        if(BoardManager.Instance.validateMove(x-1,y,this,this))
                            moves[x-2,y] = BoardManager.Instance.validateMove(x-2,y,this,this);
                    }
                }
                break;
            }
                
        }

    }

}
