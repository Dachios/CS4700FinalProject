using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected. Loading scene: " + sceneToLoad);
            SceneManager.LoadScene("Assets/Scenes/Levels/" + sceneToLoad + "/" + sceneToLoad + ".unity");
        }
    }
}
