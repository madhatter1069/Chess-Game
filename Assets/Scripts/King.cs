using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public bool firstMove = true;
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
                        moves[i,j] = true;
                    else 
                    {
                        if(c.isWhite != isWhite)
                            moves[i,j] = true;
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
                        moves[i,j] = true;
                    else 
                    {
                        if(c.isWhite != isWhite)
                            moves[i,j] = true;
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
                moves[CurrentX-1,CurrentY] = true;
            else 
            {
                if(c.isWhite != isWhite)
                    moves[CurrentX-1,CurrentY] = true;
            }
        }
        //middle right
        if(CurrentX != 7)
        {
            c = BoardManager.Instance.Chesspieces[CurrentX+1,CurrentY];
            if(c == null)
                moves[CurrentX+1,CurrentY] = true;
            else 
            {
                if(c.isWhite != isWhite)
                    moves[CurrentX+1,CurrentY] = true;
            }
        }

        return moves;
    }
}
