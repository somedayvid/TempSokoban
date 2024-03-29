using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private float spriteSize = 1.0f;
    public LevelStorage levelStorage;

    public ButtonControls buttonControls;

    //Game grid creation
    private Block[,] gameArray;

    private Vector3 origin;

    //Block prefabs
    public Block pushBlock;

    public Block pullBlock;

    public Block pushNPullBlock;

    private Block player;

    public Transform wallContainer;

    //Public properties
    public Block[,] GameArray => gameArray;

    public float PlayerX => player.transform.position.x;
    public float PlayerY => player.transform.position.y;

    void Awake()
    {
        Camera camera = Camera.main;
        float screenHeight = camera.orthographicSize;
        float screenWidth = screenHeight * camera.aspect;

        ////rowsNum = (int)(Mathf.Floor(screenHeight / spriteSize) * 2);
        int colsNum = (int)Mathf.Floor(screenWidth / spriteSize) * 2;

        float screenGap = (screenWidth * 2 - colsNum) / 2;

        origin = new Vector3(-screenWidth + screenGap, screenHeight);
    }

    void Update()
    {
        if (levelStorage.LevelLoaded)
        {
            if (!buttonControls.LevelSelectionScreen)
            {
                PlayerPushBlock();
                PlayerMovement();
                PlayerPullBlock();
            }
        }
    }

    /// <summary>
    /// Basic Player movement using WASD keys
    /// </summary>
    public void PlayerMovement() {
         if (Input.GetKeyDown(KeyCode.A) && player.CanLeft)
         {
             player.XPos -= 1;
         }
         if (Input.GetKeyDown(KeyCode.S) && player.CanDown)
         {
             player.YPos += 1;
         }
         if (Input.GetKeyDown(KeyCode.W) && player.CanUp)
         {
             player.YPos -= 1;
         }
         if (Input.GetKeyDown(KeyCode.D) && player.CanRight)
         {
             player.XPos += 1;
         }

         MoveBlock(player);
    }

    /// <summary>
    /// If the player walks into a block it will push it along the axis that the player is moving
    /// </summary>
    public void PlayerPushBlock()
    {
        //Checks right direction pushing
        if (player.XPos + 1 < levelStorage.Cols - 1 &&
             gameArray[player.XPos + 1, player.YPos] != null &&
             Input.GetKeyDown(KeyCode.D) &&
             (gameArray[player.XPos + 1, player.YPos].GetType() == typeof(PushBlock) || gameArray[player.XPos + 1, player.YPos].GetType() == typeof(PushNPullBlock)))
        {
            if (gameArray[player.XPos + 1, player.YPos].CanRight)
            {
                PushBlock(gameArray[player.XPos + 1, player.YPos], KeyCode.D);

            }
            else if (player.XPos + 2 < levelStorage.Cols - 1 && !gameArray[player.XPos + 1, player.YPos].CanRight && gameArray[player.XPos + 2, player.YPos].GetType() == typeof(PullBlock))
            {
                Destroy(gameArray[player.XPos + 1, player.YPos].gameObject);
                Destroy(gameArray[player.XPos + 2, player.YPos].gameObject);
                CreateBlock(pushNPullBlock, player.XPos + 2, player.YPos);
            }
            else
            {

            }
        }
        //Checks left direction pushing
        if (player.XPos - 1 >= 0 &&
            gameArray[player.XPos - 1, player.YPos] != null &&
            Input.GetKeyDown(KeyCode.A) &&
            (gameArray[player.XPos - 1, player.YPos].GetType() == typeof(PushBlock) || gameArray[player.XPos - 1, player.YPos].GetType() == typeof(PushNPullBlock)))
        {
            if (gameArray[player.XPos - 1, player.YPos].CanLeft)
            {
                PushBlock(gameArray[player.XPos - 1, player.YPos], KeyCode.A);

            }
            else if (player.XPos - 2 >= 0 && !gameArray[player.XPos - 1, player.YPos].CanLeft && gameArray[player.XPos - 2, player.YPos].GetType() == typeof(PullBlock))
            {
                Destroy(gameArray[player.XPos - 1, player.YPos].gameObject);
                Destroy(gameArray[player.XPos - 2, player.YPos].gameObject);
                CreateBlock(pushNPullBlock, player.XPos - 2, player.YPos);
            }
            else
            {

            }
        }
        //Checks down direction pushing
        if (player.YPos + 1 < levelStorage.Rows - 1 &&
            gameArray[player.XPos, player.YPos + 1] != null &&
            Input.GetKeyDown(KeyCode.S) &&
            (gameArray[player.XPos, player.YPos + 1].GetType() == typeof(PushBlock) || gameArray[player.XPos, player.YPos + 1].GetType() == typeof(PushNPullBlock)))
        {
            if (gameArray[player.XPos, player.YPos + 1].CanDown)                     
            {
                PushBlock(gameArray[player.XPos, player.YPos + 1], KeyCode.S);
            }
            else if (player.YPos + 2 < levelStorage.Rows - 1 && !gameArray[player.XPos, player.YPos + 1].CanDown && gameArray[player.XPos, player.YPos + 2].GetType() == typeof(PullBlock))
            {
                Destroy(gameArray[player.XPos, player.YPos + 1].gameObject);
                Destroy(gameArray[player.XPos, player.YPos + 2].gameObject);
                CreateBlock(pushNPullBlock, player.XPos, player.YPos + 2);
            }
            else
            {

            }
        }
        //Checks up direction pushing
        if (player.YPos - 1 >= 0 &&
            gameArray[player.XPos, player.YPos - 1] != null &&
            Input.GetKeyDown(KeyCode.W) &&
            (gameArray[player.XPos, player.YPos - 1].GetType() == typeof(PushBlock) || gameArray[player.XPos, player.YPos - 1].GetType() == typeof(PushNPullBlock)))

        {
            if (gameArray[player.XPos, player.YPos - 1].CanUp)
            {
                PushBlock(gameArray[player.XPos, player.YPos - 1], KeyCode.W);
            }
            else if (player.YPos - 2 >= 0 && !gameArray[player.XPos, player.YPos - 1].CanUp && gameArray[player.XPos, player.YPos - 2].GetType() == typeof(PullBlock))
            {
                Destroy(gameArray[player.XPos, player.YPos - 1].gameObject);
                Destroy(gameArray[player.XPos, player.YPos - 2].gameObject);
                CreateBlock(pushNPullBlock, player.XPos, player.YPos - 2);
            }
            else
            {

            }
        }
    }

    /// <summary>
    /// If the player walks in a direction away from the block it will pull it along with it
    /// </summary>
    public void PlayerPullBlock()
    {
        if (player.XPos - 2 >= 0 &&                                                          
            gameArray[player.XPos - 2, player.YPos] != null &&                                  
            Input.GetKeyDown(KeyCode.D) &&
            (gameArray[player.XPos - 2, player.YPos].GetType() == typeof(PullBlock) || gameArray[player.XPos - 2, player.YPos].GetType() == typeof(PushNPullBlock)))
        {
            PushBlock(gameArray[player.XPos - 2, player.YPos], KeyCode.D);
        }
        if (player.XPos + 2 < levelStorage.Cols - 1 &&
            gameArray[player.XPos + 2, player.YPos] != null &&
            Input.GetKeyDown(KeyCode.A) &&
            (gameArray[player.XPos + 2, player.YPos].GetType() == typeof(PullBlock) || gameArray[player.XPos + 2, player.YPos].GetType() == typeof(PushNPullBlock)))
        {
            PushBlock(gameArray[player.XPos + 2, player.YPos], KeyCode.A);
        }
        if (player.YPos - 2 >= 0 &&
            gameArray[player.XPos, player.YPos - 2] != null &&
            Input.GetKeyDown(KeyCode.S) &&
           (gameArray[player.XPos, player.YPos - 2].GetType() == typeof(PullBlock) || gameArray[player.XPos, player.YPos - 2].GetType() == typeof(PushNPullBlock)))

        {
            PushBlock(gameArray[player.XPos, player.YPos - 2], KeyCode.S);
        }
        if (player.YPos + 2 < levelStorage.Rows - 1 &&
            gameArray[player.XPos, player.YPos + 2] != null &&
            Input.GetKeyDown(KeyCode.W) &&
            (gameArray[player.XPos, player.YPos + 2].GetType() == typeof(PullBlock) || gameArray[player.XPos, player.YPos + 2].GetType() == typeof(PushNPullBlock)))
        {
            PushBlock(gameArray[player.XPos, player.YPos + 2], KeyCode.W);
        }
    }

    /// <summary>
    /// Instantiates and positions the newly created block
    /// </summary>
    /// <param name="item">The prefab to be instantied as a Block</param>
    /// <param name="xPos">The x position on the grid</param>
    /// <param name="yPos">The y position on the grid</param>
    public void CreateBlock(Block item, int xPos, int yPos)
    {
        Block currentCube = item;

        if (currentCube.name == "Block")
        {
            currentCube = Instantiate(item, wallContainer);
        }
        else
        {
            currentCube = Instantiate(item);
            if(currentCube.name == "Player(Clone)")
            {
                player = currentCube;
            }
        }
        currentCube.XPos = xPos;
        currentCube.YPos = yPos;
        MoveBlock(currentCube);
    }

    /// <summary>
    /// Moves the block to its new position and updates its properties
    /// </summary>
    /// <param name="item">Block to be moved and updated</param>
    private void MoveBlock(Block item)
    {
        if (item.XPos != item.PrevX || item.YPos != item.PrevY)
        {
            if(item.PrevX >= 0 && item.PrevY >= 0)
            {
                gameArray[item.PrevX, item.PrevY] = null;
            }
            item.PrevX = item.XPos;
            item.PrevY = item.YPos;
            gameArray[item.XPos, item.YPos] = item;
            item.transform.position = new Vector3(origin.x + spriteSize / 2 + item.XPos * spriteSize, origin.y - spriteSize / 2 - item.YPos * spriteSize, 1);
        }
    }

    public void CreateGameArray(int columns, int rows)
    {
        gameArray = new Block[columns, rows];
    }

    public void ClearGameArray()
    {
        if (gameArray != null)
        {
            for (int j = 0; j < gameArray.GetLength(0); j++)
            {
                for (int i = 0; i < gameArray.GetLength(1); i++)
                {
                    if (gameArray[j, i] != null)
                    {
                        Destroy(gameArray[j, i].gameObject);
                        gameArray[j, i] = null;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Updates the block properties depending on player input
    /// </summary>
    /// <param name="item">Block to be updated</param>
    /// <param name="direction">Direction the block should be pushed in</param>
    private void PushBlock(Block item, KeyCode direction)
    {
        switch (direction)
        {
            case KeyCode.A:
                item.XPos--;
                break;
            case KeyCode.W:
                item.YPos--;
                break;
            case KeyCode.S:
                item.YPos++;
                break;
            case KeyCode.D:
                item.XPos++;
                break;
        }
        MoveBlock(item);
    }
}
