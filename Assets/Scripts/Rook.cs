using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public override bool[,] PossibleMove()
    {
        bool [,] moves = new bool[8,8];
        ChessPiece c;
        int i; 

        //right
        i = CurrentX;
        while(true)
        {
            i++;
            if(i >= 8)
                break;
            c = BoardManager.Instance.Chesspieces[i, CurrentY];
            if(c == null)
                moves[i,CurrentY] = BoardManager.Instance.validateMove(i, CurrentY,king,this);
            else 
            {
                if(c.isWhite != isWhite)
                    moves[i,CurrentY] = BoardManager.Instance.validateMove(i, CurrentY,king,this);
                
                break;
            }    
        }

        //left
        i = CurrentX;
        while(true)
        {
            i--;
            if(i < 0)
                break;
            c = BoardManager.Instance.Chesspieces[i, CurrentY];
            if(c == null)
                moves[i,CurrentY] = BoardManager.Instance.validateMove(i, CurrentY,king, this);
            else 
            {
                if(c.isWhite != isWhite)
                    moves[i,CurrentY] = BoardManager.Instance.validateMove(i, CurrentY, king, this);
                
                break;
            }    
        }

        //up
        i = CurrentY;
        while(true)
        {
            i++;
            if(i >= 8)
                break;
            c = BoardManager.Instance.Chesspieces[CurrentX, i];
            if(c == null)
                moves[CurrentX,i] = BoardManager.Instance.validateMove(CurrentX, i, king,this);
            else 
            {
                if(c.isWhite != isWhite)
                    moves[CurrentX,i] = BoardManager.Instance.validateMove(CurrentX, i, king,this);
                
                break;
            }    
        }

        //down
        i = CurrentY;
        while(true)
        {
            i--;
            if(i < 0)
                break;
            c = BoardManager.Instance.Chesspieces[CurrentX, i];
            if(c == null)
                moves[CurrentX,i] = BoardManager.Instance.validateMove(CurrentX, i, king,this);
            else 
            {
                if(c.isWhite != isWhite)
                    moves[CurrentX,i] = BoardManager.Instance.validateMove(CurrentX, i, king,this);
                
                break;
            }    
        }
        return moves;
    }



}
