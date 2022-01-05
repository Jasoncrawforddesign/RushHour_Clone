using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    public GameObject gameController;


    // MAIN MENU UI PANEL BELOW
    public void playButtonPressed()
    {
        //tell game controller to load puzzle
        //disable Main menu panel
        Debug.Log("Play Game!");

    }

    public void exitGameButton()
    {
        Debug.Log("Exit Game!");
        // close game/application
    }



    // IN GAME UI PANEL BELOW
    public void exitPuzzleButton()
    {
        // when player exits current puzzle, remove last puzzle
        // set main menu canvas active
    }

 
    public void resetPuzzleButton()
    {
        gameController.GetComponent<GameController>().resetPuzzle();
        //reset current puzzle
    }
}
