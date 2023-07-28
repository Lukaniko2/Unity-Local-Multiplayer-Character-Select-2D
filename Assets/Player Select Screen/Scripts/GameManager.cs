using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SO_PlayerSelectEvents playerSelectEvents;
    [SerializeField] private GameObject pauseMenuPrefab;

    public static bool isGamePaused;

    private PlayerInput[] playerInputs;


    private void Awake()
    {
        
        //Calls when a player presses the pause button
        playerSelectEvents.pauseGameEvent.AddListener(PauseTheGame);

        //Calls whenever a player presses the resume button
        playerSelectEvents.resumeGameEvent.AddListener(ResumeTheGame);
    }

    private void Start()
    {
        //find all players in the game. May need to move inside function if errors when someone unpluggs controller
        playerInputs = GameObject.FindObjectsOfType<PlayerInput>();
    }
    private void PauseTheGame()
    {
        //Get a reference to all playerInputs
        foreach(PlayerInput input in playerInputs)
        {
            input.SwitchCurrentActionMap("UI");
        }

        //Spawn in the pause menu
        GameObject pauseMenu = Instantiate(pauseMenuPrefab);

        //connect all the player's inputs to that pause menu's input module
        pauseMenu.GetComponent<PauseMenuController>().ConnectControllersToPauseMenu(playerInputs);

        Debug.Log("PAUSED GAME");
        Time.timeScale = 0;
    }

    private void ResumeTheGame()
    {
        //Get a reference to all playerInputs
        foreach (PlayerInput input in playerInputs)
        {
            input.SwitchCurrentActionMap("Player");
        }


        Debug.Log("RESUME GAME");
        Time.timeScale = 1;
    }
}
