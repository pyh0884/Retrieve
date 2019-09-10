using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBKG : MonoBehaviour
{
    private float parallaxScales;
    public float smoothing = 1f;
    private Transform cam;
    private Vector3 previousCamPos;
    private void Awake()
    {
        cam = Camera.main.transform;
    }
    void Start()
    {
        previousCamPos = cam.position;
        parallaxScales= transform.position.z * -1;
    }

    void Update()
    {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales;
        float backgroundTargetPosX = transform.position.x + parallax;

        Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, backgroundTargetPos, smoothing * Time.deltaTime);
        previousCamPos = cam.position;
    }
}