using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRecord : MonoBehaviour
{
    public int score;
    public int depth;
    public ScoreType scoreType;
}

public enum ScoreType
{
    Exact,
    LowerBound,
    UpperBound
}