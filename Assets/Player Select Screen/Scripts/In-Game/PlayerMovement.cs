using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    private InputManager inputManager;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
    }
    
    void Update()
    {
        //Moving the player
        transform.position += (Vector3)inputManager.MoveInput * Time.deltaTime * speed;
    }
}
