using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

public class PlayerSystem : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private Rigidbody2D rb;

    [Header("Characteristic")]
    public float maxHealth;
    public float currentHealth;
    [Space(10)]

    public float maxArmmor;
    public float currentArmmor;
    [Space(10)]

    public float maxMana;
    public float currentMana;
    [Space(10)]

    [SerializeField] private float speed;
    [SerializeField] private string typeWeapoint = "Nothing";
    [SerializeField] private int damage;
    [SerializeField] private int criticalDamage;
    [SerializeField] private float range;

    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform pointWeapoint;

    [SerializeField] private GameObject[] weapoints;
    [SerializeField] private GameObject Weapoint;
    [SerializeField] private int currentWeapoint;

    private WeapointSystem weapointSystem;
    [Space(10)]

    [SerializeField] private float couldownArmorRest;

    [Header("CharacteristicUI")]
    //Characteristic of Health
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthText;
    [Space(10)]
    //Characteristic of Arrmor
    [SerializeField] private Image arrmoreBar;
    [SerializeField] private TMP_Text arrmoreText;
    [Space(10)]
    //Characteristic of Mana
    [SerializeField] private Image manaBar;
    [SerializeField] private TMP_Text manaText;
    [Space(10)]

    [Header("Help UI")]
    [SerializeField] private GameObject help_TextForTeleport;
    [SerializeField] private bool ActiveHelpTeleport;

    private float timerArrmor;
    private float horizontal, vertical;

    void Start()
    {
        if (weapoints.Length > 0) SpawnWeapoint(); // Spawn first weapoint
    }
    private void Update()
    {
        // X button (Joistick) shoot or attack
        if (Input.GetButton("Fire1"))
        {
            Attack();
        }
        // A button (Joistick) chenge weapon
        if (Input.GetButtonDown("ChengeWeapoint"))
        {
            ChengeWeapoint();
        }
        // B button (Joistick) Teleport to next level
        if (ActiveHelpTeleport)
        {
            if (Input.GetButton("NextLevel"))
            {
                gameManager.Nextlevel();
                ActiveHelpTeleport = false;
            }
        }
        help_TextForTeleport.SetActive(ActiveHelpTeleport);

        timerArrmor -= Time.deltaTime;
        UpdateCharacteristic();
        RestorationOfProtection();
    }

    void FixedUpdate()
    {
        Movment();
        
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal"))
        {
            ActiveHelpTeleport = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal"))
        {
            ActiveHelpTeleport = false;
        }
    }

    private void Movment()
    {
        //Movment player
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical);

        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }
    private void UpdateCharacteristic()
    {
        //Update health bar
        healthBar.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{maxHealth}/{currentHealth}";
        
        //Update Armmor bar
        arrmoreBar.fillAmount = currentArmmor / maxArmmor;
        arrmoreText.text = $"{maxArmmor}/{currentArmmor}";

        //Update mana bar
        manaBar.fillAmount = currentMana / maxMana;
        manaText.text = $"{maxMana}/{currentMana}";
    }

    public void TakeDamage(int damage)
    {
        if (currentArmmor > 0)
        {
            currentArmmor -= damage;
            if (currentArmmor <= 0)
            {
                currentHealth -= currentArmmor;
                currentArmmor = 0;
            }
        }
        else if (currentArmmor <= 0)
        {
            currentHealth -= damage;
        }
        if (currentHealth <= 0) gameManager.Restart(); // if player have zero or less hp than game restart

        timerArrmor = couldownArmorRest;

    }
    private void RestorationOfProtection()
    {
        if ((horizontal <= 0f || vertical <= 0f) && timerArrmor <= 0)
        {
            currentArmmor += 1;
            timerArrmor = couldownArmorRest;
            if (currentArmmor >= maxArmmor)
            {
                currentArmmor = maxArmmor;
            }
        }
    }

    private void SpawnWeapoint()
    {
        //Create weapoint at scene
        Weapoint = Instantiate(weapoints[currentWeapoint], pointWeapoint.position, Quaternion.Euler(0, 0, 0));
        Weapoint.transform.SetParent(pointWeapoint);
        weapointSystem = Weapoint.GetComponent<WeapointSystem>();
        typeWeapoint = weapointSystem.type;
    }
    private void ChengeWeapoint()
    {
        //Chenge weapoint if player have more 1 weapon
        if (weapoints.Length > 0)
        {
            currentWeapoint += 1;
            if (currentWeapoint >=  weapoints.Length)
            {
                currentWeapoint = 0;
            }
            SpawnWeapoint();
        }
        else
        {
            typeWeapoint = "Nothing";
        }
    }
    public void Attack()
    {
        if (typeWeapoint == "Nothing")
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(pointWeapoint.position, range, enemyLayer);
            if (targets.Length > 0)
            {
                for (int i = 0; i < targets.Length; i++) 
                {
                    targets[i].GetComponent<EnemySystem>().TakeDamage(damage);
                }
            }
        }
        if (typeWeapoint == "Gun")
        {
            weapointSystem.Shoot();
        }
    }

    void OnDrawGizmos()
    {
        if (typeWeapoint == "Nothing")
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(pointWeapoint.position, range);
        }
    }
}
