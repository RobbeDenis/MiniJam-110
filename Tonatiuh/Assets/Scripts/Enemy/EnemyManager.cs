using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform m_PlayerCameraTransform;

    [Header("EnemyPrefabs")]
    [SerializeField] private GameObject m_BasicEnemy;

    [Header("Settings")]
    [SerializeField] private int m_BasicEnemyCount;

    private List<GameObject> m_BasicEnemies = new List<GameObject>();

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
            GameObject tempEnemy = Instantiate(m_BasicEnemy, transform);
            BasicEnemy basicCmpt = tempEnemy.GetComponent<BasicEnemy>();

            if (basicCmpt)
            {
                basicCmpt.m_PlayerTransform = m_PlayerCameraTransform;
            }

            m_BasicEnemies.Add(tempEnemy);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
