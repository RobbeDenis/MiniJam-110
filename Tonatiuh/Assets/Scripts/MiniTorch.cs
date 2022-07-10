using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniTorch : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float m_size = 5f;


    Light m_Light;

    // Start is called before the first frame update
    void Start()
    {

        //pointlight
        m_Light = GetComponentInChildren<Light>();
        m_Light.range = m_size;
        m_Light.enabled = false;

        //fire particle effect
        ParticleSystem fireParticle = GetComponentInChildren<ParticleSystem>();
        var emission = fireParticle.emission;
        emission.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Torch")
        {
            m_Light.enabled = true;

            ParticleSystem fireParticle = GetComponentInChildren<ParticleSystem>();
            var emission = fireParticle.emission;
            emission.enabled = true;

            SphereCollider sphereCollider = GetComponentInChildren<SphereCollider>();
            sphereCollider.radius = m_size / 2;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Torch")
        {
            m_Light.enabled = false;

            ParticleSystem fireParticle = GetComponentInChildren<ParticleSystem>();
            var emission = fireParticle.emission;
            emission.enabled = false;

            SphereCollider sphereCollider = GetComponentInChildren<SphereCollider>();
            sphereCollider.radius = 0;
        }
    }

}
