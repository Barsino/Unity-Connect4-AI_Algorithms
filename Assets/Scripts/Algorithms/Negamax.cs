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

    public override Vector2Int DecideMove(int[,] board, int player)
    {
        // Llama al algoritmo Negamax
        Vector2Int bestMove = NegamaxAlgorithm(board, depth, player);

        return bestMove;
    }

    private Vector2Int NegamaxAlgorithm(int[,] board, int depth, int player)
    {
        int bestMove = -1;
        int bestScore = int.MinValue;

        // Comprueba si se ha alcanzado la profundidad m�xima o el final del juego. Si es as�, eval�a el tablero actual.
        if (depth == 0 || IsEndOfGame(board, player))
        {
            int score = Evaluate(board, player);
            return new Vector2Int(score, -1);
        }

        // Obtiene todas las posiciones v�lidas
        List<Vector2> validPos = GetValidPos(board);

        // Recorre cada posici�n v�lida para evaluar su puntaje y determinar el mejor movimiento.
        foreach (Vector2 pos in validPos)
        {
            int[,] newBoard = (int[,])board.Clone();

            newBoard[(int)pos.x, (int)pos.y] = player;

            // Llama recursivamente al algoritmo
            Vector2Int scoringMove = NegamaxAlgorithm(newBoard, depth - 1, ChangeTurn(player));

            int currentScore = -scoringMove.x;

            if(currentScore > bestScore)
            {
                bestScore = currentScore;
                bestMove  = (int)pos.x;
            }
        }

        return new Vector2Int(bestScore, bestMove);
    }
}
