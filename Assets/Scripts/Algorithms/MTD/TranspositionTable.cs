using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranspositionTable : MonoBehaviour
{
    private int length;
    Dictionary<int, BoardRecord> records;

    protected int usedRecords, overwritenRecords, notFoundRecords, regCoincidentes, regNoCoincidentes;

    public TranspositionTable(int _length)
    {
        length = _length;
        records = new Dictionary<int, BoardRecord>();
    }

    public void SaveRecord(BoardRecord record)
    {
        records[record.hashValue % length] = record;
    }

    public BoardRecord GetRecord(int hash)
    {
        BoardRecord record;
        int key = hash % length;

        if(records.ContainsKey(key))
        {
            record = records[key];

            if(record.hashValue == hash) { return record; }

            else { return null; }
        }
        else
        {
            return null;
        }
    }
}