using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Negamax")]

public class Negamax : Algorithm
{
    public override int ChangeTurn(int actualPlayer)
    {
        return 1;
    }

    public override Vector2Int DecideMove(BoardSpawner board, int player)
    {
        throw new System.NotImplementedException();
    }
}
