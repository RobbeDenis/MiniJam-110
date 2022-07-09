using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    [SerializeField] private float m_InitialHP = 100f;
    [SerializeField] private float m_MaxHP = 100f;

    public float m_HP
    {
        get;
        private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_HP = m_InitialHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_HP <= 0f)
            return;



    }

    public void TakeDamage(float damage)
    {
        m_HP -= damage;

        if (m_HP <= 0f)
        {
            //code for when something dies
        }
    }

    public void ReplenishHP(float value)
    {
        m_HP = m_HP + value > m_MaxHP ? m_MaxHP : m_HP + value;
    }
}
