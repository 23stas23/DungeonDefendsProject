using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class WeapointSystem : MonoBehaviour
{
    public string type; //Type of weapoint (Gun, Sword)
    public int damage;
    public float radiusDetect;// Radiuse to detect enemis
    public float speedRotation; // Rotation speed of weapon
    
    public LayerMask enemyLayer;
    public GameObject bulletPrefab;

    [SerializeField] private float coulDown; // cooldown between shots
    private float timerCoulDown;

    [SerializeField] private Transform shootPoint; // position where bullet spawn
    private Vector2 direction;
    private float angle;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSFX;


    void FixedUpdate()
    {
        timerCoulDown -= Time.fixedDeltaTime; // Reduce cooldown timer
        DetectedEnemy();// Try to detect enemy and rotate weapoint
    }

    private void DetectedEnemy()
    {
        //Detect enemies inside radius
        Collider2D[] enemy = Physics2D.OverlapCircleAll(transform.position, radiusDetect, enemyLayer);
        
        if (enemy.Length > 0)
        {
            direction = enemy[0].transform.position - transform.position; // Direction from weapon to enemy
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Conver direction to angle
            transform.rotation = Quaternion.Euler(0, 0, angle); //Rotate weapoint 
            
        }
        else
        {
            //if no enemy - rotate based on player movment
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
        if (timerCoulDown <= 0)// Check cooldown
        {
            //Create bullet in scene
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.Euler(0, 0, angle));
            bullet.GetComponent<Bullet>().damage = damage; // Pass damage to bullet
            timerCoulDown = coulDown; // Reset cooldown

            audioSource.PlayOneShot(shootSFX);//Play audio efect shoot one time 
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusDetect);
    }
}
