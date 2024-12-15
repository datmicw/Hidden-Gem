using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject squarePrefab;  // prefab của ô vuông
    private GameObject _gridParent;  // đối tượng chứa các ô vuông
    public Sprite[] gemSprites; // mảng của gem
    private bool[,] gridOccupied;   // check ô nào có gem rồi
    public GameObject framePrefab;  // prefab của khung bên ngoài
    public float spacing = 0.003f;  // khoảng cách các ô
    public int tile = 4;            // kích thước lưới

    void Start()
    {
        GenerateGrid(tile, tile); 
        squarePrefab.SetActive(false);
        int gemCount = 4;  // số lượng gem
        float cellSize = Mathf.Min(
            framePrefab.GetComponent<SpriteRenderer>().bounds.size.x / tile,
            framePrefab.GetComponent<SpriteRenderer>().bounds.size.y / tile
        );
        PlaceGems(tile, tile, gemCount, cellSize); // Đặt gem vào grid
    }

    // Kiểm tra xem có thể đặt gem tại vị trí (startRow, startCol) với kích thước gemSize hay không
    bool CanPlaceGem(int startRow, int startCol, Vector2Int gemSize, int rows, int cols)
    {
        for (int r = 0; r < gemSize.y; r++)
        {
            for (int c = 0; c < gemSize.x; c++)
            {
                int checkRow = startRow + r;
                int checkCol = startCol + c;

                // Kiểm tra nếu vị trí vượt ngoài biên giới hoặc ô đã được chiếm
                if (checkRow >= rows || checkCol >= cols || gridOccupied[checkRow, checkCol])
                {
                    return false;
                }
            }
        }
        return true;
    }

    // Đặt gem vào grid tại vị trí bắt đầu
    void PlaceGem(int startRow, int startCol, Vector2Int gemSize, float cellSize)
    {
        // Đánh dấu các ô đã chiếm
        for (int r = 0; r < gemSize.y; r++)
        {
            for (int c = 0; c < gemSize.x; c++)
            {
                gridOccupied[startRow + r, startCol + c] = true;
            }
        }

        // Tính toán vị trí của gem
        Transform firstSquare = _gridParent.transform.Find($"Square_{startRow}_{startCol}");
        if (firstSquare == null) return;

        Vector3 gemCenter = firstSquare.position;
        gemCenter += new Vector3((gemSize.x - 1) * cellSize / 2, -(gemSize.y - 1) * cellSize / 2, 0);

        // Tạo gem
        GameObject gem = new GameObject($"Gem_{startRow}_{startCol}");
        SpriteRenderer spriteRenderer = gem.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = gemSprites[Random.Range(0, gemSprites.Length)];

        // Đặt vị trí gem
        gem.transform.position = gemCenter;

        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        float scaleX = (cellSize * gemSize.x) / spriteWidth;
        float scaleY = (cellSize * gemSize.y) / spriteHeight;

        float scale = Mathf.Min(scaleX, scaleY);

        gem.transform.localScale = new Vector3(scale, scale, 1);

        gem.transform.SetParent(_gridParent.transform);
    }

    void PlaceGems(int rows, int cols, int gemCount, float cellSize)
    {
        gridOccupied = new bool[rows, cols];  

        Vector2Int[] gemSizes = new Vector2Int[] { new Vector2Int(1, 2), new Vector2Int(1, 2), new Vector2Int(2, 2), new Vector2Int(1, 3) };

        int placedGems = 0;
        int gemIndex = 0; 

        while (placedGems < gemCount)
        {
            Vector2Int gemSize = gemSizes[gemIndex]; 

            int startRow = Random.Range(0, rows);
            int startCol = Random.Range(0, cols);

            if (CanPlaceGem(startRow, startCol, gemSize, rows, cols))
            {
                PlaceGem(startRow, startCol, gemSize, cellSize);
                placedGems++;
            }
            gemIndex = (gemIndex + 1) % gemSizes.Length;
        }
    }

    // Tạo lưới ô vuông
    public void GenerateGrid(int rows, int cols)
    {
        if (_gridParent != null)
        {
            Destroy(_gridParent);
        }
        _gridParent = new GameObject("GridParent");

        Sprite squareSprite = squarePrefab.GetComponent<SpriteRenderer>().sprite;
        float frameWidth = framePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float frameHeight = framePrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        float cellSize = Mathf.Min(
            (frameWidth - (cols - 1) * spacing) / cols,
            (frameHeight - (rows - 1) * spacing) / rows
        );

        // Tạo đối tượng khung và điều chỉnh kích thước
        Vector3 newScale = new Vector3(
            (cols * cellSize + (cols - 1) * spacing) / frameWidth,
            (rows * cellSize + (rows - 1) * spacing) / frameHeight,
            1
        );

        GameObject frameObject = new GameObject("Frame");
        frameObject.layer = 2;
        SpriteRenderer frameRenderer = frameObject.AddComponent<SpriteRenderer>();
        frameRenderer.sprite = framePrefab.GetComponent<SpriteRenderer>().sprite;
        frameObject.transform.localScale = newScale;

        frameObject.transform.position = new Vector3(0, 0, 0);

        _gridParent.transform.position = new Vector3(0, 0, 0);

        float xOffset = (cols * cellSize + (cols - 1) * spacing) / 2;
        float yOffset = (rows * cellSize + (rows - 1) * spacing) / 2;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                float xPos = col * (cellSize + spacing) - xOffset + cellSize / 2;
                float yPos = -row * (cellSize + spacing) + yOffset - cellSize / 2;
                Vector3 position = new Vector3(xPos, yPos, 0);

                GameObject square = Instantiate(squarePrefab, position, Quaternion.identity);
                square.name = $"Square_{row}_{col}";
                square.transform.SetParent(_gridParent.transform);

                square.transform.localScale = new Vector3(
                    cellSize / squareSprite.bounds.size.x,
                    cellSize / squareSprite.bounds.size.y,
                    1
                );
            }
        }
    }
}
