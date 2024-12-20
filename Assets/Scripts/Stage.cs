using UnityEngine;

public class Stage
{
    public int Rows { get; private set; }
    public int Cols { get; private set; }
    public int gemCount { get; private set; }
    public Vector2Int[] gemSizes { get; private set; }

    public Stage(int rows, int cols, int gemCount, Vector2Int[] gemSizes)
    {
        Rows = rows;
        Cols = cols;
        this.gemCount = gemCount;
        this.gemSizes = gemSizes;
    }
}
