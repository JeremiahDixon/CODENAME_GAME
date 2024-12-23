using UnityEngine;

public class TheCamera : MonoBehaviour
{
    public static TheCamera Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keeps this manager between scenes
        }
        else
        {
            Destroy(gameObject);  // Prevent duplicates
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
