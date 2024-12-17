using UnityEngine;

public class Grid
{
    private GameObject[,] squares;
    private GameObject gridParent;
    private Transform parentTransform;

    public int Rows { get; private set; }
    public int Cols { get; private set; }

    public Grid(int rows, int cols, GameObject squarePrefab, GameObject framePrefab, float spacing)
    {
        Rows = rows;
        Cols = cols;
        gridParent = new GameObject("GridParent");
        parentTransform = gridParent.transform;
        squares = new GameObject[rows, cols];

        Sprite squareSprite = squarePrefab.GetComponent<SpriteRenderer>().sprite;
        float frameWidth = framePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float frameHeight = framePrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        float cellSize = Mathf.Min(
            (frameWidth - (cols - 1) * spacing) / cols,
            (frameHeight - (rows - 1) * spacing) / rows
        );

        float xOffset = (cols * cellSize + (cols - 1) * spacing) / 2;
        float yOffset = (rows * cellSize + (rows - 1) * spacing) / 2;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                float xPos = col * (cellSize + spacing) - xOffset + cellSize / 2;
                float yPos = -row * (cellSize + spacing) + yOffset - cellSize / 2;
                Vector3 position = new Vector3(xPos, yPos, 0);

                GameObject square = Object.Instantiate(squarePrefab, position, Quaternion.identity);
                square.name = $"Square_{row}_{col}";
                square.transform.SetParent(parentTransform);
                square.transform.localScale = new Vector3(
                    cellSize / squareSprite.bounds.size.x,
                    cellSize / squareSprite.bounds.size.y,
                    1
                );
                squares[row, col] = square;
            }
        }

        squarePrefab.SetActive(false);
    }

}
