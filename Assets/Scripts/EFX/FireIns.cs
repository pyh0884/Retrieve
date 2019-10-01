using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireIns : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public GameObject fire;
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }
    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        int i = 0;
        while (i < numCollisionEvents)
        {
            if (other.layer==8)
                Instantiate(fire, new Vector3(Mathf.RoundToInt(collisionEvents[i].intersection.x*2)/2f, collisionEvents[i].intersection.y), Quaternion.identity);
            
            i++;
        }
    }
}
