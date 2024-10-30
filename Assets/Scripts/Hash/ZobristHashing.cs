using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZobristHashing
{
    private int[,] zobristTable;
    private const int cols = 7;
    private const int rows = 6;
    private const int players = 2;

    public ZobristHashing()
    {
        zobristTable = new int[rows * cols, players];
        System.Random rand = new System.Random();

        for (int i = 0; i < rows * cols; i++)
        {
            for (int j = 0; j < players; j++)
            {
                zobristTable[i, j] = rand.Next(int.MinValue, int.MaxValue);
            }
        }
    }

    public int CalculateHash(int[,] board, int player)
    {
        int columns = board.GetLength(0);
        int rows    = board.GetLength(1);
        int hash = 0;
        int zobristKey;
        int piece, position;

        for (int column = 0; column < columns; column++)
        {
            for (int row = 0; row < rows; row++)
            {
                if (board[column, row] != 0)
                {
                    //int piece = board[x, y] - 1;
                    //hash ^= zobristTable[x * cols + y, piece];

                    position = column * columns + row; // Numero de la posicion
                    piece = board[column, row] - 1;

                    zobristKey = zobristTable[position, piece];

                    hash ^= zobristKey;// Aplicamos XOR al valor hash, con la clave Zobrist obtenida.
                }
            }
        }
        return hash;
    }

    public int UpdateHash(int hash, int column, int row, int player)
    {
        if(zobristTable != null)
        {
            if (column >= 0 && column < zobristTable.GetLength(0) &&
            row >= 0 && row < zobristTable.GetLength(1) &&
            player >= 0 && player < zobristTable.GetLength(2))
            {
                hash ^= zobristTable[column, row];
            }
            else
            {
                Debug.LogWarning("Indices fuera de rango en UpdateHash: " +
                                 $"column={column}, row={row}, player={player}");
            }
            int piece = player - 1;
            return hash ^= zobristTable[column * cols + row, piece];
        }

        else
        {
            Debug.Log("No");
            return 0;
        }
    }
}
