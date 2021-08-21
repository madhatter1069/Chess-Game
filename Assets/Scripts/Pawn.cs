using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override bool[,] PossibleMove()
    {
        bool[,] r= new bool[8,8];
        r[3,3] = true;
        return r;
    }
}
