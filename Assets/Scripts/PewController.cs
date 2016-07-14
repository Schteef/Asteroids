using UnityEngine;
using System.Collections;

public class PewController : MonoBehaviour {

    public float m_speed;
    public float m_range;

	// Use this for initialization
	void Start () {
        // Make the laser move

        GetComponent<Rigidbody>().velocity = transform.forward * m_speed;
        Destroy(gameObject, m_range);
	}


    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
