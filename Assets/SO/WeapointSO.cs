using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeapointSO", menuName = "Weapoint/WeapointSO")]
public class WeapointSO : ScriptableObject
{
    public string type;
    public string name;
    public Image imageWeapoint;
    public int damage;
    public int criticalDamage;
    public float radiusDetect;
    public float radiusAttack;
    public float speedRotation;
    public LayerMask enemyLayer;
    public GameObject bulletPrefab;
}
