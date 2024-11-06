using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZobristKey31Bits
{
    // Matriz que almacena las claves Zobrist generadas para cada posici�n y pieza en el tablero.
    private int[,] keys;

    // N�mero de posiciones en el tablero y n�mero de piezas (tipos de piezas) 
    private int boardPositions, numberOfPieces;
    public int BoardPositions { get { return boardPositions; } }
    public int NumberOfPieces { get { return numberOfPieces; } }


    // Inicializa la matriz de claves Zobrist y genera claves aleatorias para cada posici�n y pieza.
    public ZobristKey31Bits(int _boardPositions, int _numberOfPieces)
    {
        boardPositions = _boardPositions;  
        numberOfPieces = _numberOfPieces;  

        // Inicializa la matriz para las claves Zobrist. Cada posici�n tendr� una clave para cada pieza posible.
        keys = new int[boardPositions, numberOfPieces];

        // Generador de n�meros aleatorios para generar las claves Zobrist.
        System.Random rdn = new System.Random();

        // Llenado de la matriz con valores aleatorios, uno por cada posici�n y tipo de pieza.
        for (int i = 0; i < boardPositions; i++)
        {
            for (int j = 0; j < numberOfPieces; j++)
            {
                // Asigna una clave aleatoria a cada posici�n de tablero y pieza.
                keys[i, j] = rdn.Next(int.MaxValue); 
            }
        }
    }

    public int GetKeys(int position, int piece)
    {
        // Devuelve la clave Zobrist asociada a la posici�n y pieza.
        return keys[position, piece]; 
    }
}
