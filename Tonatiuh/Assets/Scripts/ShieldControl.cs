using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ShieldControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform m_ShieldSocket;
    [SerializeField] private Transform m_ShieldModel;

    private float m_VerticalOffset = 5f;

    private PlayerInputActions m_PlayerControls;
    private InputAction m_IThrowShield;
    private InputAction m_IRecalShield;

    private void Awake()
    {
        m_PlayerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        m_IThrowShield = m_PlayerControls.Player.ShieldThrow;
        m_IThrowShield.Enable();
        m_IThrowShield.performed += Throw;

        m_IRecalShield = m_PlayerControls.Player.ShieldRecal;
        m_IRecalShield.Enable();
        m_IRecalShield.performed += Recal;
    }

    private void OnDisable()
    {
        m_IThrowShield.Disable();
        m_IRecalShield.Disable();
    }

    private void Throw(InputAction.CallbackContext context)
    {
        Debug.Log("Throw");
        m_ShieldModel.localPosition = new Vector3(0f, -m_VerticalOffset, 0f);
    }

    private void Recal(InputAction.CallbackContext context)
    {
        Debug.Log("Recal");
        m_ShieldModel.localPosition = new Vector3(0f, 0f, 0f);
    }
}
