using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player")]
public class Player : Algorithm
{
    public override Vector2Int DecideMove(BoardSpawner board, int player)
    {
        return Vector2Int.zero;
    }

    public override int ChangeTurn(int actualPlayer)
    {
        return actualPlayer == 1 ? 2 : 1;
    }
}
