using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinManager : MonoBehaviour
{
    /// <summary>
    /// For Joining the player's setup panels in the "Player Select" scene
    /// </summary>
  
    //Scripts
    private PlayerConfigurationManager playerConfigManager;

    //Variables
    [SerializeField] private GameObject playerSetupPrefab;

    [SerializeField] private bool instantiateNewSetupPanel = false;

    [Space]

    //Input Actions to Start Selecting Character. Kind of Like "Press 'A' to join"
    [SerializeField] private InputAction playerWASDJoin;

    [SerializeField] private InputAction playerArrowJoin;

    [SerializeField] private InputAction playerControllerJoin;

    

    private int activePlayerCount = 0; //for increasing the index of manual players joined. The "JoinedPlayer" funct.

    //make sure that the keybaord can only join  1 WSAD player and 1 arrow keys and no more
    private bool alreadyJoinedWASD; 
    private bool alreadyJoinedArrow;

    private void Awake()
    {
        playerConfigManager = GetComponent<PlayerConfigurationManager>();

        //Enable the input actions so we can detect manual input for joining
        playerWASDJoin.Enable();
        playerArrowJoin.Enable();
        playerControllerJoin.Enable();

        //WE need to ignore inputs for a bit because from the pause menu if we load the character select screen,
        //the player is instantly joined in since the same button input is registered
        Invoke("IgnoreInputs", 0.5f);
    }
    private void IgnoreInputs()
    {
        //When players DO click a button in the input action (WASD or Arrows), the JoinWASD or JoinArrow function is called
        //subscribe to these functions when event is registered
        playerWASDJoin.started += JoinWASD;
        playerArrowJoin.started += JoinArrow;
        playerControllerJoin.started += JoinController;
    }
    private void Start()
    {
        //Set the gameObject to instantiate
        PlayerInputManager.instance.playerPrefab = playerSetupPrefab;
    }
    //We have to unsubscribe from the events when we load a new scene or else we get weird duplication bug
    private void OnDisable()
    {
        playerWASDJoin.performed -= JoinWASD;
        playerArrowJoin.performed -= JoinArrow;
        playerControllerJoin.performed -= JoinController;

        playerWASDJoin.Disable();
        playerArrowJoin.Disable();
        playerControllerJoin.Disable();
    }

    public void OnPlayerJoined (PlayerInput playerInput)
    {
        //Since we're Sending Messages, this specific event 'OnPlayerJoined' gets called when players join
        activePlayerCount++;

        //set the player prefab as a child of the input manager so it's  inputs carry over between scenes
        playerInput.transform.parent = transform;

        //Add the player to the List of Player Configurations
        playerConfigManager.PlayerConfigs.Add(new PlayerConfiguration(playerInput));

        //telling script if we want to instantiate new setup panel or not. 
        //This all depends on what type of join system you want to go for.
        //'Player Select 4' scene DOES NOT instantiate since they're all there. 'Player Select 3' scene DOES instantiate
        if(PlayerInputManager.instance.playerPrefab == playerSetupPrefab)
            playerInput.gameObject.GetComponent<SpawnPlayerSetupMenu>().InstantiateNewSetupPanel = instantiateNewSetupPanel;
    }

    private void JoinWASD(InputAction.CallbackContext context)
    {
        //Make sure that there's only one of this player
        if (alreadyJoinedWASD)
            return;
        alreadyJoinedWASD = true;

        //Manually join them using the JoinPlayer function. Set control Scheme to "WASD"
        PlayerInputManager.instance.JoinPlayer(activePlayerCount, -1, "WASD", Keyboard.current);
    }

    private void JoinArrow(InputAction.CallbackContext context)
    {
        //Make sure that there's only one of this player
        if (alreadyJoinedArrow)
            return;
        alreadyJoinedArrow = true;
   
        //Manually join them using the JoinPlayer function. Set control Scheme to "Arrow Keys"
        PlayerInputManager.instance.JoinPlayer(activePlayerCount, -1, "Arrow Keys", Keyboard.current);
    }

    private void JoinController(InputAction.CallbackContext context)
    {
        //Joining any controller. Creates new player for new controllers plugged in
        PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
    }
}
