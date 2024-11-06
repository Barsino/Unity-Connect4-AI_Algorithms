using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private Vector2 boardPos;
	
	public bool isOccupied = false;
	public bool IsOccupied { get { return isOccupied; } }
	

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        gameManager = FindObjectOfType<GameManager>();
    }

    // Método para asignar la posición del tablero desde el BoardSpawner
    public void SetBoardPos(Vector2 pos)
    {
        boardPos = pos;
    }

    // Cambiar color del tile
    private void OnMouseEnter()
    {
        spriteRenderer.color = new Color(0f, 1f, 0f, 0.5f);
    }
    private void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
    }
	public void OnMouseDown()
	{
        if (gameManager.canPlay && gameManager.isPlayer && !isOccupied)
        {
            // Verificar si el clic corresponde a una posición válida
            if (InValidPos(gameManager.ValidPos))
            {
                gameManager.PlaceToken((int)boardPos.x);  // Llamar al GameManager para colocar el token
                isOccupied = true;
            }
        }        
    }

    private bool InValidPos(Vector2[] validPos)
    {
        // Revisar si el clic está en la posición válida más baja de la columna
        return boardPos == validPos[(int)boardPos.x];
    }
}