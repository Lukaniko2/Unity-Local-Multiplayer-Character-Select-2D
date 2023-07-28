using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class PlayerSetupMenuController : MonoBehaviour
{
    /// <summary>
    /// In Charge of Collecting the data of which settings the player presses and storing it in the PlayerConfigurations
    /// </summary>

    //Components
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private GameObject PressToJoinPanel;

    [SerializeField] private GameObject menuPanel;

    [SerializeField] private GameObject readyPanel;

    [SerializeField] private Button readyButton;

    [SerializeField] private Button cancelButton; // to cancel the ready up

    //Variables

    //So players can't immediately select colour accidentally when they perform an input to open the menu
    [SerializeField] private float ignoreInputTime = 0.5f;

    private int playerIndex;

    private bool inputEnabled;

    public void SetPlayerIndex(int pi)
    {
        playerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());

        IgnoreInput();

        //stop the next scene from loading since new player joined. Wait for all players to ready up
        PlayerConfigurationManager.Instance.StopTimer();

        //Set their colour to a default at first
        PlayerConfigurationManager.Instance.SetPlayerColour(playerIndex, new Color(0.1921569f, 0.5333334f, 1f, 1f));

        //spawn a player with the position of the "player transform"
        Transform playerTransform = transform.GetChild(transform.childCount - 1).transform;
        GameObject player = Instantiate(playerPrefab, playerTransform, playerTransform);
        player.transform.localPosition = Vector3.zero;
    }

    public void ChangeToMenuNEWSetupPanels()
    {
        //From the 'SpawnPlayerSetupMenu' Script. When the input is detected we switch it to menu panel
        PressToJoinPanel.SetActive(false);
        menuPanel.SetActive(true);
        titleText.gameObject.SetActive(true);
    }
    public void SetColour()
    {
        //If a colour button was selected, we set the player's colour to the selected one
        if (!inputEnabled)
            return;

        //Get the button that the player clicked on 
        Button b = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        //Get the normal colour of the button
        ColorBlock cb = b.colors;
        Color colour = cb.normalColor;

        //Set the player's colour to the one we clicked on
        PlayerConfigurationManager.Instance.SetPlayerColour(playerIndex, colour);

        //Now hover over the Ready Button
        //readyButton.Select();


        //Change the colour of the Player Prefab that's a child of this game object
        //This gets the player transform's child, which is the player
        GameObject playerPrefab = transform.GetChild(transform.childCount - 1).transform.GetChild(0).gameObject;
        playerPrefab.GetComponent<SpriteRenderer>().color = colour;
    }

    public void ReadyPlayer()
    {
        //We ready the player up if they clicked the ready button
        if (!inputEnabled)
            return;

        //sets the player to ready
        PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);

        //makes the ready screen visible and so they can't perform any more inputs
        menuPanel.SetActive(false);
        readyPanel.SetActive(true);

        //some scenes don't have a cencel button, so first check to see if the scene has it
        if(cancelButton != null)
            cancelButton.Select();

        IgnoreInput();
    }

    public void CancelReady()
    {
        PlayerConfigurationManager.Instance.CancelReadyPlayer(playerIndex);

        readyPanel.SetActive(false);
        menuPanel.SetActive(true);

        readyButton.Select();
        IgnoreInput();
    }

    private void IgnoreInput()
    {
        //ignore inputs for a certain amount of time
        inputEnabled = false;
        Invoke("EnableInputs", ignoreInputTime);
    }

    private void EnableInputs()
    {
        inputEnabled = true;
    }


}
