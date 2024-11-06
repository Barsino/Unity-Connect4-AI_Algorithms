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

    [SerializeField] private int simulations;

    public override Vector2Int DecideMove(int[,] board, int player)
    {
        return MontecarloAlgorithm(board, player);
    }

    private Vector2Int MontecarloAlgorithm(int[,] board, int player)
    {
        int bestMove = -1;
        int bestWinCount = -1;

        List<Vector2> validPos = GetValidPos(board);

        foreach(Vector2 pos in validPos)
        {
            int winCount = 0;

            for(int i = 0; i < simulations; i++)
            {
                int[,] newBoard = (int[,])board.Clone();
                newBoard[(int)pos.x, (int)pos.y] = player;

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

        while(!IsEndOfGame(board, player))
        {
            List<Vector2> moves = GetValidPos(board);
            Vector2 randomMove = moves[Random.Range(0, moves.Count)];
            board[(int)randomMove.x, (int)randomMove.y] = player;

            player = ChangeTurn(player);
        }

        if(CheckDraw(board))
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
