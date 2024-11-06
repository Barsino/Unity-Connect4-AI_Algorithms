using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranspositionTable
{
    // Longitud de la tabla
    private int length;

    // Diccionario para almacenar los registros de la tabla de transposición.
    Dictionary<int, BoardRecord> records;

    // Contadores para llevar el control de estadísticas de la tabla.
    protected int usedRecords, overwritenRecords, notFoundRecords, regCoincidentes, regNoCoincidentes;


    public TranspositionTable(int _length)
    {
        length = _length;  
        records = new Dictionary<int, BoardRecord>(length); 
    }

    public void SaveRecord(BoardRecord record)
    {
        // Se guarda el registro usando el valor hash del estado del tablero como clave.
        records[record.hashValue] = record;
    }

    public BoardRecord GetRecord(int hash)
    {
        BoardRecord record;

        // Se calcula la clave para la tabla usando el valor hash del estado del tablero.
        int key = hash % length;

        // Verifica si la tabla contiene la clave calculada.
        if (records.ContainsKey(key))
        {
            // Recupera el registro asociado con esa clave.
            record = records[key];

            // Verifica si el valor hash del registro coincide con el valor hash que estamos buscando.
            if (record.hashValue == hash)
            {
                return record;  
            }
            else
            {
                return null;  
            }
        }
        else
        {
            return null;
        }
    }
}
