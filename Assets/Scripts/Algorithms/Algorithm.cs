
using UnityEngine;

public abstract class Algorithm : ScriptableObject
{
    [SerializeField] string algorithmName;
    [SerializeField] int turn;
    public int Turn { get { return turn; } }
    public string AlgorithmName { get { return algorithmName; } }
    public abstract Vector2Int DecideMove(BoardSpawner board, int player);

    public abstract int ChangeTurn(int actualPlayer);
}
