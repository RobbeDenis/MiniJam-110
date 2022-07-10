using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    [Space]
    [SerializeField] private List<Wave> m_Waves;

    private int m_CurrentWaveIndex = 0;
    //[SerializeField] private int m_KamikazeEnemyCount;
    //[SerializeField] private int m_BullEnemyCount;
    //[SerializeField] private int m_WispEnemyCount;

    private List<GameObject> m_BasicEnemies = new List<GameObject>();
    private List<GameObject> m_KamikazeEnemies = new List<GameObject>();
    private List<GameObject> m_BullEnemies = new List<GameObject>();
    private List<GameObject> m_WispEnemies = new List<GameObject>();

    private List<GameObject> m_LivingEnemies = new List<GameObject>();

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

        SpawnCurrentWave();



        //foreach (Wave WaveData in m_Waves)
        //{
        //    //var spawnwer = WaveData.Spawners;
        //    foreach (var spawnData in WaveData.Spawners)
        //    {
        //        Vector3 spawnPos = spawnData.SpawnerLocation;

        //        foreach (var enemy in spawnData.Enemies)
        //        {
        //            switch (enemy)
        //            {
        //                case EnemyTypes.Basic:
        //                    {
        //                        GameObject tempEnemy = Instantiate(m_BasicEnemyPrefab, spawnPos, transform.rotation);

        //                        BasicEnemy basicCmpt = tempEnemy.GetComponent<BasicEnemy>();
        //                        if (basicCmpt)
        //                            basicCmpt.m_PlayerTransform = m_PlayerCameraTransform;

        //                        m_LivingEnemies.Add(tempEnemy);
        //                    }
        //                    break;
        //                case EnemyTypes.Kamikaze:
        //                    {
        //                        GameObject tempEnemy = Instantiate(m_KamikazePreFab, spawnPos, transform.rotation);

        //                        KamikazeEnemy kamikazeCmpt = tempEnemy.GetComponent<KamikazeEnemy>();
        //                        if (kamikazeCmpt)
        //                            kamikazeCmpt.m_TorchTransform = m_TorchTransform;

        //                        m_LivingEnemies.Add(tempEnemy);
        //                    }
        //                    break;
        //                case EnemyTypes.Bull:
        //                    {
        //                        GameObject tempEnemy = Instantiate(m_BullPreFab, spawnPos, transform.rotation);

        //                        BullEnemy bullCmpt = tempEnemy.GetComponent<BullEnemy>();
        //                        if (bullCmpt)
        //                            bullCmpt.m_PlayerTransform = m_PlayerCameraTransform;

        //                        m_LivingEnemies.Add(tempEnemy);
        //                    }
        //                    break;
        //                case EnemyTypes.Wisp:
        //                    {
        //                        GameObject tempEnemy = Instantiate(m_WispPreFab, spawnPos, transform.rotation);

        //                        WispEnemy wispCmpt = tempEnemy.GetComponent<WispEnemy>();
        //                        if (wispCmpt)
        //                            wispCmpt.m_PlayerTransform = m_PlayerCameraTransform;

        //                        m_LivingEnemies.Add(tempEnemy);
        //                    }
        //                    break;
        //            }
        //        }
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //remove all NULL's from the list
        m_LivingEnemies = m_LivingEnemies.Where(item => item != null).ToList();

        if (m_LivingEnemies.Count == 0)
        {
            ++m_CurrentWaveIndex;

            if (m_CurrentWaveIndex >= m_Waves.Count)
            {
                m_CurrentWaveIndex = 0;
            }

            SpawnCurrentWave();
        }
    }

    void SpawnCurrentWave()
    {
        foreach (var spawnData in m_Waves[m_CurrentWaveIndex].Spawners)
        {
            Vector3 spawnPos = spawnData.SpawnerLocation;

            foreach (var enemy in spawnData.Enemies)
            {
                switch (enemy)
                {
                    case EnemyTypes.Basic:
                        {
                            GameObject tempEnemy = Instantiate(m_BasicEnemyPrefab, spawnPos, transform.rotation);

                            BasicEnemy basicCmpt = tempEnemy.GetComponent<BasicEnemy>();
                            if (basicCmpt)
                                basicCmpt.m_PlayerTransform = m_PlayerCameraTransform;

                            m_LivingEnemies.Add(tempEnemy);
                        }
                        break;
                    case EnemyTypes.Kamikaze:
                        {
                            GameObject tempEnemy = Instantiate(m_KamikazePreFab, spawnPos, transform.rotation);

                            KamikazeEnemy kamikazeCmpt = tempEnemy.GetComponent<KamikazeEnemy>();
                            if (kamikazeCmpt)
                                kamikazeCmpt.m_TorchTransform = m_TorchTransform;

                            m_LivingEnemies.Add(tempEnemy);
                        }
                        break;
                    case EnemyTypes.Bull:
                        {
                            GameObject tempEnemy = Instantiate(m_BullPreFab, spawnPos, transform.rotation);

                            BullEnemy bullCmpt = tempEnemy.GetComponent<BullEnemy>();
                            if (bullCmpt)
                                bullCmpt.m_PlayerTransform = m_PlayerCameraTransform;

                            m_LivingEnemies.Add(tempEnemy);
                        }
                        break;
                    case EnemyTypes.Wisp:
                        {
                            GameObject tempEnemy = Instantiate(m_WispPreFab, spawnPos, transform.rotation);

                            WispEnemy wispCmpt = tempEnemy.GetComponent<WispEnemy>();
                            if (wispCmpt)
                                wispCmpt.m_PlayerTransform = m_PlayerCameraTransform;

                            m_LivingEnemies.Add(tempEnemy);
                        }
                        break;
                }
            }
        }
    }
}
