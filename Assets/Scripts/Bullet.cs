using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    [SerializeField] private float speed;
    [SerializeField] private float time;

    void Start()
    {
        Destroy(gameObject, time); // Destroy bullet after lifetime
    }
    void FixedUpdate()
    {
        //Move forward in local direction 
        transform.Translate(speed * Time.fixedDeltaTime, 0, 0);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemySystem>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
