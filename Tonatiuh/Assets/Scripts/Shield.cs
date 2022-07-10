using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("General settings")]
    [SerializeField] private float m_Speed = 25f;
    [SerializeField] private float m_MaxTravelTime = 1f;

    [Header("Pillar hit")]
    [SerializeField] private int m_PierceIncrease = 2;
    [SerializeField] private int m_MaxShieldLevel = 3;
    [SerializeField] private float m_ScaleIncrease = 0.5f;

    [Header("Recall settings")]
    [SerializeField] private float m_ReturnMultiplier = 0.1f;
    [SerializeField] private float m_MinReturnSpeed = 2f;

    [Header("Orbit settings")]

    private ShieldControl m_Controller;

    private Rigidbody m_Rigidbody;
    private Transform m_RecallTarget;
    private float m_TravelTime;
    private int m_EnemyPierceAmount = 0;
    private int m_ShieldLevel = 1;
    private bool m_Recalling = false;
    private bool m_Active = false;

    public void SetController(ShieldControl controller)
    {
        m_Controller = controller;
    }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float y = -10f;
        transform.position = new Vector3(0f, y, 0f);
    }

    private void FixedUpdate()
    {
        if (m_Recalling)
        {
            Vector3 direction = Vector3.Normalize(m_RecallTarget.position - transform.position);
            transform.forward = direction;

            float distance = Vector3.Distance(m_RecallTarget.position, transform.position);

            distance *= m_ReturnMultiplier;

            if (distance < m_MinReturnSpeed)
                distance = m_MinReturnSpeed;

            m_Rigidbody.MovePosition(transform.position + direction * distance);
        }
    }

    private void Update()
    {
        if(m_Active && !m_Recalling)
        {
            if(m_TravelTime >= m_MaxTravelTime)
            {
                Recall(m_Controller.GetSocket());
            }
            else
            {
                m_TravelTime += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_Active)
            return;

        if (other.gameObject.tag == "CheckPoint" ||
            other.gameObject.tag == "Player")
            return;

        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("EnemyHit");
            m_EnemyPierceAmount--;

            if (m_EnemyPierceAmount <= 0)
            {
                Recall(m_Controller.GetSocket());
            }
        }
        else if(!m_Recalling && other.gameObject.tag == "Pillar")
        {
            Debug.Log("Pillar");
            m_EnemyPierceAmount += m_PierceIncrease;

            Vector3 thisPos = transform.position;
            Vector3 otherPos = other.transform.position;
            thisPos.y = 0f;
            otherPos.y = 0f;

            Vector3 pillarNorm = Vector3.Normalize(otherPos - thisPos);
            Vector3 direction = Vector3.Reflect(transform.forward, pillarNorm);

            transform.forward = direction;

            m_Rigidbody.velocity = new Vector3(0f, 0f, 0f);
            m_Rigidbody.AddForce(direction * m_Speed, ForceMode.Impulse);

            if(m_ShieldLevel < m_MaxShieldLevel)
            {
                float newScale = transform.localScale.x + m_ScaleIncrease;
                transform.localScale = new Vector3(newScale, newScale, newScale);
                m_ShieldLevel++;
            }

            m_TravelTime = 0f;
        }
        else if (m_Recalling && other.gameObject.tag == "Catch")
        {
            Debug.Log("Catched");
            Deactivate();
            m_Controller.ShieldArrived();
        }
        else if(other.gameObject.tag != "Catch")
        {
            if(!m_Recalling)
            {
                Debug.Log(other.gameObject.tag + "Hit");
                Recall(m_Controller.GetSocket());
            }
        }
    }

    public void Throw(Vector3 direction)
    {
        transform.forward = direction;

        m_Rigidbody.velocity = new Vector3(0f, 0f, 0f);
        m_Rigidbody.AddForce(direction * m_Speed, ForceMode.Impulse);
        m_Active = true;

        Debug.Log("Throw");
    }

    public void Recall(Transform target)
    {
        m_TravelTime = 0f;
        m_Recalling = true;
        m_RecallTarget = target;
        Debug.Log("Recall");
    }

    public void Deactivate()
    {
        m_Recalling = false;
        m_Active = false;
        float y = -10f;
        m_Rigidbody.velocity = new Vector3(0f, 0f, 0f);
        transform.position = new Vector3(0f, y, 0f);
        transform.localScale = new Vector3(1f, 1f, 1f);
        m_EnemyPierceAmount = 0;
        m_ShieldLevel = 1;
    }

    public void GoOrbit(Transform target)
    {

    }
}
