using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    MeshCollider col;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void Throw(Vector3 direction)
    {
        transform.forward = direction;

        m_Rigidbody.velocity = new Vector3(0f, 0f, 0f);
        m_Rigidbody.AddForce(direction * 20, ForceMode.Impulse);

        Debug.Log("Throw");
    }

    public void Recall(Transform target)
    {
        Debug.Log("Recall");
    }
}
