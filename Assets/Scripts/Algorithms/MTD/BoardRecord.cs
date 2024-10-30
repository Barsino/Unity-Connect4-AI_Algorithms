using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRecord : MonoBehaviour
{
    public int hashValue;
    public int minScore;
    public int maxScore;
    public int bestMove;
    public int depth;

    public BoardRecord()
    {
        hashValue = 0;
        minScore = 0;
        maxScore = 0;
        bestMove = 0;
        depth = 0;
    }

}
