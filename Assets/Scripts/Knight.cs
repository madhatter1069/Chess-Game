using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override bool[,] PossibleMove()
    {
        bool [,] r = new bool[8,8];
        
        //UUR
        KnightMove(CurrentX+1, CurrentY+2, ref r);
        //UUL
        KnightMove(CurrentX-1, CurrentY+2, ref r);
        //URR
        KnightMove(CurrentX+2, CurrentY+1, ref r);
        //ULL
        KnightMove(CurrentX-2, CurrentY+1, ref r);
        //DDR
        KnightMove(CurrentX+1, CurrentY-2, ref r);
        //DDL
        KnightMove(CurrentX-1, CurrentY-2, ref r);
        //DRR
        KnightMove(CurrentX+2, CurrentY-1, ref r);
        //DLL
        KnightMove(CurrentX-2, CurrentY-1, ref r);
        
        return r;
    }

    public void KnightMove(int x, int y, ref bool[,] moves)
    {
        ChessPiece c;
        if(x >= 0 && x < 8 && y >=0 && y < 8)
        {
            c = BoardManager.Instance.Chesspieces[x,y];
            if(c == null)
                moves[x,y] = BoardManager.Instance.validateMove(x, y, king, this);
            else if(isWhite != c.isWhite)
                moves[x,y] = BoardManager.Instance.validateMove(x, y, king, this);
        }
    }


}
