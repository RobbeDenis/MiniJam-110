using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("General settings")]
    [SerializeField] private float m_Speed = 25f;

    [Header("Recall settings")]
    [SerializeField] private float m_ReturnMultiplier = 0.1f;
    [SerializeField] private float m_MinReturnSpeed = 2f;

    private ShieldControl m_Controller;

    private Rigidbody m_Rigidbody;
    private Transform m_RecallTarget;
    private bool m_Recalling = false;

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
    private void OnTriggerEnter(Collider other)
    {
        if (m_Recalling && other.gameObject.tag == "Catch")
        {
            Debug.Log("Catched");
            Deactivate();
            m_Controller.ShieldArrived();
        }
    }


    public void Throw(Vector3 direction)
    {
        transform.forward = direction;

        m_Rigidbody.velocity = new Vector3(0f, 0f, 0f);
        m_Rigidbody.AddForce(direction * m_Speed, ForceMode.Impulse);

        Debug.Log("Throw");
    }

    public void Recall(Transform target)
    {
        m_Recalling = true;
        m_RecallTarget = target;
        Debug.Log("Recall");
    }

    public void Deactivate()
    {
        m_Recalling = false;
        float y = -10f;
        m_Rigidbody.velocity = new Vector3(0f, 0f, 0f);
        transform.position = new Vector3(0f, y, 0f);
    }
}
