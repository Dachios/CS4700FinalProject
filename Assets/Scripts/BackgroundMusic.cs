using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic isntance;

    private void Awake()
    {
        if (isntance == null)
        {
            DontDestroyOnLoad(gameObject);
            isntance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
