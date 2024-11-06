using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Aspirational")]
public class Aspirational : Negamax_AB
{
    public override string AlgorithmName
    {
        get { return this.name; }
    }

    // Rango de la ventana de búsqueda aspiracional
    [SerializeField] private int windowRange;
    private int previousScore = 0;


    public override Vector2Int DecideMove(int[,] board, int player)
    {
        // Llama al método de búsqueda aspiracional
        return AspirationalSearch(board, player);
    }

    private Vector2Int AspirationalSearch(int[,] board, int player)
    {
        int alpha, beta;
        Vector2Int move;

        // Si existe un puntaje previo, ajusta la ventana inicial basándose en este puntaje y el rango de ventana.
        if (previousScore != 0)
        {
            alpha = previousScore - windowRange;
            beta = previousScore + windowRange;

            // Ciclo de búsqueda que ajusta los límites de alpha y beta si el puntaje calculado está fuera de la ventana.
            while (true)
            {
                // Llama al algoritmo Negamax con poda Alpha-Beta
                move = NegamaxAB_Algorithm(board, player, depth, alpha, beta);

                if (move.x <= alpha) { alpha = int.MinValue; }

                else if (move.x >= beta) { beta = int.MaxValue; }

                // Si el puntaje está dentro de la ventana, se detiene la búsqueda.
                else { break; }
            }

            previousScore = move.x;
        }
        else
        {
            // Si no existe un puntaje previo, realiza una búsqueda completa usando los límites iniciales de alpha y beta.
            move = NegamaxAB_Algorithm(board, player, depth, int.MinValue, int.MaxValue);
            previousScore = move.x;
        }

        return move;
    }
}
