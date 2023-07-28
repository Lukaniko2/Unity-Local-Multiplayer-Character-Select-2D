using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private SO_PlayerSelectEvents playerSelectEvents;

    //Variables
    private Vector2 moveInput;

    public Vector2 MoveInput
    {
        get => moveInput;
        set => moveInput = value;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void InitializePlayerDetails(PlayerConfiguration pc)
    {
        //Set the colour and any other setting we chose in the setup menu when the player is spawned
        gameObject.GetComponent<SpriteRenderer>().color = pc.Colour;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        //Send event to pause the game and switch input action maps
        if(context.performed)
            playerSelectEvents.PauseGameEventSend();
    }
    
    public void OnResumeGame(InputAction.CallbackContext context)
    {
        if (context.performed)
            playerSelectEvents.ResumeGameEventSend();
    }
    


}
