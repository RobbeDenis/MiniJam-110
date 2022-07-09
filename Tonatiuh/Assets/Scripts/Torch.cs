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

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentRadius = m_startRadius;

        m_lightSphere = this.transform.Find("lightRadius");
        m_lightSphere.localScale = new Vector3(0, 0, 0);


        m_Light = GetComponentInChildren<Light>();
        m_Light.range = m_startRadius;
        m_Light.enabled = false;

        var emission = GetComponentInChildren<ParticleSystem>().emission;
        emission.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isTorchActive)
            return;

        m_lightSphere.localScale = new Vector3(m_CurrentRadius, m_CurrentRadius, m_CurrentRadius);

        m_CurrentRadius -= Time.deltaTime * m_sizeDecrease;
        if (m_CurrentRadius <= 0)
            m_CurrentRadius = 0;

        m_Light.range = m_CurrentRadius;

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
    //void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "Fairy")
    //        transform.position = new Vector3(500, 500, 500);
    //}
    public void SetTorchActive()
    {
        m_lightSphere.localScale = new Vector3(m_startRadius, m_startRadius, m_startRadius);

        m_isTorchActive = true;
        m_Light.enabled = true;

        var emission = GetComponentInChildren<ParticleSystem>().emission;
        emission.enabled = false;
    }
    public void AddLight(float lightAmount)
    {
        m_CurrentRadius += lightAmount;
        if (m_CurrentRadius >= m_endRadius)
        {
            m_torchComplete = true;
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
    public bool GetIsComplete()
    {
        return m_torchComplete;
    }
}
