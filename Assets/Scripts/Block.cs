using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private ObjectManager arrayContainer;
    private LevelStorage levelStorage;

    private int xPos;
    private int yPos;
   
    private int prevX;
    private int prevY;
    
    private bool canUp;
    private bool canDown;
    private bool canRight;
    private bool canLeft;

    public int XPos
    {
        get { return xPos; }
        set { xPos = value; }
    }

    public int YPos
    {
        get { return yPos;}
        set { yPos = value; }
    }

    public int PrevX
    { 
        get { return prevX; } 
        set { prevX = value; }  
    }

    public int PrevY
    {
        get { return prevY; }
        set { prevY = value; }
    }

    public bool CanUp
    {
        get { return canUp; }
        set { canUp = value; }
    }

    public bool CanDown
    {
        get { return canDown; }
        set { canDown = value; }
    }

    public bool CanRight
    {
        get { return canRight; }
        set { canRight = value; }
    }

    public bool CanLeft
    {
        get { return canLeft; }
        set { canLeft = value; }
    }

    private void Awake()
    {
        prevX = -1;
        prevY = -1;
        xPos = 0;
        yPos = 0;
        arrayContainer = GameObject.Find("StageManager").GetComponent<ObjectManager>();
        levelStorage = GameObject.Find("LevelManager").GetComponent<LevelStorage>();
    }

    protected virtual void Update()
    {
        if (xPos + 1 > levelStorage.Cols - 1 || arrayContainer.GameArray[xPos + 1, YPos] != null)
        {
            canRight = false;
        }
        else canRight = true;
        if (xPos - 1 < 0 || arrayContainer.GameArray[xPos - 1, YPos] != null)
        {
            canLeft = false;
        }
        else canLeft = true;
        if (yPos - 1 < 0 || arrayContainer.GameArray[xPos, YPos - 1] != null)
        {
            canUp = false;
        }
        else canUp = true;
        if (yPos + 1 > levelStorage.Rows - 1 || arrayContainer.GameArray[xPos, YPos + 1] != null)
        {
            canDown = false;
        }
        else canDown = true;
    }
}
