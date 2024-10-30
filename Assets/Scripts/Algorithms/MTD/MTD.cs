using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MTD")]
public class MTD : Algorithm
{
    public override string AlgorithmName
    {
        get { return this.name; }
    }

    private int hashTableLength = 9000000;
    private int globalGuess = int.MaxValue;
    [SerializeField] private int max_Iterations = 10;
    private TranspositionTable transpositionTable;

    private void Awake()
    {
        transpositionTable = new TranspositionTable(hashTableLength);
    }

    public override Vector2Int DecideMove(int[,] board, int player)
    {
        return Vector2Int.zero;
    }

    private Vector2Int MTD_Algorithm()
    {
        int gamma, guess = globalGuess;
        Vector2Int bestMove = new Vector2Int(0, 0);

        for(int i = 0; i < max_Iterations; i++)
        {
            gamma = guess;
            // Test
        }
        return Vector2Int.zero;
    }

    private Vector2Int Test(int[,] board, int depth, int gamma, int player)
    {
        int bestMove, bestScore;
        Vector2Int scoringMove;
        BoardRecord record;

        // Buscar si ya tenemos un registro del tablero en la tabla de trasposición.
        // TODO -> check implementation
        // record = transpositionTable.GetRecord (board.hashValue);
        record = transpositionTable.GetRecord(0);

        // Si hay registro de este tablero
        if(record != null)
        {
            // Profundidad adecuada
            if(record.depth > depth - 1)
            {
                // Si el score se ajusta al valor gamma que arrastramos, entonces devolvemos la jugada adecuada.
                if(record.minScore > gamma)
                {
                    scoringMove = new Vector2Int((int)record.minScore, (int)record.bestMove);
                    return scoringMove;
                }

                if(record.maxScore < gamma)
                {
                    scoringMove = new Vector2Int((int)record.maxScore, (int)record.bestMove);
                    return scoringMove;
                }
            }
        }
        // No hay registro. Se inicializa el tablero
        else
        {
            record = new BoardRecord();
            //record.hashValue = board.hashValue;
            record.depth = depth;
            record.minScore = int.MinValue;
            record.maxScore = int.MaxValue;
        }

        // Buscamos jugada
        if(depth == 0 || IsEndOfGame(board, player))
        {
            record.maxScore = Evaluate(board, player);
            record.minScore = record.maxScore;
            transpositionTable.SaveRecord(record);

            return new Vector2Int(record.maxScore, -1);
        }
        // No es estado final o suspension
        else
        {
            bestMove = 0;
            bestScore = int.MinValue;
            List<Vector2> validPos = GetValidPos(board);
            
            foreach(Vector2 pos in validPos)
            {
                int[,] newBoard = (int[,])board.Clone();
                newBoard[(int)pos.x, (int)pos.y] = player;

                // Recursividad
                scoringMove = Test(newBoard, depth - 1, -gamma, ChangeTurn(player));

                int invertedScore = -scoringMove.x;

                // Actualizar mejor score
                if(invertedScore > bestScore)
                {
                    record.bestMove = bestMove;
                    bestScore = invertedScore;
                }

                if(bestScore < gamma)
                {
                    record.maxScore = bestScore;
                }
                else
                {
                    record.minScore = bestScore;
                }
            }

            transpositionTable.SaveRecord(record);
            scoringMove = new Vector2Int((int)bestMove, (int)bestScore);
        }

        return scoringMove;
    }
}
