using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
    private int currentPlayerTurn = 1;

    // Controla si el jugador puede realizar un movimiento
    public bool canPlay = false;
    // Indica si el jugador actual es un humano
    public bool isPlayer = false;

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
        // Asignar algoritmos para cada jugador
        player1 = algorithmOptions[playerSelector.Dropdown1.value];
        if(player1 != null) { player1.SetGameManager(this); }

        player2 = algorithmOptions[playerSelector.Dropdown2.value];
        if (player2 != null) { player2.SetGameManager(this); }


        // Primera jugada
        MakePlay();
    }

    void MakePlay()
    {
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
            PlaceToken(currentPlayer.DecideMove(boardSpawner, currentPlayerTurn).x);
        }
    }

    public void PlaceToken(int column)
    {
        canPlay = false;

        // Instanciar el token en la parte superior de la columna seleccionada
        Vector3 spawnPosition = new Vector3(column, boardSpawner.NumRows, 0); // Parte superior
        GameObject tokenInstance = Instantiate(tokenPrefab, spawnPosition, Quaternion.identity);

        tokenInstance.GetComponent<SpriteRenderer>().color = currentPlayerTurn == 1 ? Color.red : Color.yellow;
        tokenInstance.GetComponent<SpriteRenderer>().sortingOrder = 1;

        // Establecer el tablero (boardSpawner) como el padre del token
        tokenInstance.transform.SetParent(boardSpawner.transform, false);


        // Mover el token a la primera posición válida en la columna
        Vector2 targetPosition = validPos[column];
        Vector3 localTargetPosition = boardSpawner.transform.TransformPoint(new Vector3(targetPosition.x, targetPosition.y, 0));

        StartCoroutine(MoveToken(tokenInstance, localTargetPosition));

        // Actualizar la posición válida de la columna
        validPos[column] = new Vector2(column, validPos[column].y + 1);
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

        // Cambiar de turno
        currentPlayer.ChangeTurn(currentPlayerTurn);

        canPlay = true;

        // Realizar el siguiente movimiento cuando la ficha este en su lugar
        MakePlay();
    }
}
