using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
    /// <summary>
    /// In charge of spawning the playerSetup Menu. 
    /// </summary>

    //Components
    [SerializeField] private SO_PlayerSelectEvents playerSelectEvents;

    [SerializeField] private PlayerInput input;

    [SerializeField] private GameObject playerSetupMenuPrefab;

    //Variables
    private bool instantiateNewSetupPanel;
    public bool InstantiateNewSetupPanel 
    { 
        get => instantiateNewSetupPanel; 
        set => instantiateNewSetupPanel = value; 
    }

    private void Awake()
    {
        int playerNumber = input.playerIndex;

        GameObject gridLayout = GameObject.FindGameObjectWithTag("Setup Panels");

        //Find the Canvas GameObject and make this a child of it for organization
        //You cannot make this a child of the Player Configuration Manager or else it will mess up with player indexes in game
        Transform canvas = GameObject.FindObjectOfType<Canvas>().transform;
        transform.parent = canvas.transform;

        if(gridLayout != null)
        {
            //Spawn in the player setup menu
            GameObject menu = gridLayout.transform.GetChild(playerNumber).gameObject;

            //sets the playerInput to be able to move to only it's designated UI.
            //An object in the menu's children has a InputSystem Event Handler, and we are getting anything attached to that canvas
            input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();

            //Now for the title text at the top of each box we're setting the playerIndex to the associated playerIndex
            //found in the PlayerInput component that's attached to this object
            PlayerSetupMenuController controller = menu.GetComponent<PlayerSetupMenuController>();
            controller.SetPlayerIndex(input.playerIndex);

            //Hide the "Press to Join" and reveal menu
            controller.ChangeToMenuNEWSetupPanels();

            if (instantiateNewSetupPanel)
            {
                //now we instantiate a new panel that will listen for the player's input (Like Minecraft dungeons)
                Instantiate(playerSetupMenuPrefab, gridLayout.transform);
            }
        }


    }


}
