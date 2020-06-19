using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public  bool pause = false;
    // mettre auto au start
    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdatePause();
    }


    public void UpdatePause()
    {
        if (Input.GetButtonDown("Pause"))
            pause = !pause;

        if (pause)
            PauseGame();
        else
            UnpauseGame();
    }

    public void PauseGame()
    {
        //Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        //Cursor.visible = false;
        Time.timeScale = 1f;
    }

}
