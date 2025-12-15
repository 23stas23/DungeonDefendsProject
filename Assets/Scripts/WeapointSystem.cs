using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class WeapointSystem : MonoBehaviour
{
    public string type;
    public string name;
    public Image imageWeapoint;
    public int damage;
    public int criticalDamage;
    public float radiusDetect;
    public float speedRotation;
    
    public LayerMask enemyLayer;
    public GameObject bulletPrefab;

    [SerializeField] private float coulDown;
    private float timerCoulDown;

    [SerializeField] private Transform shootPoint;
    private Vector2 direction;
    private float angle;


    void FixedUpdate()
    {
        timerCoulDown -= Time.fixedDeltaTime;
        DetectedEnemy();
    }

    private void DetectedEnemy()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(transform.position, radiusDetect, enemyLayer);
        if (enemy.Length > 0)
        {
            direction = enemy[0].transform.position - transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            
        }
        else
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            direction = new Vector2(horizontal, vertical);

            if (direction.magnitude > 0.1f)
            {
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    public void Shoot()
    {
        if (timerCoulDown <= 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.Euler(0, 0, angle));
            bullet.GetComponent<Bullet>().damage = damage;
            timerCoulDown = coulDown;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusDetect);
    }
}
