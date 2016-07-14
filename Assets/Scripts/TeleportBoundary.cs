using UnityEngine;
using System.Collections;

public class TeleportBoundary : MonoBehaviour {

    // Calculate boundary coordinates
    public Bounds boundary;

    void Start()
    {
        Vector3 viewport = new Vector3(0, 0, 30);
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(viewport);
                                        
        gameObject.transform.localScale = new Vector3(Mathf.Abs(bottomLeft.x) * 2, 1f, Mathf.Abs(bottomLeft.z * 2));

        boundary = new Bounds(transform.position, transform.localScale);
    }
}
