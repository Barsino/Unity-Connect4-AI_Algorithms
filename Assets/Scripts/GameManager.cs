using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Vector2[] validPos;
	public Vector2[] ValidPos { get { return validPos; } }

    [SerializeField] private BoardSpawner boardSpawner;

    [SerializeField] private GameObject tokenPrefab;

    private int currentPlayer = 1;
    public bool canPlay = true;
    [SerializeField] private Algorithm player1, player2;
    public Algorithm Player1 { get { return player1; } }
    public Algorithm Player2 { get { return player2; } }

    public PlayerSelector playerSelector;
	private void Awake()
	{
		validPos = new Vector2 [boardSpawner.NumColumns];
	}
	
    private void Start()
    {
        SetInitialValidPos();
    }

    void SetInitialValidPos()
    {
        for(int i = 0; i < validPos.Length; i++)
        {
           validPos[i] = new Vector2(i, 0);
        }
    }

    public void PlaceToken(int column)
    {
        canPlay = false;

        // Instanciar el token en la parte superior de la columna seleccionada
        Vector3 spawnPosition = new Vector3(column, boardSpawner.NumRows, 0); // Parte superior
        GameObject tokenInstance = Instantiate(tokenPrefab, spawnPosition, Quaternion.identity);

        tokenInstance.GetComponent<SpriteRenderer>().color = currentPlayer == 1 ? Color.red : Color.yellow;
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
        currentPlayer = (currentPlayer == 1) ? 2 : 1;

        canPlay = true;
    }

    public void StartMatch()
    {
        player1 = playerSelector.AlgorithmOptions[playerSelector.Dropdown1.value];
        player2 = playerSelector.AlgorithmOptions[playerSelector.Dropdown2.value];
    }

    public int GetCurrentPlayer()
    {
        return currentPlayer;
    }
}
