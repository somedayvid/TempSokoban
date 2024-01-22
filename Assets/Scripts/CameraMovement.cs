using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public ObjectManager manager;
    public LevelStorage levelStorage;

    // Update is called once per frame
    void Update()
    {
        PlayerQuadrantFollow();
    }

    private void PlayerQuadrantFollow()
    {
        if (levelStorage.LevelLoaded)
        {
            if (manager.PlayerX > Camera.main.orthographicSize * Camera.main.aspect)
            {
                transform.position = new Vector3(Camera.main.orthographicSize * Camera.main.aspect * 2, transform.position.y);
            }
            else if (manager.PlayerX < Camera.main.orthographicSize * Camera.main.aspect * 2)
            {
                transform.position = Vector3.zero;
            }
        }
    }

}
