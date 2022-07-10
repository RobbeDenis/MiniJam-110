using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fairy : MonoBehaviour
{

    public float m_speedBop = 1;
    public float m_boppingSpeed = 1;
    public float m_boppingHeight = 0.5f;

    public GameObject[] m_destination;
    NavMeshAgent m_agent;

    int m_currentDest = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        m_agent.SetDestination(m_destination[m_currentDest].transform.position);

        float y = Mathf.PingPong(Time.time * m_speedBop, m_boppingSpeed) * m_boppingHeight - m_boppingHeight;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);

        if(GameManager.Instance.m_TorchComplete)
        {
            m_currentDest++;
            GameManager.Instance.m_TorchComplete = false;

            Light light = GetComponentInChildren<Light>();
            light.enabled = true;

            var emission = GetComponentInChildren<ParticleSystem>().emission;
            emission.enabled = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Torch")
        {
            Torch currentTorch = other.gameObject.GetComponent<Torch>();
            currentTorch.SetTorchActive();


            Light light = GetComponentInChildren<Light>();
            light.enabled = false;

            var emission = GetComponentInChildren<ParticleSystem>().emission;
            emission.enabled = false;
        }
        else if(other.tag == "CheckPoint")
        {
            m_currentDest++;
        }
    }


    public void GoNext()
    {
        m_currentDest++;
    }

}
