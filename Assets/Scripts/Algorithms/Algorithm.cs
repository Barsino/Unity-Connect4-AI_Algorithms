
using UnityEngine;

public abstract class Algorithm : ScriptableObject
{
    public abstract string AlgorithmName { get; }

    [SerializeField] private GameManager gameManager;

    public abstract Vector2Int DecideMove(BoardSpawner board, int player);

    public int ChangeTurn(int actualPlayer)
    {
        return actualPlayer == 1 ? 2 : 1;
    }

    protected bool IsEndOfGame(int[,] board, int player)
    {
        if(CheckWin(board, player) || CheckDraw(board)) { return true; }

        return false;
    }

    protected int Evaluate()
    {
        int score = 0;

        // TODO Evaluate implementation

        return score;
    }

    #region Checkers
    protected bool CheckWin(int[,] board, int player)
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

    protected bool CheckDraw(int[,] board)
    {
        // Revisar si hay algún "0" en el tablero
        for (int column = 0; column < board.GetLength(0); column++)
        {
            for (int row = 0; row < board.GetLength(1); row++)
            {
                if (board[column, row] == 0)
                {
                    return false; // Todavía hay espacio
                }
            }
        }

        return true;
    }

    // Comprobar ganador en horizontal
    private bool CheckHorizontal(int[,] board, int column, int row, int player)
    {
        int columns = board.GetLength(0);

        if (column <= columns -4 &&
            player == board[row, column + 1] &&
            player == board[row, column + 2] &&
            player == board[row, column + 3])
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
            player == board[row + 1, column] &&
            player == board[row + 2, column] &&
            player == board[row + 3, column])
        {
            return true;
        }

        return false;
    }

    // Comprobar ganador en Diagonal
    private bool CheckDiagonal(int[,] board, int column, int row, int player)
    {
        int columns = board.GetLength(0);
        int rows = board.GetLength(0);

        // Izquierda => Derecha
        if (row <= rows - 4 && column <= columns - 4 &&
            player == board[row + 1, column + 1] &&
            player == board[row + 2, column + 2] &&
            player == board[row + 3, column + 3])
        {
            return true;
        }
        // Derecha => Izquierda
        else if (row <= rows - 4 && column >= 3 &&
            player == board[row + 1, column - 1] &&
            player == board[row + 2, column - 2] &&
            player == board[row + 3, column - 3])
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
    protected Vector2[] GetValidPos()
    {
        return gameManager.ValidPos;
    }


}
