using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player")]
public class Player : Algorithm
{
    public override string AlgorithmName
    {
        get { return this.name; }
    }
    public override Vector2Int DecideMove(int[,] board, int player)
    {
        return Vector2Int.zero;
    }
}
