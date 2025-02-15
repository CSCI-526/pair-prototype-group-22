using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f; // Bullet lifetime
    public string owner_tag;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("collision");
        // Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("build_part3") || collision.gameObject.CompareTag(owner_tag))
        {
            return;
        }
        // Destroy the bullet when it collides with something
        Destroy(gameObject);
    }
}