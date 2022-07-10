using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    [SerializeField] private float m_InitialHP = 100f;
    [SerializeField] private float m_MaxHP = 100f;

    [Space]
    [SerializeField] GameObject m_DeathExplosion;

    public float m_HP { get; private set; }

    public bool m_IsDead
    {
        get { return m_HP <= 0f; }
        private set { }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_HP = m_InitialHP;
    }

    // Update is called once per frame
    void Update()
    {
        //enable this code when anything below this needs to be executed
        //if (m_HP <= 0f)
        //    return;
    }

    public void TakeDamage(float damage)
    {
        m_HP -= damage;

        if (m_HP <= 0f)
        {
            if (m_DeathExplosion != null)
            {
                //code for when something dies

                //spawning particle thingy after death and making it clean itself up
                GameObject DeathExplosion = Instantiate(m_DeathExplosion, transform.position, transform.rotation);
                ParticleSystem explosionParticles = DeathExplosion.GetComponent<ParticleSystem>();
                float totalDuration = explosionParticles.main.duration + explosionParticles.main.startLifetime.constantMax;
                Destroy(DeathExplosion, totalDuration);
            }   

            Destroy(gameObject);
        }
    }

    public void ReplenishHP(float value)
    {
        m_HP = m_HP + value > m_MaxHP ? m_MaxHP : m_HP + value;
    }
}
