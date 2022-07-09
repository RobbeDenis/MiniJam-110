using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KamikazeEnemy : MonoBehaviour
{
    [SerializeField] GameObject m_DeathThingy;

    public Transform m_TorchTransform;
    private NavMeshAgent m_NavMeshAgent;


    // Start is called before the first frame update
    void Start()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //rotate to face player
        //var lookPos = m_PlayerTransform.position - transform.position;
        //lookPos.y = 0;
        //var rotation = Quaternion.LookRotation(lookPos);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_FacingSpeed);

        m_NavMeshAgent.destination = m_TorchTransform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Torch"))
        {
            //spawn particle thingy after death
            Instantiate(m_DeathThingy, transform);
        }
    }
}
