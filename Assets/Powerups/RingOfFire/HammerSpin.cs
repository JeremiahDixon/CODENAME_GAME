using UnityEngine;

public class HammerSpin : MonoBehaviour
{
    [SerializeField] GameObject hammerPrefab; // Reference to the hammer prefab
    int numberOfHammers = 1; // Number of hammers (scales with level)
    float spinSpeed = 180f; // Speed of the spin
    float radius = 1.25f; // Distance of the hammers from the player
    GameObject[] hammers; // Array to store active hammers
    IPlayer player;

    void Start()
    {
        player = GameManager.Instance.thePlayer;
        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.position = player.transform.position;
        SpawnHammers(20); // Initial hammer spawn
    }

    void Update()
    {
        RotateHammers(); // Rotate the hammers around the player
    }

    void SpawnHammers(int damage)
    {
        // Destroy existing hammers if any
        if (hammers != null)
        {
            foreach (GameObject hammer in hammers)
            {
                if (hammer != null) Destroy(hammer);
            }
        }

        hammers = new GameObject[numberOfHammers];

        // Spawn hammers evenly spaced around the player
        for (int i = 0; i < numberOfHammers; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfHammers; // Calculate angle for each hammer
            Vector3 position = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            GameObject hammer = Instantiate(hammerPrefab, position, Quaternion.identity);
            hammer.transform.parent = this.transform;
            hammer.GetComponent<Hammer>().IncreaseDamage(damage);
            hammers[i] = hammer;
        }
    }

    void RotateHammers()
    {
        // Rotate each hammer around the player
        for (int i = 0; i < hammers.Length; i++)
        {
            if (hammers[i] != null)
            {
                hammers[i].transform.RotateAround(transform.position, Vector3.forward, spinSpeed * Time.deltaTime);
            }
        }
    }

    // Call this to update the number of hammers (e.g., when the ability levels up)
    public void LevelUpAbility(int newHammerCount, int damage, float radius )
    {
        numberOfHammers = newHammerCount;
        this.radius = radius;
        SpawnHammers(damage);
    }
    
}
