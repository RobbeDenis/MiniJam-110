using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KamikazeEnemy : MonoBehaviour
{
    [SerializeField] float m_RemoveLightAmount = 2f;
    [SerializeField] GameObject m_DeathExplosionKAMIKAZE;

    public Transform m_TorchTransform { get; set; }
    private NavMeshAgent m_NavMeshAgent;
    private HP m_HP;

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
            Torch torchCmpt = other.GetComponentInParent<Torch>();
            if (torchCmpt)
                torchCmpt.RemoveLight(m_RemoveLightAmount);

            //spawning particle thingy after death and making it clean itself up
            GameObject kamikazeExplosion = Instantiate(m_DeathExplosionKAMIKAZE, transform.position, transform.rotation);
            ParticleSystem explosionParticles = kamikazeExplosion.GetComponent<ParticleSystem>();
            float totalDuration = explosionParticles.main.duration + explosionParticles.main.startLifetime.constantMax;
            Destroy(kamikazeExplosion, totalDuration);

            //destroying itself
            Destroy(gameObject);
        }
    }
}
