using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class EnemySystem : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private int damage;

    [Header("Attack")]
    [SerializeField] private float radiusAttack;
    [SerializeField] private float couldown;
    public float timerCouldown;
    [Space(10)]

    [Header("Radar")]
    [SerializeField] private float radiusRadar;
    [SerializeField] private LayerMask playerLayer;
    [Space(10)]

    [Header("Patrol")]
    [SerializeField] private Collider2D[] targets;
    [SerializeField] private Transform[] patrolPositions;
    [SerializeField] private int currentPoint;
    [Space(10)]

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip damageSFX;

    public GameObject roomM;

    void FixedUpdate()
    {
        timerCouldown -= Time.fixedDeltaTime; //Reduce attack cooldown
        DetectedPlayer(); // Detect and act
    }

    public void TakeDamage(int value)
    {
        health -= value;
        audioSource.PlayOneShot(damageSFX); // Play damage sound effect one time 
        //Death
        if (health < 0)
        {
            Destroy(gameObject);
            roomM.GetComponent<RoomManager>().EnemyAllive -= 1;
        }
    }
    public void DetectedPlayer()
    {
        //Detect player in circle
        targets = Physics2D.OverlapCircleAll(transform.position, radiusRadar, playerLayer);
        if (targets.Length > 0)
        {
            // if distance from player to enme more for radiuse attack, if distace low this enemy attack player
            if (Vector2.Distance(transform.position, targets[0].transform.position) > radiusAttack)
            {
                transform.position = Vector2.MoveTowards(transform.position, targets[0].transform.position, speed * Time.fixedDeltaTime);
            }
            else
            {
                Attack();
            }
        }
        else
        {
            //PatrolEnemy();
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

    void Attack()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(gameObject.transform.position, radiusAttack, playerLayer);
        if (targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (timerCouldown <= 0)
                {
                    targets[i].GetComponent<PlayerSystem>().TakeDamage(damage);
                    timerCouldown = couldown;
                }
            }
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
