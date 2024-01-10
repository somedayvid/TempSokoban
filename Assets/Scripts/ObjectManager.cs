using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private float spriteSize = 1.0f;
    private int rowsNum;
    private int colsNum;

    private Block[,] gameArray;

    private Vector3 origin;

    public Block player;

    public Block box;

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

        CreateBlock(player, 0, 0);

        CreateBlock(box, 5, 5);
        CreateBlock(box, 2,2);
    }
    void Update()
    {
        PlayerMoveBlock();
        PlayerMovement();
    }

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

        MoveUpdateBlock(player);
    }

    public void PlayerMoveBlock()
    {
        if (player.XPos + 1 < colsNum - 1 && 
            gameArray[player.XPos + 1, player.YPos] != null && 
            gameArray[player.XPos + 1, player.YPos].GetType() == typeof(Block) && 
            Input.GetKeyDown(KeyCode.D) &&
            gameArray[player.XPos + 1, player.YPos].CanRight)
        {
            MoveUpdateBlock(gameArray[player.XPos + 1, player.YPos], KeyCode.D);
        }
        if (player.XPos - 1 >= 0 &&
            gameArray[player.XPos - 1, player.YPos] != null &&
            gameArray[player.XPos - 1, player.YPos].GetType() == typeof(Block) && 
            Input.GetKeyDown(KeyCode.A) &&
            gameArray[player.XPos - 1, player.YPos].CanLeft)
        {
            MoveUpdateBlock(gameArray[player.XPos - 1, player.YPos], KeyCode.A);
        }
        if (player.YPos + 1 < rowsNum - 1 &&
            gameArray[player.XPos, player.YPos + 1] != null &&
            gameArray[player.XPos, player.YPos + 1].GetType() == typeof(Block) && 
            Input.GetKeyDown(KeyCode.S) &&
            gameArray[player.XPos, player.YPos + 1].CanDown)
        {
            MoveUpdateBlock(gameArray[player.XPos, player.YPos + 1], KeyCode.S);
        }
        if (player.YPos - 1 >= 0 &&
            gameArray[player.XPos, player.YPos - 1] != null &&
            gameArray[player.XPos, player.YPos - 1].GetType() == typeof(Block) && 
            Input.GetKeyDown(KeyCode.W) &&
            gameArray[player.XPos, player.YPos - 1].CanUp)
        {
            MoveUpdateBlock(gameArray[player.XPos, player.YPos - 1], KeyCode.W);
        }
    }

    private void CreateBlock(Block item, int xPos, int yPos)
    {
        Block currentCube = Instantiate(item);
        gameArray[xPos, yPos] = currentCube;
        currentCube.XPos = xPos;
        currentCube.YPos = yPos;
        MoveUpdateBlock(currentCube);
        if(item.name == "Player")
        {
            player = item;
        }
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
