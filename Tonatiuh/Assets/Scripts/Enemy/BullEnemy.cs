using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BullEnemy : MonoBehaviour
{
    [SerializeField] float m_FacingSpeed = 3f;

    [SerializeField] float m_ChargeForce = 25f;
    [SerializeField] float m_PlayerImpactForce = 20f;

    [SerializeField] float m_DelayBeforeCharge = 1f;
    [SerializeField] float m_HitCooldown = 1f;
    [SerializeField] float m_ChargeCooldown = 5f;
    [SerializeField] float m_ChargingDuration = 3f;


    //TODO: REMOVVE SERIALIZED FIELD 
    [SerializeField] public Transform m_PlayerTransform;
    private Rigidbody m_RigidBody;
    private MeleeAttack m_MeleeAttackCollider;

    private bool m_PlayerInTrigger = false;
    private bool m_ChargeMode = false;
    private bool m_CanCharge = true;
    private bool m_Stunned = false;

    // Start is called before the first frame update
    void Start()
    {
        //m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_RigidBody = GetComponent<Rigidbody>();
        m_MeleeAttackCollider = GetComponentInChildren<MeleeAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Stunned)
            return;

        if (m_ChargeMode)
        {
            if (m_MeleeAttackCollider.m_PlayerInTrigger)
            {
                m_MeleeAttackCollider.m_playerRigidBody.AddForce(transform.forward * m_PlayerImpactForce, ForceMode.Impulse);
                m_Stunned = true;
                m_ChargeMode = false;
                Invoke("ResetStun", m_HitCooldown);
            }

            return;
        }

        //rotate to face player
        var lookPos = m_PlayerTransform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_FacingSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_PlayerInTrigger = true;
            Invoke("AttamptChargeAttack", m_DelayBeforeCharge);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_PlayerInTrigger = false;
        }
    }

    private void AttamptChargeAttack()
    {
        //if player is still in trigger after the delay start the charge attack
        if (m_PlayerInTrigger && m_CanCharge)
        {
            m_ChargeMode = true;
            m_CanCharge = false;
            // makes melee collider just a collider by disabling its melee attack logic
            m_MeleeAttackCollider.m_Disable = true;

            Invoke("StopCharging", m_ChargingDuration);
            Invoke("ResetChargeAttack", m_ChargeCooldown);
            m_RigidBody.AddForce(transform.forward * m_ChargeForce, ForceMode.Impulse);
        }
    }

    void StopCharging()
    {
        m_ChargeMode = false;
        m_Stunned = true;
        Invoke("ResetStun", m_HitCooldown);
    }

    void ResetStun()
    {
        m_Stunned = false;
        m_MeleeAttackCollider.m_Disable = false;
    }

    void ResetChargeAttack()
    {
        m_CanCharge = true;
    }
}