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
    [SerializeField] protected bool useHash = false;

    private Dictionary<int, BoardRecord> transpositionTable = new Dictionary<int, BoardRecord>();
    private ZobristHashing zobrist = new ZobristHashing();

    public override Vector2Int DecideMove(int[,] board, int player)
    {
        // Calcular el valor de hash inicial del tablero
        int initialHash = useHash ? zobrist.CalculateHash(board, player) : 0;

        Vector2Int bestMove = NegamaxAB_Algorithm(board, player, depth, int.MinValue, int.MaxValue, initialHash);

        return bestMove;
    }

    protected Vector2Int NegamaxAB_Algorithm(int[,] board, int player, int depth, int alpha, int beta, int hash)
    {
        int bestMove = -1;
        int bestScore = int.MinValue;

        if(useHash)
        {
            // Verificar si el estado actual está en la tabla de transposición
            if (transpositionTable.TryGetValue(hash, out BoardRecord record) && record.depth >= depth)
            {
                if(record.scoreType == ScoreType.Exact) { return new Vector2Int(record.score, -1); }

                else if(record.scoreType == ScoreType.LowerBound && record.score > alpha) { alpha = record.score; }

                else if(record.scoreType == ScoreType.UpperBound && record.score < beta)  { beta  = record.score; }

                
                if(alpha >= beta) { return new Vector2Int(record.score, -1); }
            }
        }

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

            // Actualizar el hash usando la clave Zobrist para el movimiento actual
            int newHash = useHash ? zobrist.UpdateHash(hash, (int)pos.x, (int)pos.y, player) : 0;

            Vector2Int scoringMove = NegamaxAB_Algorithm(newBoard, ChangeTurn(player), depth - 1, -beta, -Mathf.Max(alpha, bestScore), newHash);

            int currentScore = -scoringMove.x;

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestMove = (int)pos.x;
            }

            if (bestScore >= beta)
            {
                if (useHash) { SaveToTranspositionTable(hash, bestScore, depth, ScoreType.LowerBound); }

                return new Vector2Int(bestScore, bestMove);
            }

            alpha = Mathf.Max(alpha, bestScore);
        }

        if (useHash) { SaveToTranspositionTable(hash, bestScore, depth, alpha > bestScore ? ScoreType.UpperBound : ScoreType.Exact); }

        return new Vector2Int(bestScore, bestMove);
    }

    private int Move_Ordering(int[,] board, int x, int y, int player)
    {
        int[,] newBoard = (int[,])board.Clone();

        newBoard[x, y] = player;

        return Evaluate(newBoard, player);
    }

    private void SaveToTranspositionTable(int hash, int score, int depth, ScoreType scoreType)
    {
        transpositionTable[hash] = new BoardRecord { score = score, depth = depth, scoreType = scoreType };
    }
}