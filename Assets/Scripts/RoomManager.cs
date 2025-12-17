using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Room Setting")]
    [SerializeField]private GameObject gates;
    public bool isActive; // Is room currently active 
    public bool isDone; // Is room copleted

    [Header("Spawn Enemy")]
    public GameObject room;
    [SerializeField] private GameObject[] prefabsEnemy; // Enemy prefab list
    //Enemy count range
    [SerializeField] private int maxEnemy; 
    [SerializeField] private int minEnemy;
    //Spawn area size
    [SerializeField] private float widthSpawn;
    [SerializeField] private float hSpawn;
    [SerializeField] private Transform centerPosition;// center of room
    public int EnemyAllive; //Current alive enemies

    void Start()
    {
        SpawnEnemys();// Spawn enemy when start game

    }
    private void Update()
    {
        //Room cleared condition
        if (EnemyAllive <= 0)
        {
            isDone = true;
            isActive = false;
        }
        // Gates control 
        gates.SetActive(isActive);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Activate room when player enter
        if (collision.CompareTag("Player"))
        {
            if (isDone == false && isActive == false)
            {
                isActive = true;
            }
        }
    }


    private void SpawnEnemys() // Spawn enemy randomly inside room
    {
        int countEnemy = Random.Range(minEnemy, maxEnemy);
        EnemyAllive = countEnemy;

        for (int i = 0; i < countEnemy; i++)
        {
            float w = Random.Range(widthSpawn, -widthSpawn);
            float h = Random.Range(hSpawn, -hSpawn);
            //Get random position 
            Vector3 randomPosition = new Vector3(centerPosition.position.x + w, centerPosition.position.y + h, 0);
            //Create Enemy in scene
            GameObject enemy = Instantiate(prefabsEnemy[Random.Range(0, prefabsEnemy.Length)], randomPosition, Quaternion.identity);
            enemy.GetComponent<EnemySystem>().roomM = room;
        }
    }
}
