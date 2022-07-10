using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("General settings")]
    [SerializeField] private float m_Speed = 25f;
    [SerializeField] private float m_MaxTravelTime = 1f;
    [SerializeField] private float m_Damage = 20f;
    [SerializeField] private float m_Knockback = 5f;

    [Header("Pillar hit")]
    [SerializeField] private int m_PierceIncrease = 2;
    [SerializeField] private int m_MaxShieldLevel = 3;
    [SerializeField] private float m_ScaleIncrease = 0.5f;

    [Header("Recall settings")]
    [SerializeField] private float m_ReturnMultiplier = 0.1f;
    [SerializeField] private float m_MinReturnSpeed = 2f;

    [Header("Orbit settings")]
    [SerializeField] private float m_OrbitRange = 4f;
    [SerializeField] private float m_OrbitSpeed = 1f;
    [SerializeField] private float m_OrbitAngle = 10f;
    [SerializeField] private float m_MaxOrbitTime = 2f;

    [Header("Shoot settings")]
    [SerializeField] private GameObject m_Bullet;
    [SerializeField] private float m_FireRate = 2f;

    private ShieldControl m_Controller;

    private Rigidbody m_Rigidbody;
    private Transform m_RecallTarget;
    private Transform m_OrbitTarget;
    private float m_TravelTime;
    private float m_OrbitTime;
    private int m_EnemyPierceAmount = 0;
    private int m_ShieldLevel = 1;
    private bool m_Recalling = false;
    private bool m_Active = false;
    private bool m_LookingForOrbit = false;
    private bool m_Orbitting = false;
    private bool m_Clockwise = true;

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
        else if (m_LookingForOrbit)
        {
            if (m_OrbitTarget == null)
            {
                Recall(m_Controller.GetSocket());
                return;
            }

            Vector3 direction = Vector3.Normalize(m_OrbitTarget.position - transform.position);
            transform.forward = direction;

            float distance = Vector3.Distance(m_OrbitTarget.position, transform.position);

            distance *= m_ReturnMultiplier;

            if (distance < m_MinReturnSpeed)
                distance = m_MinReturnSpeed;

            m_Rigidbody.MovePosition(transform.position + direction * distance);
        }
        else if (m_Orbitting)
        {
            if (m_OrbitTarget == null)
            {
                Recall(m_Controller.GetSocket());
                return;
            }

            Vector3 direction = Vector3.Normalize(m_OrbitTarget.position - transform.position);
            if (m_Clockwise)
            {
                Vector3 newDirection = Quaternion.AngleAxis(90 + m_OrbitAngle, Vector3.up) * direction;
                newDirection.y = 0f;
                m_Rigidbody.MovePosition(transform.position + newDirection * -m_OrbitSpeed);
            }
            else
            {
                Vector3 newDirection = Quaternion.AngleAxis(90 - m_OrbitAngle, Vector3.up) * direction;
                newDirection.y = 0f;
                m_Rigidbody.MovePosition(transform.position + newDirection * m_OrbitSpeed);
            }
            transform.forward = direction;
        }
    }

    private void Update()
    {
        if(m_Active && !m_Recalling && !m_Orbitting)
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
        if(m_LookingForOrbit)
        {
            if (m_OrbitTarget == null)
            {
                Recall(m_Controller.GetSocket());
                return;
            }

            if (Mathf.Abs(Vector3.Distance(transform.position, m_OrbitTarget.position)) <= m_OrbitRange)
            {
                m_Orbitting = true;
                m_Rigidbody.velocity = new Vector3(0f, 0f, 0f);
                m_LookingForOrbit = false;

                if (0 > AngleDir(transform.forward, m_OrbitTarget.position - transform.position, transform.up))
                    m_Clockwise = false;
                else
                    m_Clockwise = true;

                Shoot();
            }
        }
        if(m_Orbitting)
        {
            if (m_OrbitTime >= m_MaxOrbitTime)
            {
                Recall(m_Controller.GetSocket());
                m_OrbitTime = 0f;
            }
            else
            {
                m_OrbitTime += Time.deltaTime;
            }
        }
    }

    // https://forum.unity.com/threads/left-right-test-function.31420/
    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_Active)
            return;

        if (other.gameObject.tag.Equals("CheckPoint") ||
            other.gameObject.tag.Equals("Player") ||
            other.gameObject.tag.Equals("Torch") ||
            other.gameObject.tag.Equals("Fairy") ||
            other.gameObject.tag.Equals("Untagged"))
            return;

        if (other.gameObject.tag.Equals("Enemy"))
        {
            HP hp = other.gameObject.GetComponentInParent<HP>();

            if (hp == null)
                return;

            hp.TakeDamage(m_Damage);
            Rigidbody r = other.gameObject.GetComponentInParent<Rigidbody>();

            if(r != null)
                r.AddForce(transform.forward * m_Knockback, ForceMode.Impulse);

            m_EnemyPierceAmount--;
            if (m_EnemyPierceAmount <= 0)
            {
                if(!m_Orbitting)
                    Recall(m_Controller.GetSocket());
            }
        }
        else if(!m_Orbitting && !m_Recalling && other.gameObject.tag.Equals("Pillar"))
        {
            m_EnemyPierceAmount += m_PierceIncrease;

            Vector3 thisPos = transform.position;
            Vector3 otherPos = other.transform.position;
            thisPos.y = 0f;
            otherPos.y = 0f;

            Vector3 pillarNorm = Vector3.Normalize(otherPos - thisPos);
            Vector3 direction = Vector3.Reflect(transform.forward, pillarNorm);

            transform.forward = direction;
            direction.y = 0f;

            m_Rigidbody.velocity = new Vector3(0f, 0f, 0f);
            m_Rigidbody.AddForce(direction * m_Speed, ForceMode.Impulse);

            if (m_ShieldLevel < m_MaxShieldLevel)
            {
                float newScale = transform.localScale.x + m_ScaleIncrease;
                transform.localScale = new Vector3(newScale, newScale, newScale);
                m_ShieldLevel++;
            }

            m_TravelTime = 0f;
        }
        else if (m_Recalling && other.gameObject.tag.Equals("Catch"))
        {
            Deactivate();
            m_Controller.ShieldArrived();
        }
        else if(!other.gameObject.tag.Equals("Catch"))
        {
            if (m_Orbitting)
            {
                m_Clockwise = !m_Clockwise;

                if (other.gameObject.tag.Equals("Pillar")) 
                { 
                    if (m_ShieldLevel < m_MaxShieldLevel)
                    {
                        float newScale = transform.localScale.x + m_ScaleIncrease;
                        transform.localScale = new Vector3(newScale, newScale, newScale);
                        m_ShieldLevel++;
                    }
                }
            }
            else if (!m_Recalling)
            {
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
    }

    public void Recall(Transform target)
    {
        m_TravelTime = 0f;
        m_Recalling = true;
        m_RecallTarget = target;
        m_LookingForOrbit = false;
        m_Orbitting = false;
    }

    public void Deactivate()
    {
        m_LookingForOrbit = false;
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
        m_LookingForOrbit = true;
        m_OrbitTarget = target;
        m_Active = true;
        m_Recalling = false;
        m_TravelTime = 0f;
    }

    private void Shoot()
    {
        if(m_Orbitting)
        {
            Instantiate(m_Bullet, transform.position, transform.rotation);
            Invoke("Shoot", 1f / m_FireRate);
        }
    }
}
