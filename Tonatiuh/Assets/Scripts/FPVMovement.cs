using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPVMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float MoveSpeed = 6f;
    [SerializeField] private float MovementMultier = 10f;
    [SerializeField] private float DefaultRigidbodyDrag = 6f;

    [Header("References")]
    [SerializeField] private Transform m_Orientation;

    private Vector3 m_MoveDirection;
    private Rigidbody m_RigidBody;

    private float m_HorizontalMovement;
    private float m_VerticalMovement;

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
        HandleInput();
        HandleDrag();
    }

    private void HandleInput()
    {
        m_HorizontalMovement = Input.GetAxisRaw("Horizontal");
        m_VerticalMovement = Input.GetAxisRaw("Vertical");

        Debug.Log(m_VerticalMovement);

        m_MoveDirection = m_Orientation.forward * m_VerticalMovement + m_Orientation.right * m_HorizontalMovement;
    }

    private void HandleDrag()
    {
        m_RigidBody.drag = DefaultRigidbodyDrag;
    }

    private void MovePlayer()
    {
        Vector3 force = MovementMultier * MoveSpeed * m_MoveDirection.normalized;
        m_RigidBody.AddForce(force, ForceMode.Acceleration);
    }
}

