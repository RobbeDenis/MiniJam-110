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
    [SerializeField] private GameObject m_BullPreFab;
    [SerializeField] private GameObject m_WispPreFab;

    [Header("Settings")]
    [SerializeField] private int m_BasicEnemyCount;
    [SerializeField] private int m_KamikazeEnemyCount;
    [SerializeField] private int m_BullEnemyCount;
    [SerializeField] private int m_WispEnemyCount;

    private List<GameObject> m_BasicEnemies = new List<GameObject>();
    private List<GameObject> m_KamikazeEnemies = new List<GameObject>();
    private List<GameObject> m_BullEnemies = new List<GameObject>();
    private List<GameObject> m_WispEnemies = new List<GameObject>();

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

        for (int count = 0; count < m_BullEnemyCount; count++)
        {
            GameObject tempEnemy = Instantiate(m_BullPreFab, transform);
            BullEnemy bullCmpt = tempEnemy.GetComponent<BullEnemy>();

            if (bullCmpt)
            {
                bullCmpt.m_PlayerTransform = m_PlayerCameraTransform;
            }

            m_BullEnemies.Add(tempEnemy);
        }

        for (int count = 0; count < m_WispEnemyCount; count++)
        {
            GameObject tempEnemy = Instantiate(m_WispPreFab, transform);
            WispEnemy wispCmpt = tempEnemy.GetComponent<WispEnemy>();

            if (wispCmpt)
            {
                wispCmpt.m_PlayerTransform = m_PlayerCameraTransform;
            }

            m_WispEnemies.Add(tempEnemy);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
