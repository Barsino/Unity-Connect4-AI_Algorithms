using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZobristKey31Bits
{
    private int[,] keys;
    private int boardPositions, numberOfPieces;

    public ZobristKey31Bits(int _boardPositions, int _numberOfPieces)
    {
        System.Random rdn = new System.Random();

        boardPositions = _boardPositions;
        numberOfPieces = _numberOfPieces;

        keys = new int[boardPositions, numberOfPieces];

        for(int i = 0; i < boardPositions; i++)
        {
            for(int j = 0; j < numberOfPieces; j++)
            {
                keys[i, j] = rdn.Next(int.MaxValue);
            }
        }
    }

    public int GetKeys (int position, int piece)
    {
        return keys[position, piece];
    }
}
