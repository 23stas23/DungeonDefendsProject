using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class EnemySystem : MonoBehaviour
{
    [Header("Characteristic")]
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private int criticalDamage;
    [SerializeField] private float radiusAttack;
    [Space(10)]

    [Header("Weapoint")]
    [SerializeField] private string typeWeapoint;
    [SerializeField] private WeapointSystem weapoint;
    
    [Space(10)]

    [Header("Radar Enemy Settings")]
    [SerializeField] private float radiusRadar;
    [SerializeField] private LayerMask playerLayer;
    [Space(10)]

    [Header("Patrol Setings")]
    [SerializeField] private Collider2D[] targets;
    [SerializeField] private Transform[] patrolPositions;
    [SerializeField] private int currentPoint;
    [Space(10)]

    public GameObject roomM;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CheckWeapoint();
    }

    void FixedUpdate()
    {
        DetectedPlayer();
    }

    public void TakeDamage(int value)
    {
        health -= value;
        if (health < 0)
        {
            Destroy(gameObject);
            roomM.GetComponent<RoomManager>().EnemyAllive -= 1;
        }
    }
    public void DetectedPlayer()
    {
        targets = Physics2D.OverlapCircleAll(transform.position, radiusRadar, playerLayer);
        if (targets.Length > 0)
        {
            if (Vector2.Distance(transform.position, targets[0].transform.position) > radiusAttack)
            {
                transform.position = Vector2.MoveTowards(transform.position, targets[0].transform.position, speed * Time.fixedDeltaTime);
            }
        }
        else
        {
            PatrolEnemy();
        }
    }
    private void PatrolEnemy()
    {
        if (patrolPositions.Length >= 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPositions[currentPoint].position, speed * Time.fixedDeltaTime);
            if(Vector2.Distance(transform.position, patrolPositions[currentPoint].position) < 0.1f)
            {
                currentPoint = UnityEngine.Random.Range(0, patrolPositions.Length - 1);
            }
        }
    }

    void CheckWeapoint()
    {
        if (weapoint == null) typeWeapoint = "Nothing";
        else typeWeapoint = weapoint.type;
        if (typeWeapoint == "Gun")
        {
            damage = weapoint.damage;
            criticalDamage = weapoint.criticalDamage;
            //radiusAttack = weapoint.radiusAttack;
        }
        else if (typeWeapoint == "Nothing")
        {

        }
    }

    void Attack()
    {
        if (Vector2.Distance(transform.position, targets[0].transform.position) <= radiusAttack)
        {
            
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radiusRadar);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
    }
}
