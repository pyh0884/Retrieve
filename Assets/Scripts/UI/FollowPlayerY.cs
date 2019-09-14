using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerY : MonoBehaviour
{
    GameObject player;
    public float smooth;
    private Transform cam;
    private Vector3 previousCamPos;
    private void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        previousCamPos = cam.position;
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    private void FixedUpdate()
    {//if(!player) player = GameObject.FindGameObjectWithTag("Player");
        float parallax = (previousCamPos.y - cam.position.y) * -1;
        float backgroundTargetPosY = transform.position.y + parallax;
        Vector3 backgroundTargetPos = new Vector3(transform.position.x, backgroundTargetPosY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, backgroundTargetPos, smooth * Time.deltaTime);
        previousCamPos = cam.position;

        //transform.position = new Vector3(transform.position.x, player.transform.position.y + 5, transform.position.z);

        //transform.position = Vector3.Lerp(transform.position,new Vector3(transform.position.x,player.transform.position.y+5,transform.position.z), smooth * Time.deltaTime);
    }

}
