using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCam : MonoBehaviour
{
    Camera cam;
    Ray ray;
    RaycastHit hit;
    public Vector3 dest;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ray = new Ray(cam.transform.position, cam.transform.forward);
        Physics.Raycast(ray, out hit);
        dest = hit.point;
    }
}
