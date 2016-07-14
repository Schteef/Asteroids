    using UnityEngine;
using System.Collections;

public class AsteroidController : MonoBehaviour {

    // Movement Variables
    public float m_rotation = 90f;
    public float m_speed = 10f;
    public float m_maxSpeed = 10f;   
    Rigidbody rb;

    // Asteroid Children Variables
    public GameObject m_childPrefab;
    public int m_childCount;

    // Death Variables
    public ParticleSystem m_explosionPrefab;
    public int m_pointsValue = 20;

    // Use this for initialization
    void Start () {
        // Give the asteroid a rotation
        // Random Rotation
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Random.insideUnitSphere * Random.Range(-m_rotation, m_rotation);

        // Move in a random direction
        Vector3 velocity = new Vector3(
            Random.Range(-m_speed, m_speed),
            0f,
            Random.Range(-m_speed, m_speed)
            );

        rb.velocity = velocity;

        
    }

    void Update()
    {
        if (rb.velocity.magnitude > 10)
            rb.velocity = rb.velocity.normalized * m_maxSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            GameObject.FindWithTag("GameController").GetComponent<PlayerManager>().AwardPoints(m_pointsValue);
        }
        if (collision.collider.tag != "Asteroid")
        {
            if (m_childPrefab != null)
                SpawnChildren();
            Die(0f);            
        }
    }

    void SpawnChildren()
    {
        while(m_childCount > 0)
        {
            Instantiate(m_childPrefab, transform.position, transform.localRotation);
            m_childCount--;
        }
    }

    public void Die(float when)
    {
        ParticleSystem explosion = Instantiate(m_explosionPrefab, transform.position, transform.rotation) as ParticleSystem;
        Destroy(explosion.gameObject, explosion.duration);
        // So long cruel world!
        Destroy(gameObject, when);
    }
}
