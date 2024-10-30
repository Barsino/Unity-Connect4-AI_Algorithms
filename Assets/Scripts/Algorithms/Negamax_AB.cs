using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Negamax_AB")]
public class Negamax_AB : Algorithm
{
    public override string AlgorithmName
    {
        get { return this.name; }
    }

    [SerializeField] protected int depth;
    
    public override Vector2Int DecideMove(int[,] board, int player)
    {
        Vector2Int bestMove = NegamaxAB_Algorithm(board, player, depth, int.MinValue, int.MaxValue);

        return bestMove;
    }

    protected Vector2Int NegamaxAB_Algorithm(int[,] board, int player, int depth, int alpha, int beta)
    {
        int bestMove = -1;
        int bestScore = int.MinValue;

        if (depth == 0 || IsEndOfGame(board, player))
        {
            int score = Evaluate(board, player);
            return new Vector2Int(score, -1);
        }

        List<Vector2> validPos = GetValidPos(board);

        // Ordenar los movimientos válidos utilizando la función Evaluate()
        validPos.Sort((pos1, pos2) => Move_Ordering(board, (int)pos2.x, (int)pos2.y, player).CompareTo(
                                      Move_Ordering(board, (int)pos1.x, (int)pos1.y, player)));

        foreach (Vector2 pos in validPos)
        {
            int[,] newBoard = (int[,])board.Clone();
            newBoard[(int)pos.x, (int)pos.y] = player;

            Vector2Int scoringMove = NegamaxAB_Algorithm(newBoard, ChangeTurn(player), depth - 1, -beta, -Mathf.Max(alpha, bestScore));

            int currentScore = -scoringMove.x;

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestMove = (int)pos.x;
            }

            if (bestScore >= beta)
            {
                return new Vector2Int(bestScore, bestMove);
            }

            alpha = Mathf.Max(alpha, bestScore);
        }

        return new Vector2Int(bestScore, bestMove);
    }

    private int Move_Ordering(int[,] board, int x, int y, int player)
    {
        int[,] newBoard = (int[,])board.Clone();

        newBoard[x, y] = player;

        return Evaluate(newBoard, player);
    }
}