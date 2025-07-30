using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

public class PlayerInputHandler : MonoBehaviour
{
    private Vector2 moveInput;
    [SerializeField]private PlayerInput playerInput;
    NetWorkInputData.NetworkEvents playerNetworkEvent = NetWorkInputData.NetworkEvents.None;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            switch (context.action.name)
            {
                case "move":
                    moveInput = context.ReadValue<Vector2>();
                    //Debug.Log("Move Input: " + moveInput);
                    break;
                case "dash":
                    playerNetworkEvent = NetWorkInputData.NetworkEvents.Dash;
                    break;
                case "skill1":
                    playerNetworkEvent = NetWorkInputData.NetworkEvents.Skill1;
                    break;
            }
        }
    }

    void OnEnable()
    {
        if (playerInput != null && playerInput.actions != null)
        {
            playerInput.actions["move"].performed += OnActionTriggered;
            playerInput.actions["move"].canceled += OnActionTriggered;
            playerInput.actions["dash"].performed += OnActionTriggered;
            playerInput.actions["dash"].canceled += OnActionTriggered;
            playerInput.actions["skill1"].performed += OnActionTriggered;
            playerInput.actions["skill1"].canceled += OnActionTriggered;
        }
    }

    void OnDisable()
    {
        if (playerInput != null && playerInput.actions != null)
        {
            playerInput.actions["move"].performed -= OnActionTriggered;
            playerInput.actions["move"].canceled -= OnActionTriggered;
            playerInput.actions["dash"].performed -= OnActionTriggered;
            playerInput.actions["dash"].canceled -= OnActionTriggered;
            playerInput.actions["skill1"].performed -= OnActionTriggered;
            playerInput.actions["skill1"].canceled -= OnActionTriggered;
        }
    }
    public NetWorkInputData GetNetworkInput()
    {
        var inputdata = new NetWorkInputData
        {
            moveInput_NetworkData = moveInput,
            playerEvent = playerNetworkEvent,
        };
        playerNetworkEvent = NetWorkInputData.NetworkEvents.None;
        return inputdata;

    }
}
