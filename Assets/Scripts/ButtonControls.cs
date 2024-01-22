using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControls : MonoBehaviour
{
    public LevelStorage levelStorage;
    public Canvas canvas;

    private bool levelSelectionScreen = true;

    public bool LevelSelectionScreen => levelSelectionScreen;

    public void Level1()
    {
        levelStorage.CurrentLevel = 1;
        levelStorage.LevelLoaded = false;
        canvas.gameObject.SetActive(false);
        levelSelectionScreen = false;
    }
    public void Level2()
    {
        levelStorage.CurrentLevel = 2;
        levelStorage.LevelLoaded = false;
        canvas.gameObject.SetActive(false);
        levelSelectionScreen = false;
    }
    public void Level3()
    {
        levelStorage.CurrentLevel = 3;
        levelStorage.LevelLoaded = false;
        canvas.gameObject.SetActive(false);
        levelSelectionScreen = false;
    }
    public void Level4()
    {
        levelStorage.CurrentLevel = 4;
        levelStorage.LevelLoaded = false;
        canvas.gameObject.SetActive(false);
        levelSelectionScreen = false;
    }
    public void Level5()
    {
        levelStorage.CurrentLevel = 5;
        levelStorage.LevelLoaded = false;
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
