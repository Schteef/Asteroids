using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    Rigidbody m_rigidbody;

    // Movement Variables
    public float m_horizontalInput;
    public float m_verticalInput;
    public float m_rotationSpeed;
    public float m_thrusterForce;

    // Weapon Variables
    public Transform m_cannon;
    public GameObject m_laserPrefab;
    public float m_fireDelay;
    float canFire;
    public AudioSource m_laserSounds;

    // Teleport Variables
    public ParticleSystem m_teleportPrefab;
    public AudioSource m_teleportSounds;
    public float m_teleportCooldown;
    public float m_teleportFailureChance;
    float nextTeleport = 0;
    public float telefragProbability = 0;

    // Player Death
    public ParticleSystem m_explosionPrefab;
    public bool m_IsDead = false;
    public float m_TimeOfDeath;

    // Use this for initialization
    void Start() {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        // Get User Input
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
        if (Input.GetButton("Fire1"))
            FireWeapon();
        if (Input.GetButton("Fire2"))
            Teleport();
    }


    void FixedUpdate()
    {
        // Convert User Input into movement
        RotateShip();
        FireThrusters();
    }

    void FireWeapon()
    {
        if (Time.time > canFire)
        {
            // Fire the weapon
            Instantiate(m_laserPrefab, m_cannon.position, m_cannon.rotation);
            canFire = Time.time + m_fireDelay;
            m_laserSounds.Play();
        }
    }

    Vector3 RotateShip()
    {
        // Calculate how much to turn based on the rotation speed and input.
        float turn = m_horizontalInput * m_rotationSpeed * Time.deltaTime;

        // Make the rotation into a rotation around the Y axis
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply the rotation to the ship
        m_rigidbody.MoveRotation(m_rigidbody.rotation * turnRotation);

        return m_rigidbody.rotation.eulerAngles;
    }


    Vector3 FireThrusters()
    {
        // Apply a force to the object
        m_rigidbody.AddForce(m_rigidbody.transform.forward * m_thrusterForce * m_verticalInput);

        if (m_rigidbody.velocity.magnitude > m_thrusterForce)
        {
            Vector3 velocity = m_rigidbody.velocity.normalized * m_thrusterForce;
            m_rigidbody.velocity = velocity;
        }
        return m_rigidbody.velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        Die();
    }

    void Die()
    {
        if (!m_IsDead)
        {
            ParticleSystem explosion = Instantiate(m_explosionPrefab, transform.position, transform.rotation) as ParticleSystem;
            Destroy(explosion.gameObject, explosion.duration);
            // So long cruel world!
            m_IsDead = true;
            m_TimeOfDeath = Time.time;
            gameObject.SetActive(false);
        }
    }

    void Teleport()
    {
        if (Time.time > nextTeleport)
        {
            telefragProbability = telefragProbability + m_teleportFailureChance;
            if (Random.Range(0, 100) < telefragProbability)
            {
                Die();
            }
            else
            {
                m_teleportPrefab.Play();
            
                nextTeleport = Time.time + m_teleportCooldown;
                Bounds bounds = GameObject.FindWithTag("Boundary").GetComponent<TeleportBoundary>().boundary;
                Vector3 newPosition = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    0f,
                    Random.Range(bounds.min.z, bounds.max.z));

                m_rigidbody.MovePosition(newPosition);
                m_teleportSounds.Play();
            }

        }
    }
}
