using UnityEngine;

public class BulletLarge : MonoBehaviour
{
    public float lifeTime = 5f; // Bullet lifetime
    public string owner_tag;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collide" + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("build_part4") || collision.gameObject.CompareTag(owner_tag))
        {
            return;
        }
        // Destroy the bullet when it collides with something
        Destroy(gameObject);
    }
}