using Unity.Cinemachine;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsEnemy;
    [SerializeField] private int maxEnemy;
    [SerializeField] private int minEnemy;
    [SerializeField] private float radiusSpawn;
    [SerializeField] private GameObject[] allEnemy;

    private int countEnemy;

    private void Start()
    {
        SpawnEnemys();
    }

    private void SpawnEnemys()
    {
        countEnemy = Random.Range(minEnemy, maxEnemy);
        for (int i = 0; i < countEnemy; i++)
        {
            Vector3 randomPosition = Random.insideUnitCircle * radiusSpawn;

            GameObject enemy = Instantiate(prefabsEnemy[Random.Range(0, prefabsEnemy.Length)], randomPosition + transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusSpawn);
    }
}
