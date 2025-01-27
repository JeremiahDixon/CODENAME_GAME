using UnityEngine;

public class Laser : Projectile
{
    private Vector2 moveDirection;
    private float speed = 6;

    public void SetDirection(Vector2 direction, float projectileSpeed)
    {
        moveDirection = direction;
        speed = projectileSpeed;
    }

    void Update()
    {
        // Move the laser in the specified direction
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // Destroy the laser if it moves off-screen
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            collision.gameObject.GetComponent<IPlayer>().TakeDamage(damage);
        }
    }
}
