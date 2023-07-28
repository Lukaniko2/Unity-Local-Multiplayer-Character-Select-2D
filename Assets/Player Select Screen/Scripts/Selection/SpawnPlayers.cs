using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPlayers : MonoBehaviour
{
    /// <summary>
    /// This Script is in the GAME SCENE and is in charge of spawning in the players based on their configurations
    /// selected in the setup menu. 
    /// We're still using the PlayerInputManager here, so we spawn the player prefab and join players through it
    /// </summary>
    
    //Components
    [SerializeField] private Transform[] spawnTransforms;
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        PlayerConfiguration[] playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();

        //Loop through the amount of players we have in the player configs and spawn them at spawnpoints
        for(int i = 0; i < playerConfigs.Length; i++)
        {
            //Get the player's configs
            PlayerConfiguration pc = playerConfigs[i];

            //JOIN THE PLAYER AND CHANGE THE PLAYER PREFAB (NEED TO CHANGE BACK TO THE CONFIGURATION MANAGER)
            //This was the problem, I now join the player instead of instantiating the player prefab
            PlayerInputManager.instance.playerPrefab = playerPrefab;

            PlayerInput playerInput = PlayerInputManager.instance.JoinPlayer(i, -1, pc.ControlScheme, pc.PlayerInputDevice);
            SetupSpawnDetails(playerInput, playerConfigs);
        }
    }

    private void SetupSpawnDetails(PlayerInput playerInput, PlayerConfiguration[] playerConfigs)
    {
        GameObject playerGO = playerInput.gameObject;

        //set parent
        playerGO.transform.parent = gameObject.transform;

        //spawn location
        playerGO.transform.position = spawnTransforms[playerInput.playerIndex].position;
        playerGO.transform.rotation = spawnTransforms[playerInput.playerIndex].rotation;

        //Setting the Player's colour and options they chose
        playerGO.GetComponent<InputManager>().InitializePlayerDetails(playerConfigs[playerInput.playerIndex]);
    }

}
