using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    [SerializeField] private float speed;
    [SerializeField] private float time;
    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        Destroy(gameObject, time);
    }
    void FixedUpdate()
    {
        transform.Translate(speed * Time.fixedDeltaTime, 0, 0);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemySystem>().TakeDamage(damage);
            
        }
    }
}
