using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Aspirational")]
public class Aspirational : Negamax_AB
{
    public override string AlgorithmName
    {
        get { return this.name; }
    }

    [SerializeField] private int windowRange;
    private int previousScore = 0;


    public override Vector2Int DecideMove(int[,] board, int player)
    {
        //return AspirationalSearch(board, player);
        return Vector2Int.zero;
    }

    private Vector2Int AspirationalSearch(int[,] board, int player)
    {
        int alpha, beta;
        Vector2Int move;

        if (previousScore != 0)
        {
            alpha = previousScore - windowRange;
            beta = previousScore + windowRange;

            while (true)
            {
                move = NegamaxAB_Algorithm(board, player, depth, alpha, beta);

                if (move.x <= alpha) { alpha = int.MinValue; }

                else if (move.x >= beta) { beta = int.MaxValue; }

                else { break; }
            }

            previousScore = move.x;
        }
        else
        {
            move = NegamaxAB_Algorithm(board, player, depth, int.MinValue, int.MaxValue);
            previousScore = move.x;
        }

        return move;
    }
}
