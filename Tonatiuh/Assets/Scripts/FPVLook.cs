using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPVLook : MonoBehaviour
{
    [Header("Sensitivity")]
    [SerializeField] private float m_SensX = 2f;
    [SerializeField] private float m_SensY = 2f;

    [Header("References")]
    [SerializeField] private Transform m_CameraHolderTransform;
    [SerializeField] private Transform m_Orientation;

    private float m_MouseInputX;
    private float m_MouseInputY;

    private float m_Yaw;
    private float m_Pitch;

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
        m_MouseInputX = Input.GetAxisRaw("Mouse X");
        m_MouseInputY = Input.GetAxisRaw("Mouse Y");

        m_Yaw += m_MouseInputX * m_SensX;
        m_Pitch -= m_MouseInputY * m_SensY;

        m_Pitch = Mathf.Clamp(m_Pitch, -90f, 90f);
    }
}
