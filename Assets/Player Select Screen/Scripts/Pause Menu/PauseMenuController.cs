using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private SO_PlayerSelectEvents playerSelectEvents;

    [SerializeField] private Button resumeButton;
    
    private void Awake()
    {
        resumeButton.Select();
    }

    public void ConnectControllersToPauseMenu(PlayerInput[] players)
    {
        //Get all the playuer's inputUI Modules and connect them to this UI input module in child
        foreach (PlayerInput player in players)
        {
            player.uiInputModule = GetComponentInChildren<InputSystemUIInputModule>();
        }
    }

    public void ResumeGameButton()
    {
        //Send event to the game manager to resume the game
        playerSelectEvents.ResumeGameEventSend();

        //remove this pause menu
        Destroy(transform.root.gameObject);
    }

    public void CharacterSelectButton()
    {
        Time.timeScale = 1;

        //remove this pause menu
        Destroy(transform.root.gameObject);

        PlayerConfigurationManager.Instance.DestroyTheManager();
        SceneManager.LoadScene(Random.Range(0, 2));
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
