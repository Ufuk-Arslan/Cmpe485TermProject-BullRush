using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cane : MonoBehaviour
{
    public static bool canFire = true;
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
        canFire = other.tag != "Collide";
    }
    private void OnTriggerExit(Collider other)
    {
        canFire = true;
    }
}
