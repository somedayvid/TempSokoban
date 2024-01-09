using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private float spriteSize = 1.0f;
    public int rowsNum;
    public int colsNum;

    public Block[,] gameArray;

    private bool canLeft;
    private bool canRight;
    private bool canTop;
    private bool canBot;

    private Vector3 origin;

    public Block player;

    public Block box;

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

        gameArray[player.XPos, player.YPos] = player;
        player.XPos = player.XPos;
        player.YPos = player.YPos;
        player.transform.position = new Vector3(origin.x + spriteSize / 2 + player.XPos * spriteSize, origin.y - spriteSize / 2 - player.YPos * spriteSize, 1);

        CreateBlock(box, 5, 5);
    }
    void Update()
    {
        BoundingArea();
        PlayerMoveBlock();
        PlayerMovement();

    }
    private void BoundingArea()
    {
        if (player.XPos - 1 < 0 || gameArray[player.XPos - 1, player.YPos] != null) 
        {
            canLeft = false;
        }
        else
        {
            canLeft = true;
        }
        if (player.YPos - 1 < 0 || gameArray[player.XPos, player.YPos - 1] != null)
        {
            canTop = false;
        }
        else
        {
            canTop = true;
        }
        if (player.XPos + 1 > colsNum - 1 || gameArray[player.XPos + 1, player.YPos] != null)
        {
            canRight = false;
        }
        else
        {
            canRight = true;
        }
        if (player.YPos + 1 > rowsNum - 1 || gameArray[player.XPos, player.YPos + 1] != null)
        {
            canBot = false;
        }
        else
        {
            canBot = true;
        }
    }

    public void PlayerMovement(){
        player.PrevX =  player.XPos;
        player.PrevY = player.YPos;

        if (Input.GetKeyDown(KeyCode.A) && canLeft)
        {
            player.XPos -= 1;
        }
        if (Input.GetKeyDown(KeyCode.S) && canBot)
        {
            player.YPos += 1;
        }
        if (Input.GetKeyDown(KeyCode.W) && canTop)
        {
            player.YPos -= 1;
        }
        if (Input.GetKeyDown(KeyCode.D) && canRight)
        {
            player.XPos += 1;
        }

        MoveUpdateBlock(player);
    }

    public void PlayerMoveBlock()
    {
        if (player.XPos + 1 < colsNum - 1 && 
            gameArray[player.XPos + 1, player.YPos] != null && 
            gameArray[player.XPos + 1, player.YPos].GetType() == typeof(Block) && 
            Input.GetKeyDown(KeyCode.D))
        {
            MoveUpdateBlock(gameArray[player.XPos + 1, player.YPos], KeyCode.D);
        }
        if (player.XPos - 1 >= 0 &&
            gameArray[player.XPos - 1, player.YPos] != null &&
            gameArray[player.XPos - 1, player.YPos].GetType() == typeof(Block) && 
            Input.GetKeyDown(KeyCode.A))
        {
            MoveUpdateBlock(gameArray[player.XPos - 1, player.YPos], KeyCode.A);
        }
        if (player.YPos + 1 < rowsNum - 1 &&
            gameArray[player.XPos, player.YPos + 1] != null &&
            gameArray[player.XPos, player.YPos + 1].GetType() == typeof(Block) && 
            Input.GetKeyDown(KeyCode.S))
        {
            MoveUpdateBlock(gameArray[player.XPos, player.YPos + 1], KeyCode.S);
        }
        if (player.YPos - 1 >= 0 &&
            gameArray[player.XPos, player.YPos - 1] != null &&
            gameArray[player.XPos, player.YPos - 1].GetType() == typeof(Block) && 
            Input.GetKeyDown(KeyCode.W))
        {
            MoveUpdateBlock(gameArray[player.XPos, player.YPos - 1], KeyCode.W);
        }
    }

    private void CreateBlock(Block item, int xPos, int yPos)
    {
        Block currentCube = Instantiate(item, new Vector3(origin.x + spriteSize / 2 + item.XPos * spriteSize, origin.y - spriteSize / 2 - item.YPos * spriteSize, 1), Quaternion.identity);
        gameArray[xPos, yPos] = currentCube;
        currentCube.PrevX = currentCube.XPos = xPos;
        currentCube.PrevY = currentCube.YPos = yPos;

    }

    private void MoveUpdateBlock(Block item)
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

    private void MoveUpdateBlock(Block item, KeyCode direction)
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

        MoveUpdateBlock(item);
    }

    private void OnDrawGizmos()
    {
        
    }
}
