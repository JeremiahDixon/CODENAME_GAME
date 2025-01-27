using UnityEngine;

public class MushroomAttack : MonoBehaviour
{
    public float growthDuration = 0.1f; // Time it takes to grow to full size
    public float shrinkDuration = 0.1f; // Time it takes to shrink back
    public float maxScale = 1.25f; // Maximum scale of the mushroom
    public int damage = 10; // Damage to the player

    private Vector3 initialScale;
    //private bool isGrowing = true;

    private void Start()
    {
        initialScale = new Vector3(transform.localScale.x * 0.25f, transform.localScale.y * 0.25f, transform.localScale.z * 0.25f);
        StartCoroutine(GrowAndShrink());
    }

    private System.Collections.IEnumerator GrowAndShrink()
    {
        float elapsedTime = 0f;

        // Growth phase
        while (elapsedTime < growthDuration)
        {
            float progress = elapsedTime / growthDuration;
            transform.localScale = Vector3.Lerp(initialScale, Vector3.one * maxScale, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it's fully grown
        transform.localScale = Vector3.one * maxScale;

        // Enable the collider during maximum size
        GetComponent<Collider2D>().enabled = true;

        elapsedTime = 0f;

        // Shrinking phase
        while (elapsedTime < shrinkDuration)
        {
            float progress = elapsedTime / shrinkDuration;
            transform.localScale = Vector3.Lerp(Vector3.one * maxScale, initialScale, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it's fully shrunk
        transform.localScale = initialScale;

        // Disable the collider while shrinking
        GetComponent<Collider2D>().enabled = false;

        // Destroy the mushroom after the cycle
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Assume the player has a script with a TakeDamage method
            other.GetComponent<IPlayer>().TakeDamage(damage);
        }
    }
}
