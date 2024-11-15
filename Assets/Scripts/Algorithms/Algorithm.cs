
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public abstract class Algorithm : ScriptableObject
{
    public abstract string AlgorithmName { get; }

    [SerializeField] private GameManager gameManager;

    // Metodo que devuelve el mejor movimiento elejido por el correspondiente algoritmo
    public abstract Vector2Int DecideMove(int[,] board, int player);

    // Cambiar de turno
    public int ChangeTurn(int actualPlayer)
    {
        return actualPlayer == 1 ? 2 : 1;
    }

    // Comprobar si el juego ha terminado(victoria o empate)
    protected bool IsEndOfGame(int[,] board, int player)
    {
        if(CheckWin(board, player) || CheckDraw(board)) { return true; }

        return false;
    }

    #region Evaluation
    protected int Evaluate(int[,] board, int player)
    {
        int score = 0;
        int columns = board.GetLength(0);
        int rows = board.GetLength(1);
        int opponent = (player == 1) ? 2 : 1;

        // Puntuacion por columnas centrales (favorece colocar fichas en el centro)
        int centerColumn = columns / 2;
        int centerPlayerCount = 0;

        for (int row = 0; row < rows; row++)
        {
            if (board[centerColumn, row] == player) { centerPlayerCount++; }
        }

        score += centerPlayerCount * 3;

        // Evaluar todas las posiciones posibles de 4 en linea
        for(int column = 0; column < columns; column++)
        {
            for(int row = 0; row < rows; row++)
            {
                // Horizontal
                if(column <= columns - 4)
                {
                    score += EvaluateWindow(new int[]
                    {
                        board[column    , row],
                        board[column + 1, row],
                        board[column + 2, row],
                        board[column + 3, row]

                    }, player, opponent);
                }

                // Vertical
                if (row <= rows - 4)
                {
                    score += EvaluateWindow(new int[]
                    {
                        board[column, row],
                        board[column, row + 1],
                        board[column, row + 2],
                        board[column, row + 3]

                    }, player, opponent);
                }

                // Diagonal Izquierda => Derecha
                if(column <= columns - 4 && row <= rows - 4)
                {
                    score += EvaluateWindow(new int[]
{
                        board[column    , row],
                        board[column + 1, row + 1],
                        board[column + 2, row + 2],
                        board[column + 3, row + 3]

                    }, player, opponent);
                }

                // Diagonal Derecha => Izquierda
                if(column <= columns - 4 && row >= 3)
                {
                    score += EvaluateWindow(new int[]
{
                        board[column    , row],
                        board[column + 1, row - 1],
                        board[column + 2, row - 2],
                        board[column + 3, row - 3]

                    }, player, opponent);
                }
            }
        }

        return score;
    }

    private int EvaluateWindow(int[] window, int player, int opponent)
    {
        int score = 0;

        int playerCount = 0, opponentCount = 0, emptyCount = 0;

        foreach(int tile in window)
        {
            if(tile == player)
            {
                playerCount++;
            }
            else if(tile == opponent)
            {
                opponentCount++;
            }
            else
            {
                emptyCount++;
            }
        }

        // Asignar puntaje basado en la cantidad de fichas del jugador o del oponente en el grupo
        if (playerCount == 4) { score += 100; }// Ganar

        else if (playerCount == 3 && emptyCount == 1) { score += 5; }// Tres en l�nea con un espacio

        else if (playerCount == 2 && emptyCount == 2) { score += 2; }// Dos en l�nea con dos espacios

        //if (opponentCount == 4) { score -= 100; }// Oponente gana

        else if (opponentCount == 3 && emptyCount == 1) { score -= 10; }// Oponente con tres en l�nea

        else if (opponentCount == 2 && emptyCount == 2) { score -= 4; }// Oponente con dos en l�nea

        return score;
    }
    #endregion

    #region Checkers
    public bool CheckDraw(int[,] board)
    {
        //// Revisar empate comprobando si hay alg�n "0" en la ultiam fila del tablero
        for (int column = 0; column < board.GetLength(0); column++)
        {
            if (board[column, board.GetLength(1) - 1] == 0) { return false; }
        }

        return true;
    }

    public bool CheckWin(int[,] board, int player)
    {
        int columns = board.GetLength(0);
        int rows    = board.GetLength(1);

        for(int column = 0; column < columns; column++)
        {
            for(int row = 0; row < rows; row++)
            {
                if(CheckHorizontal(board, column, row, player) ||
                   CheckVertical  (board, column, row, player) ||
                   CheckDiagonal  (board, column, row, player))
                {
                    return true;
                }
            }
        }

        return false;
    }

    // Comprobar ganador en horizontal
    private bool CheckHorizontal(int[,] board, int column, int row, int player)
    {
        int columns = board.GetLength(0);

        if (column <= columns -4 &&
            player == board[column    , row] &&
            player == board[column + 1, row] &&
            player == board[column + 2, row] &&
            player == board[column + 3, row])
        {
            return true;
        }

        return false;
    }

    // Comprobar ganador en Vertical
    private bool CheckVertical(int[,] board, int column, int row, int player)
    {
        int rows = board.GetLength(1);

        if (row <= rows - 4 &&
            player == board[column, row    ] &&
            player == board[column, row + 1] &&
            player == board[column, row + 2] &&
            player == board[column, row + 3])
        {
            return true;
        }

        return false;
    }

    // Comprobar ganador en Diagonal
    private bool CheckDiagonal(int[,] board, int column, int row, int player)
    {
        int columns = board.GetLength(0);
        int rows = board.GetLength(1);

        // Izquierda => Derecha
        if (row <= rows - 4 && column <= columns - 4 &&
            player == board[column    , row    ] &&
            player == board[column + 1, row + 1] &&
            player == board[column + 2, row + 2] &&
            player == board[column + 3, row + 3])
        {
            return true;
        }
        // Derecha => Izquierda
        else if (column <= columns - 4 && row >= 3 &&
            player == board[column    , row    ] &&
            player == board[column + 1, row - 1] &&
            player == board[column + 2, row - 2] &&
            player == board[column + 3, row - 3])
        {
            return true;
        }

        return false;
    }
    #endregion

    public void SetGameManager(GameManager gM)
    {
        gameManager = gM;
    }

    // Obtener las posiciones validas donde colocar una ficha
    protected List<Vector2> GetValidPos(int[,] board)
    {
        int columns = board.GetLength(0);
        int rows = board.GetLength(1);

        List<Vector2> validPos = new List<Vector2>();

        for(int column = 0; column < columns; column++)
        {
            if (board[column, rows - 1] == 0)
            {
                int row = 0;
                while(row < rows && board[column, row] != 0) { row++; }
                validPos.Add(new Vector2(column, row));
            }
        }
        return validPos;
    }
}
