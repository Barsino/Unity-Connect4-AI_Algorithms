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

    // Profundidad máxima de búsqueda
    [SerializeField] private int maxDepth = 6;

    // Número máximo de iteraciones en la búsqueda MTD
    [SerializeField] private int max_Iterations = 5;

    private int globalGuess = int.MaxValue;

    // Tabla de transposición y claves de Zobrist
    private TranspositionTable transpositionTable;
    private ZobristKey31Bits zobristKey;

    // Longitud de la tabla hash de transposición
    private int hashTableLength = 90000000;
    private int hash;

    private void OnEnable()
    {
        // Inicializa la clave Zobrist para el hash y la tabla de transposición
        zobristKey = new ZobristKey31Bits(42, 2);
        transpositionTable = new TranspositionTable(hashTableLength);

        if (zobristKey == null || transpositionTable == null)
        {
            Debug.LogError("Error en la inicialización de zobristKey o transpositionTable");
        }
    }

    public override Vector2Int DecideMove(int[,] board, int player)
    {
        // Calcula el hash del tablero para el jugador actual
        hash = CalculateHash(board, player);

        // Llama al algoritmo MTD
        Vector2Int bestMove = MTD_Algorithm(board, maxDepth, globalGuess, player);

        return bestMove;
    }

    private Vector2Int MTD_Algorithm(int[,] board, int _depth, int initialGuess, int player)
    {
        int guess = initialGuess;
        Vector2Int bestMove = new Vector2Int(0, 0);

        // Realiza iteraciones para ajustar guess
        for (int i = 0; i < max_Iterations; i++)
        {
            int gamma = guess;

            // Ejecuta una prueba con guess ajustado
            bestMove = Test(board, _depth, gamma - 1, player);

            guess = bestMove.x;

            if(gamma == guess)
            {
                break;
            }
        }

        // Actualiza la globalGuess para el siguiente movimiento
        globalGuess = guess;
        return bestMove;
    }

    private Vector2Int Test(int[,] board, int depth, int gamma, int player)
    {
        int bestMove, bestScore;
        Vector2Int scoringMove;
        BoardRecord record;

        // Intenta obtener el registro de la tabla de transposición para el hash actual
        record = transpositionTable.GetRecord(hash);

        // Si hay registro de este tablero
        if (record != null)
        {
            // Profundidad adecuada
            if (record.depth > depth - 1)
            {
                // Si el score se ajusta al valor gamma que arrastramos, entonces devolvemos la jugada adecuada.
                if (record.minScore > gamma)
                {
                    scoringMove = new Vector2Int((int)record.minScore, (int)record.bestMove);
                    return scoringMove;
                }

                if (record.maxScore < gamma)
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
            record.hashValue = hash;
            record.depth = depth;
            record.minScore = int.MinValue;
            record.maxScore = int.MaxValue;
        }

        // Caso base: si se alcanza la profundidad o se llega a un estado final del juego
        if (depth == 0 || IsEndOfGame(board, player))
        {
            record.maxScore = Evaluate(board, player);
            record.minScore = record.maxScore;
            transpositionTable.SaveRecord(record);

            return new Vector2Int(record.maxScore, -1);
        }

        // Caso recursivo: si no es un estado final o se alcanza la profundidad máxima
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
                    bestMove = (int)pos.x;
                }

                // Actualiza los límites de la puntuación
                if (bestScore < gamma)
                {
                    record.maxScore = bestScore;
                }
                else
                {
                    record.minScore = bestScore;
                }
            }

            // Guarda el registro en la tabla de transposición
            transpositionTable.SaveRecord(record);
            scoringMove = new Vector2Int(bestScore, bestMove);
        }

        return scoringMove;
    }

    private int CalculateHash(int[,] board, int player)
    {
        int hashValue = 0;
        int columns = board.GetLength(0);
        int rows = board.GetLength(1);

        if (zobristKey == null)
        {
            Debug.LogError("zobristKey no está inicializado");
            return 0; // Devuelve un valor por defecto o lanza una excepción personalizada si es necesario.
        }

        // Recorre el tablero y ajusta el hash según las piezas en cada posición
        for (int column = 0; column < columns; column++)
        {
            for (int row = 0; row < rows; row++)
            {
                if (board[column, row] != 0)
                {
                    int piece = board[column, row] == player ? 1 : 0;
                    int position = column * rows + row;

                    Debug.Log($"Position: {position}, Piece: {piece}");
                    if (position >= zobristKey.BoardPositions || piece >= zobristKey.NumberOfPieces)
                    {
                        Debug.LogError("Índice fuera de los límites en ZobristKey");
                        continue; // o maneja el caso según lo necesites
                    }

                    // Aplica la clave de Zobrist para obtener el hash del tablero
                    hashValue ^= zobristKey.GetKeys(position, piece);
                }
            }
        }
        return hashValue;
    }
}
