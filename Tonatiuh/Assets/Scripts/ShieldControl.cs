using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ShieldControl : MonoBehaviour
{
    [Header("Aim settings")]
    [SerializeField] private float m_MaxAimDistance = 100f;

    [Header("References")]
    [SerializeField] private Shield m_Shield;
    [SerializeField] private Transform m_ShieldSocket;
    [SerializeField] private Transform m_ShieldModel;
    [SerializeField] private Transform m_CameraTransform;

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
        m_IRecalShield.performed += Recall;
    }

    private void OnDisable()
    {
        m_IThrowShield.Disable();
        m_IRecalShield.Disable();
    }

    private void Throw(InputAction.CallbackContext context)
    {
        m_ShieldModel.localPosition = new Vector3(0f, -m_VerticalOffset, 0f);
        m_Shield.transform.position = m_ShieldSocket.position;

        Vector3 direction;
        RaycastHit hit;
        if(Physics.Raycast(m_CameraTransform.position, m_CameraTransform.forward, out hit, m_MaxAimDistance))
        {
            direction = Vector3.Normalize(hit.point - m_ShieldSocket.position);
        }
        else
        {
            direction = Vector3.Normalize((m_CameraTransform.position + m_CameraTransform.forward * m_MaxAimDistance) - m_ShieldSocket.position);
        }

        m_Shield.Throw(direction);
    }

    private void Recall(InputAction.CallbackContext context)
    {
        m_ShieldModel.localPosition = new Vector3(0f, 0f, 0f);

        m_Shield.Recall(m_ShieldSocket);
    }
}
