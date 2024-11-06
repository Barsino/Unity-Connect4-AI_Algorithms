using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Montecarlo")]
public class Montecarlo : Algorithm
{
    public override string AlgorithmName
    {
        get { return this.name; }
    }

    // Número de simulaciones a realizar.
    [SerializeField] private int simulations;

    public override Vector2Int DecideMove(int[,] board, int player)
    {
        // Llama al algoritmo de Monte Carlo
        return MontecarloAlgorithm(board, player);
    }

    private Vector2Int MontecarloAlgorithm(int[,] board, int player)
    {
        int bestMove = -1;
        int bestWinCount = -1;

        // Obtiene todas las posiciones válidas
        List<Vector2> validPos = GetValidPos(board);

        // Recorre todas las posiciones válidas
        foreach (Vector2 pos in validPos)
        {
            int winCount = 0;

            // Realiza varias simulaciones para cada movimiento.
            for (int i = 0; i < simulations; i++)
            {
                int[,] newBoard = (int[,])board.Clone();
                newBoard[(int)pos.x, (int)pos.y] = player;

                // Simula el juego con el movimiento aplicado.
                int result = SimulateGame(newBoard, ChangeTurn(player));

                if(result == player)
                {
                    winCount++;
                }
            }

            if(winCount > bestWinCount)
            {
                bestWinCount = winCount;
                bestMove = (int)pos.x;
            }
        }

        return new Vector2Int(bestWinCount, bestMove);
    }

    private int SimulateGame(int[,] board, int player)
    {
        int gameValue;

        // Continúa simulando hasta que se termine el juego
        while (!IsEndOfGame(board, player))
        {
            // Obtiene los movimientos válidos y selecciona uno aleatoriamente.
            List<Vector2> moves = GetValidPos(board);
            Vector2 randomMove = moves[Random.Range(0, moves.Count)];
            board[(int)randomMove.x, (int)randomMove.y] = player;

            player = ChangeTurn(player);
        }

        // Verifica el resultado del juego: empate o victoria
        if (CheckDraw(board))
        { 
            gameValue = 0; 
        }
        else if(CheckWin(board, player))
        {
            gameValue = player;
        }
        else
        {
            gameValue = player == 1 ? 2 : 1;
        }

        return gameValue;
    }
}
