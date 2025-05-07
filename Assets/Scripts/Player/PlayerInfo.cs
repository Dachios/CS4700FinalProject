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

    bool isDead;

    void Start()
    {
        isDead = false;
        UpdateUI();
        message.gameObject.SetActive(false);
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
            AbsorbDamage(damage);
        }
        else if (damage < 0)
        {
            Heal(-damage, overheal);   // flip sign for clarity
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
