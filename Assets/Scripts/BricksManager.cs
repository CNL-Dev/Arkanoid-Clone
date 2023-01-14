using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BricksManager : MonoBehaviour
{
    #region Singleton

    

    private static BricksManager _instance;

    public static BricksManager Instance => _instance;

    private void Awake()
    {
        //Ensures that there is only 1 instance of bricksManager
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    private int maxRows = 17;
    private int maxCols = 12;
    private float initialBrickSpawnPosX = -1.96f;
    private float initialBrickSpawnPosY = 3.325f;
    private float shiftAmount = 0.365f;

    private GameObject bricksContainer;

    public Brick brickPrefab;

    public Sprite[] Sprites;

    public Color[] BrickColors;

    public List<Brick> RemainingBricks { get; set; }

    public List<int[,]> levelsData { get; set; }

    public int InitialBricksCount { get; set; }

    public int currentLevel;

    private void Start()
    {
        this.bricksContainer = new GameObject("BrickContainer");
        this.RemainingBricks = new List<Brick>();
        this.levelsData = this.LoadLevelsData();
        this.GenerateBricks();
    }

    private void GenerateBricks()
    {
        this.RemainingBricks = new List<Brick>();
        int[,] currentLevelData = this.levelsData[this.currentLevel];
        float currentSpawnX = initialBrickSpawnPosX;
        float currentSpawnY = initialBrickSpawnPosY;
        float zShift = 0f;

        for(int i = 0; i < this.maxRows; i++)
        {
            for(int j = 0; j < this.maxCols; j++)
            {
                int brickType = currentLevelData[i, j];

                if (brickType > 0)
                {
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(bricksContainer.transform, this.Sprites[brickType - 1], this.BrickColors[brickType], brickType);

                    this.RemainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }

                currentSpawnX += shiftAmount;
                if (j + 1 == this.maxCols)
                {
                    currentSpawnX = initialBrickSpawnPosX;
                }
            }

            currentSpawnY -= shiftAmount;
        }

        this.InitialBricksCount = this.RemainingBricks.Count;
    }

    //Generate levels from text file
    private List<int[,]> LoadLevelsData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;

        string[] rows = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxCols];
        int currentRow = 0;

        for(int i = 0; i < rows.Length; i++)
        {
            string line = rows[i];

            //Generate level
            if (line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
                for(int j = 0; j < bricks.Length; j++)
                {
                    currentLevel[currentRow, j] = int.Parse(bricks[j]);
                }

                currentRow++;
            }
            else
            {
                //End of current level
                currentRow = 0;
                levelsData.Add(currentLevel);
                currentLevel = new int[maxRows, maxCols];
            }
;       }

        return levelsData;
    }

    public void LoadLevel(int level)
    {
        this.currentLevel = level;
        this.ClearRemainingBricks();
        this.GenerateBricks();
    }

    private void ClearRemainingBricks()
    {
        foreach(Brick brick in this.RemainingBricks.ToList())
        {
            Destroy(brick.gameObject);
        }
    }

    public void LoadNextLevel()
    {
        this.currentLevel++;

        if(this.currentLevel >= this.levelsData.Count)
        {
            GameManager.Instance.ShowVictoryScreen();
        }
        else
        {
            this.LoadLevel(this.currentLevel);
        }
    }
}
