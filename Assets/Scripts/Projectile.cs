using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WarriorAnimsFREE
{
    public class Projectile : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Level")
            {
                Destroy(gameObject);
            } else if (other.tag == "Boss")
            {
                //Deal damage to boss
                Destroy(gameObject);
            }
        }
    }
}
