using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    /* 
    // Wanted to add something but the next scene loads before the full sound plays
    [SerializeField] private AudioClip teleportSound;
    [SerializeField] private AudioSource teleportAudioSource;

    private void Start()
    {
        teleportAudioSource.clip = teleportSound;
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        //teleportAudioSource.Play();

        Debug.Log("Trigger entered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected. Loading scene: " + sceneToLoad);
            SceneManager.LoadScene("Assets/Scenes/Levels/" + sceneToLoad + "/" + sceneToLoad + ".unity");
        }
    }
}
