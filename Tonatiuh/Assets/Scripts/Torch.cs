using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float m_startRadius = 6f;
    [SerializeField] private float m_endRadius = 10f;
    [SerializeField] private float m_sizeDecrease = 0.1f;

    [SerializeField] private bool m_test = false;
    private bool m_doOnce = false;


    public bool m_torchComplete = false;
    bool m_isTorchActive = false;

    private float m_CurrentRadius = 0;
    Transform m_lightSphere;
    Light m_Light;

    SphereCollider m_sphereCollider;

    ParticleSystem m_fireParticle;
    float m_fireScale = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentRadius = m_startRadius;

        m_lightSphere = this.transform.Find("lightRadius");
        m_lightSphere.localScale = new Vector3(0, 0, 0);


        m_Light = GetComponentInChildren<Light>();
        m_Light.range = m_startRadius;
        m_Light.enabled = false;

        m_fireParticle = GetComponentInChildren<ParticleSystem>();
        var emission = m_fireParticle.emission;
        emission.enabled = false;


        //GetComponent<SphereCollider>().transform.localScale = m_lightSphere.localScale;
        m_sphereCollider = GetComponentInChildren<SphereCollider>();
        m_sphereCollider.radius = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isTorchActive)
            return;

        m_lightSphere.localScale = new Vector3(m_CurrentRadius, m_CurrentRadius, m_CurrentRadius);
        m_sphereCollider.radius = m_CurrentRadius/2;

        m_CurrentRadius -= Time.deltaTime * m_sizeDecrease;
        if (m_CurrentRadius <= 0)
            m_CurrentRadius = 0;

        m_Light.range = m_CurrentRadius;


        float scale = m_CurrentRadius / m_endRadius * m_fireScale;
        m_fireParticle.transform.localScale = new Vector3(scale, scale, scale);

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

    public void SetTorchActive()
    {
        m_lightSphere.localScale = new Vector3(m_startRadius, m_startRadius, m_startRadius);

        m_isTorchActive = true;
        m_Light.enabled = true;

        var emission = GetComponentInChildren<ParticleSystem>().emission;
        emission.enabled = true;

        GameManager.Instance.m_CurrentTorch = this;
    }
    public void AddLight(float lightAmount)
    {
        m_CurrentRadius += lightAmount;
        if (m_CurrentRadius >= m_endRadius)
        {
            m_torchComplete = true;
            GameManager.Instance.m_TorchComplete = true;


            m_lightSphere.localScale = new Vector3(0, 0, 0);

            m_isTorchActive = false;
            m_Light.enabled = false;

            var emission = GetComponentInChildren<ParticleSystem>().emission;
            emission.enabled = false;

        }
    }
    public void RemoveLight(float lightAmount)
    {
        m_CurrentRadius -= lightAmount;
        if (m_CurrentRadius <= 0)
        {
            m_CurrentRadius = 0;
        }
    }
}
