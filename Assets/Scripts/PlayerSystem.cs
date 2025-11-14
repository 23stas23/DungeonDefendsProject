using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSystem : MonoBehaviour
{
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
    [SerializeField] private int damage;
    [SerializeField] private int criticalDamage;
    [Space(10)]

    [SerializeField] private float couldownArmorRest;

    [Header("CharacteristicUI")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthText;
    [Space(10)]

    [SerializeField] private Image arrmoreBar;
    [SerializeField] private TMP_Text arrmoreText;
    [Space(10)]

    [SerializeField] private Image manaBar;
    [SerializeField] private TMP_Text manaText;
    [Space(10)]

    private float timerArrmor;

    private Rigidbody2D rb;
    private float horizontal, vertical;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {

        Movment();
        UpdateCharacteristic();
        RestorationOfProtection();
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
        healthBar.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{maxHealth}/{currentHealth}";
        Debug.Log(currentHealth / maxHealth);

        arrmoreBar.fillAmount = currentArmmor / maxArmmor;
        arrmoreText.text = $"{maxArmmor}/{currentArmmor}";

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
        
    }
    private void RestorationOfProtection()
    {
        if ((horizontal >= 0.1f || vertical >= 0.1f) && timerArrmor <= 0)
        {
            currentArmmor += 1;
            timerArrmor = couldownArmorRest;
            if (currentArmmor >= maxArmmor)
            {
                currentArmmor = maxArmmor;
            }
        }
    }

    public void AddHealth(int addVoulum)
    {
        currentHealth += addVoulum;
        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public void AddMana(int addVoulum)
    {
        currentMana += addVoulum;
        if(currentMana >= maxMana)
        {
            currentMana = maxMana;
        }
    }
}
