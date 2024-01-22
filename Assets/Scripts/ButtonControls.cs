using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControls : MonoBehaviour
{
    public LevelStorage levelStorage;
    public Canvas canvas;

    private bool levelSelectionScreen = true;

    public bool LevelSelectionScreen => levelSelectionScreen;

    public void Level1()
    {
        levelStorage.CurrentLevel = 1;
        canvas.gameObject.SetActive(false);
        levelSelectionScreen = false;
    }
    public void Level2()
    {
        levelStorage.CurrentLevel = 2;
        canvas.gameObject.SetActive(false);
        levelSelectionScreen = false;
    }
    public void Level3()
    {
        levelStorage.CurrentLevel = 3;
        canvas.gameObject.SetActive(false);
        levelSelectionScreen = false;
    }
    public void Level4()
    {
        levelStorage.CurrentLevel = 4;
        canvas.gameObject.SetActive(false);
        levelSelectionScreen = false;
    }
    public void Level5()
    {
        levelStorage.CurrentLevel = 5;
        canvas.gameObject.SetActive(false);
        levelSelectionScreen = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && levelStorage.CurrentLevel != 0)
        {
            if (levelSelectionScreen)
            {
                canvas.gameObject.SetActive(false);
                levelSelectionScreen = false;
            }
            else if(!levelSelectionScreen)
            {
                canvas.gameObject.SetActive(true);
                levelSelectionScreen = true;
            }
        }
    }
}
