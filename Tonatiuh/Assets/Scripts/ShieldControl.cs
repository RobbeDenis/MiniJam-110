using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ShieldControl : MonoBehaviour
{
    [Header("Aim settings")]
    [SerializeField] private float m_MaxAimDistance = 100f;
    [SerializeField] private LayerMask m_IgnoreMask;

    [Header("Layer masks")]
    [SerializeField] private LayerMask m_PlayerMask;
    [SerializeField] private LayerMask m_ShieldMask;

    [Header("References")]
    [SerializeField] private Shield m_Shield;
    [Space(5)]
    [SerializeField] private Transform m_ShieldSocket;
    [SerializeField] private Transform m_ShieldModel;
    [SerializeField] private Transform m_CameraTransform;

    private float m_VerticalOffset = 5f;
    private bool m_Thrown = false;

    private Transform m_OrbitTarget;
    private bool m_CanOrbit = true;

    private PlayerInputActions m_PlayerControls;
    private InputAction m_IThrowShield;
    private InputAction m_IRecalShield;
    private InputAction m_IOrbitShield;

    private void Awake()
    {
        m_PlayerControls = new PlayerInputActions();
        m_Shield.SetController(this);
    }

    private void OnEnable()
    {
        m_IThrowShield = m_PlayerControls.Player.ShieldThrow;
        m_IThrowShield.Enable();
        m_IThrowShield.performed += Throw;

        m_IRecalShield = m_PlayerControls.Player.ShieldRecal;
        m_IRecalShield.Enable();
        m_IRecalShield.performed += Recall;

        m_IOrbitShield = m_PlayerControls.Player.ShieldOrbit;
        m_IOrbitShield.Enable();
        m_IOrbitShield.performed += StartOrbit;
    }

    private void OnDisable()
    {
        m_IThrowShield.Disable();
        m_IRecalShield.Disable();
        m_IOrbitShield.Disable();
    }

    private void FixedUpdate()
    {
        LookForOrbit();
    }

    private void Throw(InputAction.CallbackContext context)
    {
        if (m_Thrown)
            return;

        m_ShieldModel.localPosition = new Vector3(0f, -m_VerticalOffset, 0f);
        m_Shield.transform.position = m_ShieldSocket.position;

        Vector3 direction;
        RaycastHit hit;
        if(Physics.Raycast(m_CameraTransform.position + m_CameraTransform.forward * 2f, m_CameraTransform.forward, out hit, m_MaxAimDistance))
        {
            direction = Vector3.Normalize(hit.point - m_ShieldSocket.position);
        }
        else
        {
            direction = Vector3.Normalize((m_CameraTransform.position + m_CameraTransform.forward * m_MaxAimDistance) - m_ShieldSocket.position);
        }

        m_Shield.Throw(direction);
        m_Thrown = true;
    }

    private void Recall(InputAction.CallbackContext context)
    {
        if (!m_Thrown)
            return;

        m_Shield.Recall(m_ShieldSocket);
    }

    private void LookForOrbit()
    {
        if (!m_CanOrbit)
            return;

        RaycastHit hit;
        if (Physics.Raycast(m_CameraTransform.position + m_CameraTransform.forward * 2f, m_CameraTransform.forward, out hit, m_MaxAimDistance, m_IgnoreMask))
        {
            Debug.DrawLine(m_CameraTransform.position + m_CameraTransform.forward * 2f, hit.point, Color.red);
            if(hit.collider.gameObject.tag == "Enemy")
            {
                m_OrbitTarget = hit.transform;
            }
            else
            {
                m_OrbitTarget = null;
            }
        }
    }

    private void StartOrbit(InputAction.CallbackContext context)
    {
        if(m_CanOrbit && m_OrbitTarget)
        {
            m_Shield.GoOrbit(m_OrbitTarget);
            m_CanOrbit = false;
            if(!m_Thrown)
            {
                m_ShieldModel.localPosition = new Vector3(0f, -m_VerticalOffset, 0f);
                m_Shield.transform.position = m_ShieldSocket.position;
                m_Thrown = true;
            }
        }
    }

    public void ShieldArrived()
    {
        m_Thrown = false;
        m_CanOrbit = true;
        m_ShieldModel.localPosition = new Vector3(0f, 0f, 0f);
    }

    public Transform GetSocket()
    {
        return m_ShieldSocket;
    }
}
