// Script by Luca, modifications by Dachi.
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour
{

    private const int MAX_HEALTH = 200;
    private const int MAX_ARMOR = 200;



    private const int healthSoftCap = 100;
    private const int armorSoftCap = 100;
    public int health = 100;
    public int armor = 50;


    private bool isDead;
    public TextMeshProUGUI HPVal;
    public TextMeshProUGUI ARVal;

    public TextMeshProUGUI message;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Player damage audio variables
    [SerializeField] private AudioClip[] damageSounds;
    //[SerializeField] private Transform damageAudioSourceObject;
    [SerializeField] private AudioSource damageAudioSource;

    // Player heal audio variables
    [SerializeField] private AudioClip healSound;
    [SerializeField] private Transform healAudioSourceObject;
    private AudioSource healAudioSource;

    void Start()
    {
        // Player doesn't start dead.
        isDead = false;

        // Initialize relevant values
        HPVal.text = health.ToString();
        ARVal.text = armor.ToString();
        //AmmoVal.text = MAX_AMMO.ToString();

        message.gameObject.SetActive(false);

        // Initialize damage audio source
        damageAudioSource = GetComponent<AudioSource>();
        // Initialize heal audio source
        healAudioSource = healAudioSourceObject.GetComponent<AudioSource>();
        healAudioSource.clip = healSound;
    }

    void Update ()
    {
        if (isDead == true)
        {
            // Cant seem to get this working!
            Debug.Log("Player is dead.");
            if(Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            
        }
    }
    public void TakeDamage(int damage, bool overheal)
    {
        if (health > 0 && damage > 0) // Hurt
        {
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
        }


        Debug.Log("Player Health: " + health);
        if (health <= 0)
        {

            health = 0;
            HPVal.text = health.ToString();

            isDead = true;
            message.gameObject.SetActive(true);

            Destroy(gameObject);

            //Debug.Log("Player Died");
        }
    }
}
