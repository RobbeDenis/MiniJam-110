using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPVCameraHolder : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform m_PlayerCameraTransform;

    private void Update()
    {
        transform.position = m_PlayerCameraTransform.position;
    }
}
