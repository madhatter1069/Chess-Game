using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override bool[,] PossibleMove()
    {
        bool [,] moves = new bool[8,8];
        
        //UUR
        KnightMove(CurrentX+1, CurrentY+2, ref moves);
        //UUL
        KnightMove(CurrentX-1, CurrentY+2, ref moves);
        //URR
        KnightMove(CurrentX+2, CurrentY+1, ref moves);
        //ULL
        KnightMove(CurrentX-2, CurrentY+1, ref moves);
        //DDR
        KnightMove(CurrentX+1, CurrentY-2, ref moves);
        //DDL
        KnightMove(CurrentX-1, CurrentY-2, ref moves);
        //DRR
        KnightMove(CurrentX+2, CurrentY-1, ref moves);
        //DLL
        KnightMove(CurrentX-2, CurrentY-1, ref moves);
        
        return moves;
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
