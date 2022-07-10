using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [Header("properties")]
    [SerializeField] private float m_InitialHP = 100f;
    [SerializeField] private float m_MaxHP = 100f;
    [SerializeField] private float m_OutsideDamage = 5f;
    [SerializeField] private float m_OutsideTimeDamage = 1f;

    [Header("healthbar")]
    [SerializeField] private HPBar m_HealthBar;

    private bool m_insideLight = false;
    private float m_OutsideTimer;

    public float m_HP
    {
        get;
        private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_HP = m_InitialHP;
        m_HealthBar.SetMaxHealth(m_InitialHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_HP <= 0f)
            return;


        if(!m_insideLight)
        {
            m_OutsideTimer += Time.deltaTime;
            if(m_OutsideTimer >= m_OutsideTimeDamage)
            {
                m_OutsideTimer = 0;
                OutsideDamage();
            }
        }

    }

    public void TakeDamage(float damage)
    {
        m_HP -= damage;

        if (m_HP <= 0f)
        {
            //code for when something dies
        }

        m_HealthBar.SetHealth(m_HP);

    }
    private void OutsideDamage()
    {
        if (m_insideLight)
            return;

        TakeDamage(m_OutsideDamage);

        Debug.Log(m_HP);
    }


    public void ReplenishHP(float value)
    {
        m_HP = m_HP + value > m_MaxHP ? m_MaxHP : m_HP + value;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Torch")
        {
            m_insideLight = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Torch")
        {
            m_insideLight = false;
            m_OutsideTimer = 0;
        }
    }
}
