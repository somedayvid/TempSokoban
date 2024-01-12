using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private float spriteSize = 1.0f;
    
    //Game grid creation
    private int rowsNum;
    private int colsNum;

    private Block[,] gameArray;

    private Vector3 origin;

    //Block prefabs
    public Block pushBlock;

    public Block pullBlock;

    public Block playerPrefab;

    public Block pushNPullBlock;

    
    private Block player;

    //Public properties
    public Block[,] GameArray => gameArray;
    public int RowsNum => rowsNum;
    public int ColsNum => colsNum;

    void Awake()
    {
        Camera camera = Camera.main;
        float screenHeight = camera.orthographicSize;
        float screenWidth = screenHeight * camera.aspect;

        rowsNum = (int)(Mathf.Floor(screenHeight / spriteSize) * 2);
        colsNum = (int)Mathf.Floor(screenWidth / spriteSize) * 2;

        float screenGap = (screenWidth * 2 - colsNum)/2;

        gameArray = new Block[colsNum, rowsNum];

        origin = new Vector3(-screenWidth + screenGap, screenHeight);

        CreateBlock(playerPrefab, 1, 1);

        CreateBlock(pushBlock, 5, 5);
        CreateBlock(pushBlock, 2, 2);
        CreateBlock(pullBlock, 6, 6);
    }
    void Update()
    {
        PlayerPushBlock();
        PlayerMovement();
        PlayerPullBlock();
    }

    /// <summary>
    /// Basic Player movement using WASD keys
    /// </summary>
    public void PlayerMovement(){
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
    /// Depending on the blocks position relative to the player and its environment it will move around the grid
    /// </summary>
    public void PlayerPushBlock()
    {
        //Checks right direction pushing
        if ((player.XPos + 1 < colsNum - 1 &&                                           
             gameArray[player.XPos + 1, player.YPos] != null &&                         
             Input.GetKeyDown(KeyCode.D)) &&
             gameArray[player.XPos + 1, player.YPos].GetType() == typeof(PushBlock) || gameArray[player.XPos + 1, player.YPos].GetType() == typeof(PushNPullBlock))                                          
        {
            if (gameArray[player.XPos + 1, player.YPos].CanRight)                       
            {
                PushBlock(gameArray[player.XPos + 1, player.YPos], KeyCode.D);

            }
            else if (!gameArray[player.XPos + 1, player.YPos].CanRight && gameArray[player.XPos + 2, player.YPos].GetType() == typeof(PullBlock))
            {
                Destroy(gameArray[player.XPos + 1, player.YPos].gameObject);
                Destroy(gameArray[player.XPos + 2, player.YPos].gameObject);
                CreateBlock(pushNPullBlock, player.XPos + 2, player.YPos);            
            }
        }
        //Checks left direction pushing
        if (player.XPos - 1 >= 0 &&
            gameArray[player.XPos - 1, player.YPos] != null &&
            gameArray[player.XPos - 1, player.YPos].GetType() == typeof(PushBlock) &&
            Input.GetKeyDown(KeyCode.A))
        {
            if (gameArray[player.XPos - 1, player.YPos].CanLeft)                        //Makes sure that the pushable block can move to the right without inteference
            {
                PushBlock(gameArray[player.XPos - 1, player.YPos], KeyCode.A);

            }
            else if (!gameArray[player.XPos - 1, player.YPos].CanLeft && gameArray[player.XPos - 2, player.YPos].GetType() == typeof(PullBlock))
            {
                Destroy(gameArray[player.XPos - 1, player.YPos].gameObject);
                Destroy(gameArray[player.XPos - 2, player.YPos].gameObject);
                CreateBlock(pushNPullBlock, player.XPos - 2, player.YPos);
            }
        }
        //Checks down direction pushing
        if (player.YPos + 1 < rowsNum - 1 &&
            gameArray[player.XPos, player.YPos + 1] != null &&
            gameArray[player.XPos, player.YPos + 1].GetType() == typeof(PushBlock) &&
            Input.GetKeyDown(KeyCode.S))
        {
            if (gameArray[player.XPos, player.YPos + 1].CanDown)                        //Makes sure that the pushable block can move to the right without inteference
            {
                PushBlock(gameArray[player.XPos, player.YPos + 1], KeyCode.S);
            }
            else if (!gameArray[player.XPos, player.YPos + 1].CanDown && gameArray[player.XPos, player.YPos + 2].GetType() == typeof(PullBlock))
            {
                Destroy(gameArray[player.XPos, player.YPos + 1].gameObject);
                Destroy(gameArray[player.XPos, player.YPos + 2].gameObject);
                CreateBlock(pushNPullBlock, player.XPos, player.YPos + 2);
            }
        }
        //Checks up direction pushing
        if (player.YPos - 1 >= 0 &&
            gameArray[player.XPos, player.YPos - 1] != null &&
            gameArray[player.XPos, player.YPos - 1].GetType() == typeof(PushBlock) &&
            Input.GetKeyDown(KeyCode.W))
        {
            if (gameArray[player.XPos, player.YPos - 1].CanUp)                        //Makes sure that the pushable block can move to the right without inteference
            {
                PushBlock(gameArray[player.XPos, player.YPos - 1], KeyCode.W);
            }
            else if (!gameArray[player.XPos, player.YPos - 1].CanUp && gameArray[player.XPos, player.YPos - 2].GetType() == typeof(PullBlock))
            {
                Destroy(gameArray[player.XPos, player.YPos - 1].gameObject);
                Destroy(gameArray[player.XPos, player.YPos - 2].gameObject);
                CreateBlock(pushNPullBlock, player.XPos, player.YPos - 2);
            }
        }
    }

    public void PlayerPullBlock()
    {
        if (player.XPos - 2 > 0 &&                                                          //Makes sure checked position is within range
            gameArray[player.XPos - 2, player.YPos] != null &&                              //
            gameArray[player.XPos - 2, player.YPos].GetType() == typeof(PullBlock) &&       //
            Input.GetKeyDown(KeyCode.D))                                                    //
        {
            PushBlock(gameArray[player.XPos - 2, player.YPos], KeyCode.D);
        }
        if (player.XPos + 2 < colsNum - 1 &&
            gameArray[player.XPos + 2, player.YPos] != null &&
            gameArray[player.XPos + 2, player.YPos].GetType() == typeof(PullBlock) &&
            Input.GetKeyDown(KeyCode.A))
        {
            PushBlock(gameArray[player.XPos + 2, player.YPos], KeyCode.A);
        }
        if (player.YPos - 2 > 0 &&
            gameArray[player.XPos, player.YPos - 2] != null &&
            gameArray[player.XPos, player.YPos - 2].GetType() == typeof(PullBlock) &&
            Input.GetKeyDown(KeyCode.S))
        {
            PushBlock(gameArray[player.XPos, player.YPos - 2], KeyCode.S);
        }
        if (player.YPos + 2 < rowsNum - 1 &&
            gameArray[player.XPos, player.YPos + 2] != null &&
            gameArray[player.XPos, player.YPos + 2].GetType() == typeof(PullBlock) &&
            Input.GetKeyDown(KeyCode.W))
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
    private void CreateBlock(Block item, int xPos, int yPos)
    {
        Block currentCube = Instantiate(item);
        if (currentCube.name == "Player(Clone)")
        {
            player = currentCube;
        }
        gameArray[xPos, yPos] = currentCube;
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
            gameArray[item.PrevX, item.PrevY] = null;
            gameArray[item.XPos, item.YPos] = item;
            item.PrevX = item.XPos;
            item.PrevY = item.YPos;
            item.transform.position = new Vector3(origin.x + spriteSize / 2 + item.XPos * spriteSize, origin.y - spriteSize / 2 - item.YPos * spriteSize, 1);
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
