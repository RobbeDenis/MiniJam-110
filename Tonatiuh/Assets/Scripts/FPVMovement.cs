using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPVMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float m_MoveSpeed = 6f;
    [SerializeField] private float m_MovementMultiplier = 10f;
    [SerializeField] private float m_DefaultDrag = 6f;
    [SerializeField] private float m_JumpForce = 6f;

    [Header("Ground detection")]
    [SerializeField] private float m_GroundDetectionDistance = 0.4f;
    [SerializeField] private LayerMask m_GroundMask;

    [Header("References")]
    [SerializeField] private Transform m_Orientation;

    [Header("Air properties")]
    [SerializeField] private float m_AirDrag = 0.2f;
    [SerializeField] private float m_AirMultiplier = 0.1f; 

    private Vector3 m_MoveDirection;
    private Vector3 m_InputDirection;
    private Vector3 m_SlopeMoveDirection;
    private Rigidbody m_RigidBody;

    private bool m_IsGrounded;
    private RaycastHit m_SlopeHit;

    private PlayerInputActions m_PlayerControls;
    private InputAction m_IMove;
    private InputAction m_IJump;

    private void Awake()
    {
        m_PlayerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        m_IMove = m_PlayerControls.Player.Move;
        m_IMove.Enable();

        m_IJump = m_PlayerControls.Player.Jump;
        m_IJump.Enable();
        m_IJump.performed += Jump;
    }

    private void OnDisable()
    {
        m_IMove.Disable();
        m_IJump.Disable();
    }

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_RigidBody.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        HandleState();
        HandleInput();
        HandleDrag();
        HandleSlopes();
    }

    private void HandleInput()
    {
        // Move
        m_InputDirection = m_IMove.ReadValue<Vector2>();
        m_MoveDirection = m_Orientation.forward * m_InputDirection.y + m_Orientation.right * m_InputDirection.x;
    }

    private void HandleDrag()
    {
        m_RigidBody.drag = m_IsGrounded ? m_DefaultDrag : m_AirDrag;
    }

    private void HandleState()
    {
        m_IsGrounded = Physics.CheckSphere(transform.position, m_GroundDetectionDistance, m_GroundMask);
        Debug.Log(m_IsGrounded);
        
    }

    private void MovePlayer()
    {
        bool isOnSlope = IsOnSlope();

        if (m_IsGrounded && !isOnSlope)
        {
            m_RigidBody.AddForce(m_MovementMultiplier * m_MoveSpeed * m_MoveDirection.normalized, ForceMode.Acceleration);
        }
        else if (m_IsGrounded && isOnSlope)
        {
            m_RigidBody.AddForce(m_MovementMultiplier * m_MoveSpeed * m_SlopeMoveDirection.normalized, ForceMode.Acceleration);
        }
        else if(!m_IsGrounded)
        {
            m_RigidBody.AddForce(m_MovementMultiplier * m_AirMultiplier * m_MoveSpeed * m_MoveDirection.normalized, ForceMode.Acceleration);
        }
    }

    private void HandleSlopes()
    {
        m_SlopeMoveDirection = Vector3.ProjectOnPlane(m_MoveDirection, m_SlopeHit.normal);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(m_IsGrounded)
            m_RigidBody.AddForce(transform.up * m_JumpForce, ForceMode.Impulse);
    }

    private bool IsOnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out m_SlopeHit, 0.5f))
        {
            return m_SlopeHit.normal != Vector3.up;
        }
        else return false;
    }
}

