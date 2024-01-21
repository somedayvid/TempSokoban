using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.UI.Image;

public class LevelStorage : MonoBehaviour
{
    public ObjectManager objectManager;

    public Block wall;
    public Block player;
    public Block pushBlock;
    public Block pullBlock;
    public Block pushNPullBlock;

    private int currentLevel = 1;
    public bool levelLoaded = false;

    private int currentRows;
    private int currentColumns;

    public Dictionary<int, int[,]> levelDict = new Dictionary<int, int[,]>();

    public int Rows => currentRows;
    public int Cols => currentColumns;

    public int[,] level1 = 
        { { 2, 2, 2, 2, 2, 2},
          { 2, 3, 0, 0, 3, 2},
          { 2, 1, 3, 0, 0, 2},
          { 2, 3, 0, 0, 3, 2},
          { 2, 2, 2, 2, 2, 2},
        };

    public int[,] level2 =
    {
        {2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
        {2, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
        {2, 3, 3, 0, 0, 0, 0, 0, 0, 2 },
        {2, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
        {2, 0, 3, 3, 3, 3, 3, 3, 0, 2 },
        {2, 0, 3, 3, 3, 3, 3, 3, 0, 2 },
        {2, 1, 0, 4, 0, 0, 0, 3, 0, 2 },
        {2, 3, 3, 3, 3, 3, 3, 3, 0, 2 },
        {2, 3, 3, 3, 3, 3, 3, 3, 0, 2 },
        {2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }
    };

    public int[,] level3 =
    {
        { 2, 2, 2, 2, 2, 2, 2 },
        { 2, 0, 0, 0, 4, 4, 2 },
        { 2, 0, 0, 0, 4, 4, 2 },
        { 2, 4, 3, 0, 3, 3, 2 },
        { 2, 0, 0, 3, 3, 3, 2 },
        { 2, 1, 4, 0, 3, 3, 2 },
        { 2, 2, 2, 2, 2, 2, 2 }
    };

    public int[,] level4 =
    {

    };

    public int[,] level5 =
    {

    };

    private void Awake()
    {
        levelDict.Add(1, level1);
        levelDict.Add(2, level2);
        levelDict.Add(3, level3);
        levelDict.Add(4, level4);
        levelDict.Add(5, level5);
    }

    void Update()
    {         
        LevelLoading();
        if (WinCondition())
        {
            levelLoaded = false;
            currentLevel++;
        }
        Restart();
    }

    private void LevelLoading()
    {
        if (!levelLoaded)
        {
            objectManager.ClearGameArray();
            int[,] currentStage = levelDict[currentLevel];
            int rows = currentRows = currentStage.GetLength(0);
            int columns = currentColumns = currentStage.GetLength(1);
            objectManager.CreateGameArray(columns, rows);
            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++)
                {
                    //j is the vertical length and i is the horizontal length
                    switch (currentStage[j, i])
                    {
                        case 0:
                            break;
                        case 1:
                            objectManager.CreateBlock(player, i, j);
                            break;
                        case 2:
                            objectManager.CreateBlock(wall, i, j);
                            break;
                        case 3:
                            objectManager.CreateBlock(pushBlock, i, j);
                            break;
                        case 4:
                            objectManager.CreateBlock(pullBlock, i, j);
                            break;
                    }
                }
            }
            levelLoaded = true;
        }
    }

    private bool WinCondition()
    {
        bool winFulfilled = false;

        switch (currentLevel)
        {
            case 1:
                if (objectManager.GameArray[4,2] != null && objectManager.GameArray[4, 2].GetType() == typeof(PushBlock))
                {
                    winFulfilled = true;
                }
                break;
            case 2:
                if (objectManager.GameArray[1, 4] != null && objectManager.GameArray[1, 4].GetType() == typeof(PushBlock) &&
                    objectManager.GameArray[1, 5] != null && objectManager.GameArray[1, 5].GetType() == typeof(PushBlock) &&
                    objectManager.GameArray[1, 6] != null && objectManager.GameArray[1, 6].GetType() == typeof(PushNPullBlock))
                {
                    winFulfilled = true;    
                }
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }

        return winFulfilled;
    }

    private void Restart()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            levelLoaded = false;
        }
    }
}
