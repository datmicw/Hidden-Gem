using UnityEngine;

public class Stage
{
    public int Rows { get; private set; }
    public int Cols { get; private set; }
    public Stage(int rows, int cols)
    {
        Rows = rows;
        Cols = cols;

    }
}