using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    public override bool[,] PossibleMove()
    {
        bool [,] moves = new bool[8,8];
        ChessPiece c;
        int i;
        int j;

        //up right
        i = CurrentX;
        j = CurrentY;
        while(true)
        {
            i++;
            j++;
            if (i>= 8 || j >= 8)
                break;
            c = BoardManager.Instance.Chesspieces[i,j];
            if(c == null)
                moves[i,j] = BoardManager.Instance.validateMove(i, j, king, this);
            else   
            {
                if(c.isWhite != isWhite)
                    moves[i,j] = BoardManager.Instance.validateMove(i, j, king, this);

                break;
            }
        }
        //up left
        i = CurrentX;
        j = CurrentY;
        while(true)
        {
            i--;
            j++;
            if (i < 0 || j >= 8)
                break;
            c = BoardManager.Instance.Chesspieces[i,j];
            if(c == null)
                moves[i,j] = BoardManager.Instance.validateMove(i, j, king, this);
            else   
            {
                if(c.isWhite != isWhite)
                    moves[i,j] = BoardManager.Instance.validateMove(i, j, king, this);
                    
                break;
            }
        }
        //down left
        i = CurrentX;
        j = CurrentY;
        while(true)
        {
            i--;
            j--;
            if (i < 0 || j < 0)
                break;
            c = BoardManager.Instance.Chesspieces[i,j];
            if(c == null)
                moves[i,j] = BoardManager.Instance.validateMove(i, j, king, this);
            else   
            {
                if(c.isWhite != isWhite)
                    moves[i,j] = BoardManager.Instance.validateMove(i, j, king, this);
                    
                break;
            }
        }
        //down right
        i = CurrentX;
        j = CurrentY;
        while(true)
        {
            i++;
            j--;
            if (i >= 8 || j < 0)
                break;
            c = BoardManager.Instance.Chesspieces[i,j];
            if(c == null)
                moves[i,j] = BoardManager.Instance.validateMove(i, j, king, this);
            else   
            {
                if(c.isWhite != isWhite)
                    moves[i,j] = BoardManager.Instance.validateMove(i, j, king, this);
                    
                break;
            }
        }
        return moves;
    }
}
