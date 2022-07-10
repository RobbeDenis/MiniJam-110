using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] float m_FacingSpeed = 3f;

    public Transform m_PlayerTransform { get; set; }
    private NavMeshAgent m_NavMeshAgent;
    //private HP m_HP;

    // Start is called before the first frame update
    void Start()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        //m_HP = GetComponent<HP>();
    }

    // Update is called once per frame
    void Update()
    {
        //rotate to face player
        var lookPos = m_PlayerTransform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_FacingSpeed);

        m_NavMeshAgent.destination = m_PlayerTransform.position;
    }
}
