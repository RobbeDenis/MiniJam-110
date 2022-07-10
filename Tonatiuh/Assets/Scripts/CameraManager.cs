using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager m_instance;
    public static CameraManager Instance
    {
        get
        {
            if (m_instance is null)
                Debug.LogError("CameraManager is NULL");
            return m_instance;
        }
        set { m_instance = value; }
    }

    [SerializeField] private Camera m_Camera;

    private float m_FOV;
    private float m_WarpAmount;

    private bool m_Warp = false;
    private float m_MaxWarpTime;
    private float m_WarpTime;

    private bool m_UnWarp = false;
    private float m_MaxUnWarpTime;
    private float m_UnWarpTime;

    private void Awake()
    {
        m_instance = this;
    }

    private void Start()
    {
        m_FOV = m_Camera.fieldOfView;
    }

    private void Update()
    {
        if(m_Warp)
        {
            m_WarpTime += Time.deltaTime;
            m_Camera.fieldOfView = Mathf.Lerp(m_FOV, m_FOV + m_WarpAmount, m_WarpTime / m_MaxWarpTime);

            if(Mathf.Abs(m_Camera.fieldOfView - (m_FOV + m_WarpAmount)) <= Mathf.Epsilon)
            {
                m_Warp = false;
                m_UnWarp = true;
            }
        }
        else if (m_UnWarp)
        {
            m_UnWarpTime += Time.deltaTime;
            m_Camera.fieldOfView = Mathf.Lerp(m_FOV + m_WarpAmount, m_FOV, m_UnWarpTime / m_MaxUnWarpTime);
            if (Mathf.Abs(m_Camera.fieldOfView - m_FOV) <= Mathf.Epsilon)
            {
                m_UnWarp = false;
                m_Camera.fieldOfView = m_FOV;
            }
        }
    }

    public void FovWarp(float amount, float warpTime, float unWarpTime)
    {
        m_Warp = true;
        m_UnWarp = false;

        m_MaxWarpTime = warpTime;
        m_MaxUnWarpTime = unWarpTime;

        m_WarpTime = 0f;
        m_UnWarpTime = 0f;

        m_WarpAmount = amount;
    }
}
