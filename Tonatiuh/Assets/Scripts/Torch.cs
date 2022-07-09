using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float m_startRadius = 6f;
    [SerializeField] private float m_endRadius = 20f;
    [SerializeField] private float m_sizeDecrease = 0.1f;

    [SerializeField] private bool m_test = false;
    private bool m_doOnce = false;


    bool m_torchComplete = false;

    private float m_CurrentRadius = 0;
    Transform m_lightSphere;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentRadius = m_startRadius;

        m_lightSphere = this.transform.Find("lightRadius");


        m_lightSphere.localScale = new Vector3(m_startRadius, m_startRadius, m_startRadius);

        var idk = GetComponent<Light>();
        idk.intensity = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        m_lightSphere.localScale = new Vector3(m_CurrentRadius, m_CurrentRadius, m_CurrentRadius);

        m_CurrentRadius -= Time.deltaTime * m_sizeDecrease;
        if (m_CurrentRadius <= 0)
            m_CurrentRadius = 0;

        if (m_test)
        {
            if (!m_doOnce)
            {
                AddLight(0.5f);
                m_doOnce = true;
            }
        }
        else
            m_doOnce = false;
    }


    void AddLight(float lightAmount)
    {
        m_CurrentRadius += lightAmount;
        if (m_CurrentRadius >= m_endRadius)
        {
            m_torchComplete = true;
        }
    }
    void RemoveLight(float lightAmount)
    {
        m_CurrentRadius -= lightAmount;
        if (m_CurrentRadius <= 0)
        {
            m_CurrentRadius = 0;
        }
    }
}
