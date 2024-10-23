using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Negascout")]
public class Negascout : Algorithm
{
    public override string AlgorithmName
    {
        get { return this.name; }
    }

    // Depth

    public override Vector2Int DecideMove(int[,] board, int player)
    {
        throw new System.NotImplementedException();
    }

    private Vector2Int Negascout_Algorithm(int[,] board, int player, int depth, int currentDepth, int alpha, int beta)
    {
        return Vector2Int.zero;
    }
}
