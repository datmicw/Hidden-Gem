using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject squarePrefab;
    public GameObject framePrefab;
    public float spacing = 0.003f;
    public int tile = 4;
    private int currentStageIndex = 0;
    private Stage[] stages;

    void Start()
    {
        stages = new Stage[] { new Stage(tile, tile ),};
        LoadStage(currentStageIndex);
    }

    public void LoadStage(int stageIndex)
    {
        Stage stage = stages[stageIndex];
        Grid grid = new Grid(stage.Rows, stage.Cols, squarePrefab, framePrefab, spacing);

         Mathf.Min(
            framePrefab.GetComponent<SpriteRenderer>().bounds.size.x / stage.Cols,
            framePrefab.GetComponent<SpriteRenderer>().bounds.size.y / stage.Rows
        );

        
    }
}