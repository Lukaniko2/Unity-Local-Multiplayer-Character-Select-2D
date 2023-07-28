using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;


public class PlayerConfigurationManager : MonoBehaviour
{
    /// <summary>
    /// Central script any scene can access to get player configs
    /// Player Inputs, Colour, player number
    /// </summary>

    //any script can access each player's configurations
    public static PlayerConfigurationManager Instance { get; private set; }

    private List<PlayerConfiguration> playerConfigs;

    public List<PlayerConfiguration> PlayerConfigs
    {
        get => playerConfigs;
        set => playerConfigs = value;
    }

    //Variables
    //At the end, when everyone readies up, start a timer to get into game
    [SerializeField] private int readyTimer = 3;

    private Coroutine storedCoroutine;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(Instance);

        PlayerConfigs = new List<PlayerConfiguration>(); //initialize List
    }

    public void SetPlayerColour(int index, Color colour)
    {
        PlayerConfigs[index].Colour = colour;
    }

    public void ReadyPlayer(int index)
    {
        PlayerConfigs[index].IsReady = true;

        Debug.Log(PlayerConfigs[index].ControlScheme + " IS READY");

        //Checks to see if all current players are ready, and if so, start a countdown timer to enter game
        if (PlayerConfigs.All(p => p.IsReady == true))
        {
            storedCoroutine = StartCoroutine(StartTimer());
        }
    }

    public void StopTimer()
    {
        if(storedCoroutine != null) 
            StopCoroutine(storedCoroutine);
    }

    public void CancelReadyPlayer(int index)
    {
        PlayerConfigs[index].IsReady = false;
        Debug.Log(index + " IS NOT READY ANYMORE");
        StopTimer();
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void DestroyTheManager()
    {
        //I need this because if I have the same Player Input Manager, it remembers ALL previous players
        //and I won't be allowed to press any button to spawn a new player in.
        //So when the level is loaded I instantiate a new configuration game Object and destroy the old one.

        //Once you implement this to a game where the in-game scene resets and you need to remember the player configs,
        //you should destroy this gameObject before you load in the menu scene when the game is over (not the round)
        Destroy(gameObject);
    }

    //Starts a countdown timer for when all players are ready to transition to next scene
    IEnumerator StartTimer()
    {
        for(int i = readyTimer; i > 0; i--)
        {
            yield return new WaitForSeconds(1f);

            //Update Text on Screen if there is one
        }
        SceneManager.LoadScene(2);
        yield return null;
    }
}

public class PlayerConfiguration
{
    //Constructor that takes in player input
    public PlayerConfiguration(PlayerInput pi)
    {
        Input = pi;
        PlayerIndex = pi.playerIndex;
        ControlScheme = pi.currentControlScheme;
        PlayerInputDevice = pi.GetDevice<InputDevice>();
    }

    public PlayerInput Input { get; set; }

    public int PlayerIndex { get; set; }

    //after player select colour, they say if they're ready
    public bool IsReady { get; set; }

    public Color Colour { get; set; }

    public string ControlScheme { get; set; }

    public InputDevice PlayerInputDevice { get; set; }

    //For the In-game rounds if we need to keep track of player performance
    public int CurrentPlacement { get; set; }
}
