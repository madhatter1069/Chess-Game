using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{
    public override bool[,] PossibleMove()
    {
        bool [,] moves = new bool[8,8];
        ChessPiece c;
        int i,j;

        //right
        i = CurrentX;
        while(true)
        {
            i++;
            if(i >= 8)
                break;
            c = BoardManager.Instance.Chesspieces[i, CurrentY];
            if(c == null)
                moves[i,CurrentY] = true;
            else 
            {
                if(c.isWhite != isWhite)
                    moves[i,CurrentY] = true;
                
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
                moves[i,CurrentY] = true;
            else 
            {
                if(c.isWhite != isWhite)
                    moves[i,CurrentY] = true;
                
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
                moves[CurrentX,i] = true;
            else 
            {
                if(c.isWhite != isWhite)
                    moves[CurrentX,i] = true;
                
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
                moves[CurrentX,i] = true;
            else 
            {
                if(c.isWhite != isWhite)
                    moves[CurrentX,i] = true;
                
                break;
            }    
        }

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
                moves[i,j] = true;
            else   
            {
                if(c.isWhite != isWhite)
                    moves[i,j] = true;

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
                moves[i,j] = true;
            else   
            {
                if(c.isWhite != isWhite)
                    moves[i,j] = true;
                    
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
                moves[i,j] = true;
            else   
            {
                if(c.isWhite != isWhite)
                    moves[i,j] = true;
                    
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
                moves[i,j] = true;
            else   
            {
                if(c.isWhite != isWhite)
                    moves[i,j] = true;
                    
                break;
            }
        }
        
        return moves;
    }
}
