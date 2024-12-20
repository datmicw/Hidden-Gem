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

    // tạo gem với số lượng và gem prefab được cung cấp
    public void PlaceGems(Vector2Int[] gemSizes, GameObject[] gemPrefabs)
    {
        // Create a 2D array to track the occupied squares
        bool[,] occupied = new bool[Rows, Cols];

        for (int i = 0; i < gemSizes.Length; i++)
        {
            Vector2Int gemSize = gemSizes[i];
            GameObject gemPrefab = gemPrefabs[Random.Range(0, gemPrefabs.Length)];

            bool placed = false;
            int attempts = 0;
            while (!placed && attempts < 100)  // giới hạn
            {
                attempts++;

                // ngẫu nhiên vị trí
                int randomRow = Random.Range(0, Rows - gemSize.x + 1);
                int randomCol = Random.Range(0, Cols - gemSize.y + 1);

                bool canPlace = true;
                for (int r = 0; r < gemSize.x; r++)
                {
                    for (int c = 0; c < gemSize.y; c++)
                    {
                        if (occupied[randomRow + r, randomCol + c])  // nếu đã trùng
                        {
                            canPlace = false;
                            break;
                        }
                    }
                    if (!canPlace) break;
                }

                if (canPlace)
                {
                    Vector3 gemPosition = squares[randomRow, randomCol].transform.position;
                    gemPosition.z = -1; // Ensure gems are below the square layer
                    GameObject gem = Object.Instantiate(gemPrefab, gemPosition, Quaternion.identity);
                    gem.name = $"Gem_{randomRow}_{randomCol}";
                    gem.transform.SetParent(parentTransform);

                    for (int r = 0; r < gemSize.x; r++)
                    {
                        for (int c = 0; c < gemSize.y; c++)
                        {
                            occupied[randomRow + r, randomCol + c] = true;
                        }
                    }
                    placed = true;  //đặt thành công
                }
            }

            if (attempts >= 100)
            {
                Debug.LogWarning("Too many attempts to place gem, some gems may not be placed.");
            }
        }
    }
}
