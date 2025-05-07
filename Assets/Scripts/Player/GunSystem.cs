using UnityEngine;
using System.Collections;
using TMPro;

public class GunSystem : MonoBehaviour
{
    [Header("Shotgun Properties")]
    [SerializeField] private int pelletCount = 14;
    [SerializeField] private float spreadAngle = 15f;
    [SerializeField] private float range = 512f;
    [SerializeField] private int damage = 10;
    [SerializeField] private const int MAX_AMMO = 50;
    [SerializeField] private int initialAmmo = 25;
    [SerializeField] private int currentAmmo;

    [Header("Effects")]
    [SerializeField] private GameObject impactEffect;
    
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform gunModel;
    
    // Debug visualization
    [SerializeField] private bool showDebugRays = false;
    [SerializeField] private float debugRayDuration = 1f;

    public TextMeshProUGUI AmmoVal;

    private bool fireCooldown;

    // Shotgun sound variables
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private Transform shotgunAudioSourceObject;
    private AudioSource shotgunAudioSource;
    
    private void Start()
    {
        currentAmmo = initialAmmo;
        AmmoVal.text = currentAmmo.ToString();

        // Initialize shotgun audio source
        shotgunAudioSource = shotgunAudioSourceObject.GetComponent<AudioSource>();
        shotgunAudioSource.clip = fireSound;
        
        // Reference to player camera
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }
    
    private void Update()
    {
        // Fire shotgun on left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            if (currentAmmo > 0 && !fireCooldown)
            {
                Fire();
                
            }
        }
    }
    
    private void Fire()
    {

        if (shotgunAudioSource != null)
        {
            Debug.Log("Shotgun Sound played: " + shotgunAudioSource.clip.name);
            shotgunAudioSource.Play(); // Play the shotgun sound
        }
        else
        {
            Debug.LogError("Shotgun AudioSource is not assigned!");
        }

        currentAmmo--;
        AmmoVal.text = currentAmmo.ToString();
        
        
        // Fire multiple pellets with spread
        for (int i = 0; i < pelletCount; i++)
        {
            FirePellet();
        }

        
        fireCooldown = true;
        StartCoroutine(Cooldown());
    }
    
    private void FirePellet()
    {
        // Calculate random spread direction
        Vector3 spreadDirection = playerCamera.transform.forward;
        
        // Add random spread
        float randomAngleX = Random.Range(-spreadAngle, spreadAngle);
        float randomAngleY = Random.Range(-spreadAngle, spreadAngle);
        
        //Apply spread using quaternion rotation
        Quaternion randomRotation = Quaternion.Euler(randomAngleX, randomAngleY, 0);
        spreadDirection = randomRotation * spreadDirection;
        
        // Create raycast from center of camera
        Ray ray = new Ray(playerCamera.transform.position, spreadDirection);
        RaycastHit hit;
        
        // Visualize rays in editor if debug mode is enabled
        if (showDebugRays)
        {
            Debug.DrawRay(ray.origin, ray.direction * range, Color.red, debugRayDuration);
        }
        
        // Check for hits
        if (Physics.Raycast(ray, out hit, range))
        {
            
            // Apply damage if hit object has health component
            HealthComponent healthComponent = hit.collider.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);
            }
            

            // Spawn imapct effect
            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f);
            }
        }
    }
    
    // Method to add ammo (for pickup items, etc.)
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, MAX_AMMO);
        AmmoVal.text = currentAmmo.ToString();
    }
    
    //Public method for HUD to access
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.75f);
        fireCooldown = false;
    }
}
