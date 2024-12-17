using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Collision checker has been set up");
    }


    private void OnCollisionEnter(Collision collision)
    {
        // Log the name of the colliding object
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        // Check for specific tags (optional)
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with a Player!");
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with an Enemy!");
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collided with an Obstacle!");
        }
        else
        {
            Debug.Log("Collided with an untagged object.");
        }

        // Log collision points
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.Log($"Collision point: {contact.point}");
        }

        // Log the relative velocity of the collision
        Debug.Log("Relative velocity: " + collision.relativeVelocity);
    }
}
