using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    protected int xPos;
    protected int yPos;

    protected int prevX;
    protected int prevY;

    protected bool canUp;
    protected bool canDown;
    protected bool canRight;
    protected bool canLeft;

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
}
