 using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    public AudioSource m_weaponSound;
    public Material m_normal;
    public Material m_phased;

    public float m_FireRate = 5f;
    float nextShot;
    public float m_rotationSpeed = 45f;
    public float m_movementSpeed = 5f;

    public Vector3 m_origin;
    public Vector3 m_destination;

    public Transform m_leftCannon;
    public Transform m_rightCannon;

    public bool isSmart;
    public float m_targetDuration;
    public float nextTarget;


    public ParticleSystem m_enemyDeath;
    public Transform m_Target;
    public Vector3 m_TargetPosition;
    public GameObject m_weaponPrefab;

    public float m_shieldRadius = 5;

    public int m_pointsValue = 500;

    bool phased = false;

    /*
     * Enemy Ship Behaviour
     * 
     * Enter from a random point on the boundary, and fly towards a random point on the boundary
     * If an asteroid is in range, PhaseOut.  If not, PhaseIn
     * Fire weapon every few seconds.
     * Select a random game object and rotate towards it.
     * 
     * 
     * Remove self from the game when outside of the boundary
     */

    TeleportBoundary boundary;

    // Use this for initialization
    void Start() {
        boundary = GameObject.FindGameObjectWithTag("Boundary").GetComponent<TeleportBoundary>();
        RandomizeOriginAndDestination();
        Vector3 velocity = CalculateInitialVelocity();

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = velocity;
        rb.MovePosition(m_origin);
        isSmart = false;
    }

    public void SmartenUp()
    {
        isSmart = true;
        m_FireRate = m_FireRate / 2;
        m_movementSpeed = m_movementSpeed * 2;
        transform.localScale = Vector3.one;
        m_pointsValue = m_pointsValue * 2;
    }

    void RandomizeOriginAndDestination()
    {
        int edge = Random.Range(1, 4);

        switch (edge)
        {
            case 1: LeftStart(); break;
            case 2: RightStart(); break;
            case 3: TopStart(); break;
            case 4: BottomStart(); break;
        }

    }

    Vector3 CalculateInitialVelocity()
    {
        Vector3 distance = m_destination - m_origin;
        Vector3 velocity = distance.normalized * m_movementSpeed;
        return velocity;
    }

    void LeftStart()
    {
        m_origin = new Vector3(boundary.boundary.min.x, 0, Random.Range(boundary.boundary.min.z, boundary.boundary.max.z));
        m_destination = new Vector3(boundary.boundary.max.x, 0, Random.Range(boundary.boundary.min.z, boundary.boundary.max.z));
    }

    void RightStart()
    {
        m_origin = new Vector3(boundary.boundary.max.x, 0, Random.Range(boundary.boundary.min.z, boundary.boundary.max.z));
        m_destination = new Vector3(boundary.boundary.min.x, 0, Random.Range(boundary.boundary.min.z, boundary.boundary.max.z));
    }

    void TopStart()
    {
        m_origin = new Vector3(Random.Range(boundary.boundary.min.x, boundary.boundary.max.x), 0, boundary.boundary.min.z);
        m_destination = new Vector3(Random.Range(boundary.boundary.min.x, boundary.boundary.max.x), 0, boundary.boundary.max.z);
    }

    void BottomStart()
    {
        m_origin = new Vector3(Random.Range(boundary.boundary.min.x, boundary.boundary.max.x), 0, boundary.boundary.max.z);
        m_destination = new Vector3(Random.Range(boundary.boundary.min.x, boundary.boundary.max.x), 0, boundary.boundary.min.z);
    }

    // Update is called once per frame
    void Update() {



        TargetObject();
        RotateShip();

        UpdateShields();

        if (Time.time > nextShot)
        {
            nextShot = Time.time + m_FireRate;
            FireWeapons();
        }
    }

    void TargetObject()
    {

        if (isSmart)
        {
            m_Target = GameObject.FindWithTag("Player").transform;
            m_TargetPosition = m_Target.position;
        }
        else
        {
            // Target Random Object
            if (Time.time > nextTarget)
            {
                nextTarget = Time.time + m_targetDuration;
                Collider[] objects = Physics.OverlapBox(boundary.boundary.center, boundary.boundary.extents);
                int index = Random.Range(0, objects.Length - 1);
                m_Target = objects[index].transform;
                m_TargetPosition = m_Target.position;
            }
        }
    }

    void RotateShip()
    {
        // Rotate ship towards the selected target.
        Vector3 direction = (m_TargetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * m_rotationSpeed);
    }

    void UpdateShields()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, m_shieldRadius);
        bool asteroidInRange = false;
        foreach (Collider o in objects)
        {
            asteroidInRange = o.tag == "Asteroid";
            if (asteroidInRange)
                break;
        }

        if (asteroidInRange)
            PhaseOut();
        else
            PhaseIn();
    }

    void PhaseOut()
    {
        phased = false;
        // Disable Colldier
        GetComponent<CapsuleCollider>().enabled = false;
        // Swap Alpha on Texture
        GetComponent<MeshRenderer>().material = m_phased;
    }

    void PhaseIn()
    {
        // Enable Colldier
        GetComponent<CapsuleCollider>().enabled = true;
        // Swap Alpha on Texture
        GetComponent<MeshRenderer>().material = m_normal;
    }

    void FireWeapons()
    {
        // Fire the weapon
        Instantiate(m_weaponPrefab, m_leftCannon.position, m_leftCannon.rotation);
        Instantiate(m_weaponPrefab, m_rightCannon.position, m_rightCannon.rotation);
        m_weaponSound.Play();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {            
            GameObject.FindWithTag("GameController").GetComponent<PlayerManager>().AwardPoints(m_pointsValue);
        }
        Die();
    }

    public void Die()
    {

        Destroy(gameObject);
        ParticleSystem explosion = Instantiate(m_enemyDeath, transform.position, transform.rotation) as ParticleSystem;
        Destroy(explosion.gameObject, explosion.duration);
        
    }
}
