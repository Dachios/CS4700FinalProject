using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private int AMMO_RECOVER = 25;
    [SerializeField] private bool respawnable = false;
    [SerializeField] private int RESPAWN_TIME = 30;

    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Probably will put collision checks here once the player script is done.

    }

    void OnTriggerEnter(Collider collision)
    {
        collision = player.GetComponent<Collider>();

        if (collision.tag == "Player")
        {
            if (player.TryGetComponent(out GunSystem playerAmmo))
            {
                playerAmmo.AddAmmo(5);


                
            }
            gameObject.SetActive(false);
            
        }


    }
}
