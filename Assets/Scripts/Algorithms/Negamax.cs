using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Negamax")]
public class Negamax : Algorithm
{
    public override string AlgorithmName
    {
        get { return this.name; }
    }

    [SerializeField] private int depth;

    public override Vector2Int DecideMove(BoardSpawner board, int player)
    {
        return NegamaxAlgorithm(board, depth, player);
    }

    private Vector2Int NegamaxAlgorithm(BoardSpawner board, int depth, int player)
    {
        byte bestMove = 0;
        int bestScore = int.MinValue;
        int currentScore;

        //if(depth == 0)
        return Vector2Int.zero;
    }
}
