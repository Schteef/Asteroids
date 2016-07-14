using UnityEngine;
using System.Collections;

public class TeleportableObject : MonoBehaviour {

    TeleportBoundary arena;

    public float m_cooldown = 0.3f;
    float m_nextTeleport;

	// Use this for initialization
	void Start () {
        // Find the Boundary

        arena = GameObject.FindWithTag("Boundary").GetComponent<TeleportBoundary>();

	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > m_nextTeleport)
            if (!arena.boundary.Contains(gameObject.transform.position))
                Teleport(); 
	}

    void Teleport()
    {
        Vector3 position = transform.position;

        // Handle Horizontal Teleportation
        if (transform.position.x < arena.boundary.min.x)
             position = new Vector3(arena.boundary.max.x, transform.position.y, transform.position.z);
        else if (transform.position.x > arena.boundary.max.x)
            position = new Vector3(arena.boundary.min.x, transform.position.y, transform.position.z);       

        // Handle Vertical Teleportation
        if (transform.position.z < arena.boundary.min.z)        
            position = new Vector3(transform.position.x, transform.position.y, arena.boundary.max.z);
        else if (transform.position.z > arena.boundary.max.z)
            position = new Vector3(transform.position.x, transform.position.y, arena.boundary.min.z);            

        transform.position = position;
        m_nextTeleport = Time.time + m_cooldown;
    }
}
