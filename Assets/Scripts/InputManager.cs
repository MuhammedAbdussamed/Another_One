using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Input Assets")]
    [SerializeField] private InputActionAsset playerInput;

    // Action Maps
    private InputActionMap interaction;

    // Actions
    internal InputAction tap;

    void Start()
    {
        playerInput.Enable();

        interaction = playerInput.FindActionMap("Interaction");
        tap = interaction.FindAction("Tap");
    }

}
