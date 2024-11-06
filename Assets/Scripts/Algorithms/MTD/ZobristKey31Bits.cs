using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZobristKey31Bits
{
    // Matriz que almacena las claves Zobrist generadas para cada posición y pieza en el tablero.
    private int[,] keys;

    // Número de posiciones en el tablero y número de piezas (tipos de piezas) 
    private int boardPositions, numberOfPieces;
    public int BoardPositions { get { return boardPositions; } }
    public int NumberOfPieces { get { return numberOfPieces; } }


    // Inicializa la matriz de claves Zobrist y genera claves aleatorias para cada posición y pieza.
    public ZobristKey31Bits(int _boardPositions, int _numberOfPieces)
    {
        boardPositions = _boardPositions;  
        numberOfPieces = _numberOfPieces;  

        // Inicializa la matriz para las claves Zobrist. Cada posición tendrá una clave para cada pieza posible.
        keys = new int[boardPositions, numberOfPieces];

        // Generador de números aleatorios para generar las claves Zobrist.
        System.Random rdn = new System.Random();

        // Llenado de la matriz con valores aleatorios, uno por cada posición y tipo de pieza.
        for (int i = 0; i < boardPositions; i++)
        {
            for (int j = 0; j < numberOfPieces; j++)
            {
                // Asigna una clave aleatoria a cada posición de tablero y pieza.
                keys[i, j] = rdn.Next(int.MaxValue); 
            }
        }
    }

    public int GetKeys(int position, int piece)
    {
        // Devuelve la clave Zobrist asociada a la posición y pieza.
        return keys[position, piece]; 
    }
}
