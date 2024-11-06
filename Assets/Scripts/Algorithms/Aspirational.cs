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

    // Rango de la ventana de b�squeda aspiracional
    [SerializeField] private int windowRange;
    private int previousScore = 0;


    public override Vector2Int DecideMove(int[,] board, int player)
    {
        // Llama al m�todo de b�squeda aspiracional
        return AspirationalSearch(board, player);
    }

    private Vector2Int AspirationalSearch(int[,] board, int player)
    {
        int alpha, beta;
        Vector2Int move;

        // Si existe un puntaje previo, ajusta la ventana inicial bas�ndose en este puntaje y el rango de ventana.
        if (previousScore != 0)
        {
            alpha = previousScore - windowRange;
            beta = previousScore + windowRange;

            // Ciclo de b�squeda que ajusta los l�mites de alpha y beta si el puntaje calculado est� fuera de la ventana.
            while (true)
            {
                // Llama al algoritmo Negamax con poda Alpha-Beta
                move = NegamaxAB_Algorithm(board, player, depth, alpha, beta);

                if (move.x <= alpha) { alpha = int.MinValue; }

                else if (move.x >= beta) { beta = int.MaxValue; }

                // Si el puntaje est� dentro de la ventana, se detiene la b�squeda.
                else { break; }
            }

            previousScore = move.x;
        }
        else
        {
            // Si no existe un puntaje previo, realiza una b�squeda completa usando los l�mites iniciales de alpha y beta.
            move = NegamaxAB_Algorithm(board, player, depth, int.MinValue, int.MaxValue);
            previousScore = move.x;
        }

        return move;
    }
}
