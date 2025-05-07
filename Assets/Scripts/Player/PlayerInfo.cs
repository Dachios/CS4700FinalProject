// Script by Luca, modifications by Dachi
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour
{
    [Header("Limits")]
    public int maxHealth = 200;
    public int maxArmor  = 200;
    public int healthSoftCap = 100;
    public int armorSoftCap  = 100;

    [Header("Current values")]
    public int health = 100;
    public int armor  = 50;

    [Header("UI")]
    public TextMeshProUGUI HPVal;
    public TextMeshProUGUI ARVal;
    public TextMeshProUGUI message;
<<<<<<< HEAD

    bool isDead;
=======
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Player damage audio variables
    [SerializeField] private AudioClip[] damageSounds;
    //[SerializeField] private Transform damageAudioSourceObject;
    [SerializeField] private AudioSource damageAudioSource;

    // Player heal audio variables
    [SerializeField] private AudioClip healSound;
    [SerializeField] private Transform healAudioSourceObject;
    private AudioSource healAudioSource;
>>>>>>> bngo

    void Start()
    {
        isDead = false;
        UpdateUI();
        message.gameObject.SetActive(false);

        // Initialize damage audio source
        damageAudioSource = GetComponent<AudioSource>();
        // Initialize heal audio source
        healAudioSource = healAudioSourceObject.GetComponent<AudioSource>();
        healAudioSource.clip = healSound;
    }

    void Update()
    {
        if (isDead && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // positive damage hurts, negative damage heals
    // overheal true allows going above soft caps
    public void TakeDamage(int damage, bool overheal = false)
    {
        if (damage > 0)
        {
<<<<<<< HEAD
            AbsorbDamage(damage);
        }
        else if (damage < 0)
        {
            Heal(-damage, overheal);   // flip sign for clarity
=======
            health -= damage;
            HPVal.text = health.ToString();

            // Play a random damage sound
            if (damageSounds.Length > 0 && damageAudioSource != null)
            {
                int randomIndex = Random.Range(0, damageSounds.Length);
                damageAudioSource.PlayOneShot(damageSounds[randomIndex]);
            }
        } 
        else if (damage < 0 ) // Heal
        {
            health -= damage;

            // Play heal sound
            // Here instead of on HealthPickup because the gameobject is destroyed too quickly
            healAudioSource.PlayOneShot(healSound);

            if (health > healthSoftCap && !overheal) //If the entity interacted with doesn't have an overheal attribute, the health won't go above the softcap.
            {
                if ((health + damage) <= healthSoftCap) 
                {
                    health = healthSoftCap;
                    Debug.Log("Health at softcap");
                }
                else
                {
                    health += damage;
                    Debug.Log("Health Beyond Softcap");
                }
            }

            if (health > MAX_HEALTH && overheal) // If health surpasses max health from an entity and it is overheal, cap it out at max.
            {
                health = MAX_HEALTH;
            }
            HPVal.text = health.ToString();
>>>>>>> bngo
        }

        UpdateUI();
        CheckDeath();
    }

    void AbsorbDamage(int dmg)
    {
        // armor soaks first
        int leftover = Mathf.Max(dmg - armor, 0);
        armor = Mathf.Max(armor - dmg, 0);
        health = Mathf.Max(health - leftover, 0);
    }

    void Heal(int amount, bool overheal)
    {
        health += amount;
        if (!overheal && health > healthSoftCap) health = healthSoftCap;
        if (overheal && health > maxHealth)      health = maxHealth;
    }

    void UpdateUI()
    {
        HPVal.text = health.ToString();
        ARVal.text = armor.ToString();
    }

    void CheckDeath()
    {
        if (health > 0 || isDead) return;

        isDead = true;
        message.gameObject.SetActive(true);
        Debug.Log("Player died");
        Destroy(gameObject);   // remove model, UI still shows message
    }
}
