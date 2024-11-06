using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Arreglo de posiciones validas para colocar las fichas
    [SerializeField] private Vector2[] validPos;
	public Vector2[] ValidPos { get { return validPos; } }

    // Referencia al spawner del tablero que gestiona las posiciones del tablero
    [SerializeField] private BoardSpawner boardSpawner;

    // Prefab de la ficha que se va a colocar en el tablero
    [SerializeField] private GameObject tokenPrefab;

    // Controla el turno del jugador actual (1 o 2)
    [SerializeField] private int currentPlayerTurn = 1;

    // Controla si el jugador puede realizar un movimiento
    public bool canPlay = false;
    // Indica si el jugador actual es un humano
    [HideInInspector]
    public bool isPlayer = false;
    // Controla si puede empezar una partida
    private bool canStart = true;

    // Referencias a los algoritmos para los jugadores
    [SerializeField] private Algorithm player1, player2, currentPlayer;
    public Algorithm Player1 { get { return player1; } }
    public Algorithm Player2 { get { return player2; } }
    public Algorithm CurrentPlayer { get { return currentPlayer; } }

    // Seleccionador de jugadores
    public PlayerSelector playerSelector;

    // Lista de opciones de algoritmos (IA o Player) disponibles
    [SerializeField] private List<Algorithm> algorithmOptions = new List<Algorithm>();
    public List<Algorithm> AlgorithmsOptions { get { return algorithmOptions; } }

    // Texto de EndGame
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private GameObject endImageBackground;
    [SerializeField] private GameObject startImage;

	private void Awake()
	{
        // Inicializar lista posiciones validas
		validPos = new Vector2 [boardSpawner.NumColumns];
	}
	
    private void Start()
    {
        // Establecer posiciones validas
        SetInitialValidPos();
    }

    void SetInitialValidPos()
    {
        for(int i = 0; i < validPos.Length; i++)
        {
            validPos[i] = new Vector2(i, 0);
        }
    }

    // Comienzo de partida(llamada a funcion con boton en la escena)
    public void StartMatch()
    {
        if(canStart)
        {
            // Asignar algoritmos para cada jugador
            player1 = algorithmOptions[playerSelector.Dropdown1.value];
            if (player1 != null) { player1.SetGameManager(this); }

            player2 = algorithmOptions[playerSelector.Dropdown2.value];
            if (player2 != null) { player2.SetGameManager(this); }

            canStart = false;
            startImage.SetActive(false);

            // Primera jugada
            MakePlay();
        }
    }

    void MakePlay()
    {
        canPlay = true;

        // Selecciona el jugador actual según el turno
        currentPlayer = currentPlayerTurn == 1 ? player1 : player2;

        // Verifica si el jugador actual es humano o IA
        if (currentPlayer is Player)
        {
            isPlayer = true;
        }
        else
        {
            isPlayer = false;
            PlaceToken(currentPlayer.DecideMove(boardSpawner.intBoard, currentPlayerTurn).y);
        }
    }

    public void PlaceToken(int column)
    {
        canPlay = false;

        // Instanciar el token en la parte superior de la columna seleccionada
        Vector3 spawnPosition = new Vector3(column, boardSpawner.NumRows, 0); // Parte superior
        GameObject tokenInstance = Instantiate(tokenPrefab, spawnPosition, Quaternion.identity);
        tokenInstance.tag = "Token";

        tokenInstance.GetComponent<SpriteRenderer>().color = currentPlayerTurn == 1 ? Color.red : Color.yellow;
        tokenInstance.GetComponent<SpriteRenderer>().sortingOrder = 1;

        // Establecer el tablero (boardSpawner) como el padre del token
        tokenInstance.transform.SetParent(boardSpawner.transform, false);


        // Mover el token a la primera posición válida en la columna
        Vector2 targetPosition = validPos[column];
        Vector3 localTargetPosition = boardSpawner.transform.TransformPoint(new Vector3(targetPosition.x, targetPosition.y, 0));

        // Actualizar la posición válida de la columna
        validPos[column] = new Vector2(column, validPos[column].y + 1);

        boardSpawner.intBoard[column, (int)validPos[column].y - 1] = currentPlayerTurn;

        StartCoroutine(MoveToken(tokenInstance, localTargetPosition));

    }

    private IEnumerator MoveToken(GameObject token, Vector2 targetPosition)
    {
        Vector3 startPosition = token.transform.position;
        float time = 0;
        float duration = 1f;  // Tiempo para mover el token

        while (time < duration)
        {
            token.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        token.transform.position = targetPosition;  // Asegurar que el token llegue a la posición final

        if (CheckEnd()) { yield break; }

        // Cambiar de turno
        currentPlayerTurn = currentPlayer.ChangeTurn(currentPlayerTurn);

        canPlay = true;

        // Realizar el siguiente movimiento cuando la ficha este en su lugar
        MakePlay();
    }

    private void EndGame(string message)
    {
        endGameText.text = message;
        endGameText.gameObject.SetActive(true);

        endImageBackground.SetActive(true);

        canPlay = false;
        isPlayer = false;

        StartCoroutine(EnGameScreen());
    }

    private bool CheckEnd()
    {
        // Comprobar ganador
        if (currentPlayer.CheckWin(boardSpawner.intBoard, currentPlayerTurn))
        {
            EndGame($"Player {currentPlayerTurn} wins!!!");
            return true;
        }

        // Comprobar empate
        if (currentPlayer.CheckDraw(boardSpawner.intBoard))
        {
            EndGame("It's a draw");
            return true;
        }

        return false;
    }

    private IEnumerator EnGameScreen()
    {
        float time = 0;
        float duration = 5f;

        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        Restart();
    }

    public void Restart()
    {
        // Reiniciar el tablero a cero
        for (int column = 0; column < boardSpawner.NumColumns; column++)
        {
            for (int row = 0; row < boardSpawner.NumRows; row++)
            {
                boardSpawner.intBoard[column, row] = 0;
            }
        }

        // Borrar todos los tokens creados
        foreach (Transform child in boardSpawner.transform)
        {
            if(child.CompareTag("Token"))
            {
                Destroy(child.gameObject); // Destruir todos los hijos del spawner, es decir, las fichas creadas
            }
            else if(child.CompareTag("Tile"))
            {
                child.GetComponent<Tile>().isOccupied = false;
            }
        }

        // Reiniciar las posiciones válidas para colocar fichas (validPos)
        SetInitialValidPos();

        // Anular los jugadores y reiniciar el turno
        player1 = null;
        player2 = null;
        currentPlayer = null;
        currentPlayerTurn = 1;

        // Ocultar el texto de fin de partida
        endGameText.gameObject.SetActive(false);
        endImageBackground.SetActive(false);
        startImage.SetActive(true);

        canPlay = false;
        isPlayer = false;
        canStart = true;
    }
}
