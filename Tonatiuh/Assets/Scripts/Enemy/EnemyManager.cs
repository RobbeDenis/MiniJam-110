using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Locations")]
    [SerializeField] private Transform m_PlayerCameraTransform;
    [SerializeField] private Transform m_TorchTransform;

    [Header("EnemyPrefabs")]
    [SerializeField] private GameObject m_BasicEnemyPrefab;
    [SerializeField] private GameObject m_KamikazePreFab;

    [Header("Settings")]
    [SerializeField] private int m_BasicEnemyCount;
    [SerializeField] private int m_KamikazeEnemyCount;

    private List<GameObject> m_BasicEnemies = new List<GameObject>();
    private List<GameObject> m_KamikazeEnemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //looking for player in scene
        if (m_PlayerCameraTransform == null)
        {
            Debug.LogWarning("NO PLAYER WAS GIVING!\nSEARCHING FOR PLAYER IN SCENE");

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj)
                m_PlayerCameraTransform = playerObj.transform;
            else
                Debug.LogError("NO PLAYER IN SCENE");
        }

        for (int count = 0; count < m_BasicEnemyCount; count++)
        {
            GameObject tempEnemy = Instantiate(m_BasicEnemyPrefab, transform);
            BasicEnemy basicCmpt = tempEnemy.GetComponent<BasicEnemy>();

            if (basicCmpt)
            {
                basicCmpt.m_PlayerTransform = m_PlayerCameraTransform;
            }

            m_BasicEnemies.Add(tempEnemy);
        }
        
        for (int count = 0; count < m_KamikazeEnemyCount; count++)
        {
            GameObject tempEnemy = Instantiate(m_KamikazePreFab, transform);
            KamikazeEnemy kamikazeCmpt = tempEnemy.GetComponent<KamikazeEnemy>();

            if (kamikazeCmpt)
            {
                kamikazeCmpt.m_TorchTransform = m_TorchTransform;
            }

            m_KamikazeEnemies.Add(tempEnemy);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
