
using UnityEngine;

public abstract class Algorithm : ScriptableObject
{
    [SerializeField] string algorithmName;
    public string AlgorithmName { get { return algorithmName; } }
    public abstract Vector2Int DecideMove(BoardSpawner board, int player);

    public abstract int ChangeTurn(int actualPlayer);
}
