using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{

    [DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
    [SaveDuringPlay]
    public class CameraShake : CinemachineImpulseSource
    {
        public GameObject main;
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "EnemyAttack"&&main.GetComponent<HealthBarControl>().invincibleCD>0.7f)
            {
                GenerateImpulseAt(main.transform.position,new Vector3(0,1,0));
            }
            //else if (col.tag == "PlayerAttack") 
            //{
            //    GenerateImpulseAt(main.transform.position, new Vector3(0, 0.1f, 0));
            //}
        }
        private void Start()
        {
            main = GameObject.FindWithTag("Player");
        }
    }
}