using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Rigidbody m_RigidBody;

    private float m_HorizontalMovement;
    private float m_VerticalMovement;

    private bool m_IsGrounded;

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
    }

    private void HandleInput()
    {
        // Move
        m_HorizontalMovement = Input.GetAxisRaw("Horizontal");
        m_VerticalMovement = Input.GetAxisRaw("Vertical");

        m_MoveDirection = m_Orientation.forward * m_VerticalMovement + m_Orientation.right * m_HorizontalMovement;

        // Jump
        if(Input.GetKeyDown(KeyCode.Space) && m_IsGrounded)
        {
            Jump();
        }
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
        float multiplier = m_IsGrounded ? m_MovementMultiplier : m_MovementMultiplier * m_AirMultiplier;

        Vector3 force = multiplier * m_MoveSpeed * m_MoveDirection.normalized;
        m_RigidBody.AddForce(force, ForceMode.Acceleration);
    }

    private void Jump()
    {
        m_RigidBody.AddForce(transform.up * m_JumpForce, ForceMode.Impulse);
    }
}

