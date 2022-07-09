using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPVLook : MonoBehaviour
{
    [Header("Sensitivity")]
    [SerializeField] private float m_SensX = 2f;
    [SerializeField] private float m_SensY = 2f;
    private float m_Multiplier = 0.1f;

    [Header("References")]
    [SerializeField] private Transform m_CameraHolderTransform;
    [SerializeField] private Transform m_Orientation;

    private Vector2 m_InputLook;

    private float m_Yaw;
    private float m_Pitch;

    private PlayerInputActions m_PlayerControls;
    private InputAction m_ILook;

    private void OnEnable()
    {
        m_PlayerControls = new PlayerInputActions();

        m_ILook = m_PlayerControls.Player.Look;
        m_ILook.Enable();
    }

    private void OnDisable()
    {
        m_ILook.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleInput();

        m_Orientation.rotation = Quaternion.Euler(0, m_Yaw, 0);
        m_CameraHolderTransform.transform.localRotation = Quaternion.Euler(m_Pitch, m_Yaw, 0);
    }

    private void HandleInput()
    {
        m_InputLook = m_ILook.ReadValue<Vector2>();

        m_Yaw += m_InputLook.x * m_SensX * m_Multiplier;
        m_Pitch -= m_InputLook.y * m_SensY * m_Multiplier;

        m_Pitch = Mathf.Clamp(m_Pitch, -90f, 90f);
    }
}
