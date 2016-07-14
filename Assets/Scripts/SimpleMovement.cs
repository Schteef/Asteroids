using UnityEngine;
using System.Collections;

public class SimpleMovement : MonoBehaviour {

    // Autorotation Variables
    public bool m_autoRotate = true;
    public Vector3 m_rotationSpeed = new Vector3(0, 90, 0);

    public bool m_bounce = true;
    public Vector3 m_bounceSpeed = new Vector3(0, .5f, 0);
    public Vector3 m_bounceRange = new Vector3(0, .5f, 0);
    Vector3 bounceOrigin, bounceSpeed, bounceMaxima, bounceMinima;


	// Use this for initialization
	void Start () {
        bounceOrigin = transform.localPosition;
        bounceSpeed = new Vector3(1f, 1f, 1f);
        bounceMaxima = new Vector3(
                bounceOrigin.x + (m_bounceRange.x / 2f),
                bounceOrigin.y + (m_bounceRange.y / 2f),
                bounceOrigin.z + (m_bounceRange.z / 2f)
            );
        bounceMinima = new Vector3(
                bounceOrigin.x - (m_bounceRange.x / 2f),
                bounceOrigin.y - (m_bounceRange.y / 2f),
                bounceOrigin.z - (m_bounceRange.z / 2f)
            );

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_autoRotate)
            transform.Rotate(m_rotationSpeed.x * Time.deltaTime, m_rotationSpeed.y * Time.deltaTime, m_rotationSpeed.z * Time.deltaTime);

        if (m_bounce)
        {
            Vector3 newlocalPosition = new Vector3(transform.localPosition.x + (bounceSpeed.x * m_bounceSpeed.x * Time.deltaTime),
                                              transform.localPosition.y + (bounceSpeed.y * m_bounceSpeed.y * Time.deltaTime),
                                              transform.localPosition.z + (bounceSpeed.z * m_bounceSpeed.z * Time.deltaTime));
            if (newlocalPosition.x > bounceMaxima.x || newlocalPosition.x < bounceMinima.x) bounceSpeed.x *= -1f;
            if (newlocalPosition.y > bounceMaxima.y || newlocalPosition.y < bounceMinima.y) bounceSpeed.y *= -1f;
            if (newlocalPosition.z > bounceMaxima.z || newlocalPosition.z < bounceMinima.z) bounceSpeed.z *= -1f;
            transform.localPosition = newlocalPosition;
            // Bounce Dimension
            // if we are outside of the bounce range, reverse the sign on the bounce speed for the axis.
            // update our position based on the bounce speed and deltatime.
        }
    }
}
