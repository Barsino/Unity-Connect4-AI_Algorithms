using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    private SpriteRenderer tileSpriteRenderer;


    const int numColumns = 7;
    public int NumColumns { get { return numColumns; } }


    const int numRows = 6;
    public int NumRows { get { return numRows; } }


    private GameObject[,] board;
    public GameObject[,] Board { get { return board; } }

    private int[,] intBoard;
    public int[,] IntBoard { get { return intBoard; } }

    private void Start()
    {
        board = new GameObject[numColumns, numRows];
        CreateBoard();
    }

    private void CreateBoard()
    {
        // Create a boars with thw num of columns an rows
        for (int column = 0; column < numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                // Instatiate tile
                GameObject tileInstance = Instantiate(tilePrefab, new Vector3(0, 0, 0), Quaternion.identity);

                tileInstance.GetComponent<SpriteRenderer>().color = ((column + row) % 2 == 0) ? Color.white : Color.gray;

                tileInstance.transform.parent = transform;
                tileInstance.GetComponent<Transform>().position = new Vector3(column, row, 0) + transform.position;

                tileInstance.name = "Tile " + column + "," + row;

                // Asignar boardPos a la casilla
                Tile tile = tileInstance.GetComponent<Tile>();
                if (tile != null)
                {
                    tile.SetBoardPos(new Vector2(column, row));  // Asignar la posición en el tablero
                }

                board[column, row] = tileInstance;
                intBoard[column, row] = 0;
            }
        }
    }
}
