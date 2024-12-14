using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject squarePrefab;  // Prefab của ô vuông
    private GameObject _gridParent;  // Đối tượng chứa các ô vuông
    public GameObject framePrefab;   // Prefab của khung bên ngoài 
    public float spacing = 0.003f;   // Khoảng cách giữa các ô vuông
    public int tile = 5;             // Tỉ lệ ô vuông

    void Start()
    {
        GenerateGrid(tile, tile); // Tạo lưới ô vuông
        squarePrefab.SetActive(false);  // Ẩn prefab gốc
    }

    public void GenerateGrid(int rows, int cols)
    {
        // Xóa lưới cũ nếu tồn tại
        if (_gridParent != null)
        {
            Destroy(_gridParent);
        }
        _gridParent = new GameObject("GridParent");

        // Tính kích thước của mỗi ô vuông
        Sprite squareSprite = squarePrefab.GetComponent<SpriteRenderer>().sprite;
        float frameWidth = framePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float frameHeight = framePrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        // Tính kích thước ô vuông sao cho vừa khung
        float cellSize = Mathf.Min(
            (frameWidth - (cols - 1) * spacing) / cols,  // Chiều rộng mỗi ô
            (frameHeight - (rows - 1) * spacing) / rows  // Chiều cao mỗi ô
        );

        Debug.Log($"Calculated Cell Size: {cellSize}");

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

        // Đặt vị trí cố định cho khung tại Vector3(0, 0, 0)
        frameObject.transform.position = new Vector3(0, 0, 0);

        // Đặt vị trí của _gridParent tại (0, 0, 0)
        _gridParent.transform.position = new Vector3(0, 0, 0);

        // Tính toán lại vị trí của các ô vuông sao cho khung bao quanh
        float xOffset = (cols * cellSize + (cols - 1) * spacing) / 2;
        float yOffset = (rows * cellSize + (rows - 1) * spacing) / 2;

        // Tạo các ô vuông
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                // Vị trí của mỗi ô vuông
                float xPos = col * (cellSize + spacing) - xOffset + cellSize / 2;
                float yPos = -row * (cellSize + spacing) + yOffset - cellSize / 2;
                Vector3 position = new Vector3(xPos, yPos, 0);

                // Tạo ô vuông
                GameObject square = Instantiate(squarePrefab, position, Quaternion.identity);
                square.name = $"Square_{row}_{col}";
                square.transform.SetParent(_gridParent.transform);

                // Cập nhật kích thước ô vuông dựa trên tỷ lệ
                square.transform.localScale = new Vector3(
                    cellSize / squareSprite.bounds.size.x,
                    cellSize / squareSprite.bounds.size.y,
                    1
                );
            }
        }
    }
}
