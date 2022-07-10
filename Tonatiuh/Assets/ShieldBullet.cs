using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBullet : MonoBehaviour
{
    [SerializeField] private float m_Speed = 10;
    [SerializeField] private Rigidbody m_Rigidbody;
    [SerializeField] private float m_Lifetime = 4f;
    [SerializeField] private float m_EndDelay = 1f;
    [SerializeField] private ParticleSystem m_Sys;
    [SerializeField] private Light m_Light;

    private bool m_Disabled = false;
    private bool m_Hit = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody.AddForce(transform.forward * m_Speed, ForceMode.Impulse);
        Invoke("End", m_Lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_Hit || m_Disabled)
            return;

        if (other.gameObject.tag.Equals("Enemy"))
        {
            m_Hit = true;
        }

        m_Rigidbody.velocity = new Vector3(0, 0, 0);
        m_Rigidbody.useGravity = true;
        m_Light.intensity = 0f;
    }
    private void End()
    {
        m_Sys.Stop();
        m_Disabled = true;
        Invoke("RealEnd", m_EndDelay);
    }

    private void RealEnd()
    {
        Destroy(gameObject);
    }
}
