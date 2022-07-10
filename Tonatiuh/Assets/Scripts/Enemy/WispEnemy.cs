using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WispEnemy : MonoBehaviour
{
    //[SerializeField] float m_FacingSpeed = 3f;

    //Old hovering code
    //[SerializeField] private float m_HoverAmplitude = 0.1f;
    //[SerializeField] private float m_HoverSpeed = 1.1f;

    [Header("Movement Settings")]
    [SerializeField] private float m_MinRepositionDelay = 3f;
    [SerializeField] private float m_MaxRepositionDelay = 6f;
    [SerializeField] private float m_StrafeDistance = 4f;
    [SerializeField] private float m_MaxPlayerDistance = 15f;


    [Header("Bullet/Firing Settings")]
    [SerializeField] private Transform m_Socket;
    [SerializeField] private GameObject m_BulletPrefab;
    [SerializeField] private float m_FireRate = 0.25f;
    [SerializeField] private float m_SpawnFireDelay = 2f;


    //Old hovering code
    //private Vector3 m_StartPos;
    public Transform m_PlayerTransform { get; set; }
    private NavMeshAgent m_NavMeshAgent;
    private bool m_GetNewPos;

    private const float m_AIMHEIGHTOFFSET = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //Old hovering code
        //m_StartPos = transform.position;
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        float delay = Random.Range(m_MinRepositionDelay, m_MaxRepositionDelay);
        Invoke("ChooseNewPos", delay);
        Invoke("ShootBullet", m_SpawnFireDelay);

        //hardcoded offset cuz player transform is at top of it's head
        m_PlayerTransform.position = new Vector3(m_PlayerTransform.position.x,
            m_PlayerTransform.position.y - m_AIMHEIGHTOFFSET, m_PlayerTransform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, m_PlayerTransform.position) > m_MaxPlayerDistance)
        {
            m_NavMeshAgent.destination = m_PlayerTransform.position;
        }
        else if (m_GetNewPos)
        {
            m_GetNewPos = false;

            if (Random.value <= 0.5f)
                m_NavMeshAgent.destination = transform.position + (transform.right * m_StrafeDistance);
            else
                m_NavMeshAgent.destination = transform.position + (transform.right * -m_StrafeDistance);
        }

        //rotate to face player
        transform.LookAt(m_PlayerTransform.position);

        //Vector3 lookPos = m_PlayerTransform.position - transform.position;
        //Vector3.Normalize(lookPos);
        //lookPos.Normalize();
        //lookPos.y -= m_AIMHEIGHTOFFSET;
        //lookPos.y = -5;
        //Quaternion rotation = Quaternion.LookRotation(lookPos);
        //Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_FacingSpeed);
        //transform.forward = Vector3.Lerp(transform.forward, lookPos, Time.deltaTime * m_FacingSpeed);
    }

    private void FixedUpdate()
    {
        //Old hovering code
        //Vector3 newPos = m_StartPos;
        //newPos.y = m_StartPos.y + (Mathf.Sin(Time.realtimeSinceStartup * m_HoverSpeed) * m_HoverAmplitude);
        //transform.position = newPos;
    }

    void ChooseNewPos()
    {
        m_GetNewPos = true;
        float delay = Random.Range(m_MinRepositionDelay, m_MaxRepositionDelay);
        Invoke("ChooseNewPos", delay);
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(m_BulletPrefab, m_Socket.position, m_Socket.rotation);
        bullet.transform.forward = transform.forward;
        Invoke("ShootBullet", 1f / m_FireRate);
        Debug.Log("BULLET");
    }
}
