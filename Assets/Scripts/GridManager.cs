using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject squarePrefab;
    public GameObject framePrefab;
    public GameObject gemPrefab;
    public GameObject[] gemPrefabs;  // mảng gameobjects prefab
    public float spacing = 0.003f;
    public int tile = 4;
    private readonly int currentStageIndex = 0;
    private Stage[] stages;

    void Start()
    {
        stages = new Stage[] {
            new Stage( // truyền vào các tham số như số ô tỉ lệ 4x4 số gem và tỉ lệ gem
                tile, 
                tile, 
                4, 
                new Vector2Int[] {
                    new Vector2Int(1, 2),
                    new Vector2Int(1, 2), 
                    new Vector2Int(2, 2), 
                    new Vector2Int(1, 3) 
                }
            )
        };
        
        LoadStage(currentStageIndex);
        gemPrefab.SetActive(false); // ẩn gem pref gốc
    }

    public void LoadStage(int stageIndex)
    {
        Stage stage = stages[stageIndex];
        Grid grid = new Grid(stage.Rows, stage.Cols, squarePrefab, framePrefab, spacing);

        grid.PlaceGems(stage.gemSizes, gemPrefabs);
    }
}
