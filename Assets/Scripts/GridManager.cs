using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Sprite squareSprite;  // Sprite của ô vuông
    public Sprite gemSprite; // Sprite của gem
    private GameObject _gridParent;  // GameObject chứa tất cả các ô vuông
    public float spacing = 0.54f; 

    void Start()
    {
        _gridParent = new GameObject("GridParent");
        _gridParent.transform.position = Vector3.zero;
        GenerateGrid(5, 5, 3);
    }

    public void GenerateGrid(int rows, int cols, int gemCount)
    {
        foreach (Transform child in _gridParent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 position = new Vector3(col * spacing, -row * spacing, 0);
                GameObject square = new GameObject("Square_" + row + "_" + col);
                SpriteRenderer spriteRenderer = square.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = squareSprite; 
                square.transform.position = position; 
                square.transform.SetParent(_gridParent.transform);
            }
        }

        // Đặt gems ngẫu nhiên vào ô vuông
        PlaceGems(rows, cols, gemCount);
    }

    void PlaceGems(int rows, int cols, int gemCount)
    {
        int placed = 0;

        // Đặt gems ngẫu nhiên
        while (placed < gemCount)
        {
            int randomRow = Random.Range(0, rows);
            int randomCol = Random.Range(0, cols);

            // Tạo vị trí ngẫu nhiên cho gem, gem sẽ nằm dưới ô vuông
            Vector3 position = new Vector3(randomCol * spacing, -(randomRow * spacing) - (spacing / 2), 0);

            // Kiểm tra xem vị trí đã có gem chưa
            bool hasGem = false;
            foreach (Transform child in _gridParent.transform)
            {
                if (child.position == position && child.CompareTag("Gem"))
                {
                    hasGem = true;
                    break;
                }
            }

            // tạo gem mới nếu chưa có gem ở vị trí này
            if (!hasGem)
            {
                GameObject gem = new GameObject("Gem_" + randomRow + "_" + randomCol);
                gem.AddComponent<SpriteRenderer>().sprite = gemSprite;
                gem.transform.position = position;
                gem.tag = "Gem";  // Đánh dấu gem là đối tượng gem
                placed++;
            }
        }
    }
}
