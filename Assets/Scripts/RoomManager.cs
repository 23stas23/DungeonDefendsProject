using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Room Setting")]
    [SerializeField]private GameObject[] doors;
    public bool isActive;
    public bool isDone;

    [Header("Spawn Enemy")]
    [SerializeField] private GameObject[] prefabsEnemy;
    [SerializeField] private int maxEnemy;
    [SerializeField] private int minEnemy;
    [SerializeField] private float radiusSpawn;
    public int EnemyAllive;

    

    private int countEnemy;

    void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetButtonDown("SpawnEnemy"))
        {
            SpawnEnemys();
        }
        if (EnemyAllive <= 0)
        {
            isDone = true;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isDone && !isActive)
            {
                SpawnEnemys();
                isActive = true;
            }
        }
    }


    private void SpawnEnemys()
    {
        countEnemy = Random.Range(minEnemy, maxEnemy);
        EnemyAllive = countEnemy;

        for (int i = 0; i < countEnemy; i++)
        {
            Vector3 randomPosition = Random.insideUnitCircle * radiusSpawn;

            GameObject enemy = Instantiate(prefabsEnemy[Random.Range(0, prefabsEnemy.Length)], randomPosition + transform.position, Quaternion.identity);
            enemy.GetComponent<EnemySystem>().roomM = gameObject;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusSpawn);
    }
}
