using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public float m_Damage = 15f;
    [SerializeField] float m_Speed = 2f;

    private Rigidbody m_RigidBody;

    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_RigidBody.AddForce(transform.forward * m_Speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

        //transform.position = transform.position + (transform.forward * m_Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            Destroy(gameObject);
        else if (other.CompareTag("Player"))
            other.GetComponent<PlayerHP>().TakeDamage(m_Damage);
    }
}
