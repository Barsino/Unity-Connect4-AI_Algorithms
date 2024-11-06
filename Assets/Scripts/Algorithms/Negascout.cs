using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Negascout_AB")]
public class Negascout : Negamax_AB
{
    public override string AlgorithmName
    {
        get { return this.name; }
    }

    public override Vector2Int DecideMove(int[,] board, int player)
    {
        // Llama al método NegascoutAB_Algorithm
        return NegascoutAB_Algorithm(board, player, depth, 0, int.MinValue, int.MaxValue);
    }

    private Vector2Int NegascoutAB_Algorithm(int[,] board, int player, int depth, int currentDepth, int alpha, int beta)
    {
        int bestMove = -1;
        int bestScore = int.MinValue;
        int adaptiveBeta = beta;

        // Comprueba si se ha alcanzado la profundidad máxima o el final del juego. Si es así, evalúa el tablero actual.
        if (depth == 0 || IsEndOfGame(board, player))
        {
            int score = Evaluate(board, player);
            return new Vector2Int(score, -1);
        }

        // Obtiene todas las posiciones válidas
        List<Vector2> validpos = GetValidPos(board);

        // Itera sobre cada posición válida para evaluar su puntaje y determinar el mejor movimiento.
        foreach (Vector2 pos in validpos)
        {
            int[,] newBoard = (int[,])board.Clone();
            newBoard[(int)pos.x, (int)pos.y] = player;

            // Llamada recursiva
            Vector2Int recursedScore = NegamaxAB_Algorithm(newBoard, ChangeTurn(player), depth, -adaptiveBeta, -Mathf.Max(alpha, bestScore));
            int currentScore = -recursedScore.x;

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestMove = (int)pos.x;

                if (adaptiveBeta == beta || currentDepth >= depth - 2)
                {
                    bestScore = currentScore;
                }
                else
                {
                    // Llamada adicional para verificar si se puede ajustar el puntaje con un beta más alto
                    Vector2Int negativeBestScore = NegascoutAB_Algorithm(newBoard, ChangeTurn(player), depth, currentDepth + 1, -beta, -currentScore);
                }
            }

            if (bestScore >= beta)
            {
                return new Vector2Int(bestScore, bestMove);
            }

            // Ajusta adaptiveBeta para la próxima iteración, aumentando ligeramente por encima del valor de alpha.
            adaptiveBeta = Mathf.Max(alpha, bestScore) + 1;
        }

        return new Vector2Int(bestScore, bestMove);
    }
}
