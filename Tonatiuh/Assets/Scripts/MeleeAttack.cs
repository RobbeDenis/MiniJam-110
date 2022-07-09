using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] float m_AttackDelay = 0.75f;//delay until damage is actually applied
    [SerializeField] float m_AttackResetDelay = 1.5f;
    [SerializeField] float m_Damage = 5f;

    private HP m_HPCmpt = null;
    public Rigidbody m_playerRigidBody;

    public bool m_PlayerInTrigger = false;
    public bool m_Disable = false;
    private bool m_CanAttack = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_Disable)
            return;

        if (m_PlayerInTrigger && m_CanAttack)
        {
            m_CanAttack = false;
            Invoke("Attack", m_AttackDelay);
            Invoke("ResetAttack", m_AttackResetDelay);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_HPCmpt = other.GetComponent<HP>();
            m_playerRigidBody = other.GetComponent<Rigidbody>();
            m_PlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_HPCmpt = null;
            m_PlayerInTrigger = false;
        }
    }

    public void Attack()
    {
        if (!m_Disable && m_HPCmpt)
            m_HPCmpt.TakeDamage(m_Damage);
    }

    void ResetAttack()
    {
        m_CanAttack = true;
    }
}