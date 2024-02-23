using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    new private Rigidbody rigidbody;

    [SerializeField]
    private float torque;

    private bool didHit=false;
    
    [SerializeField]
    private string bullTag;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip collisionSound;

    [SerializeField]
    private Collider collider_body;
    private Collider collider_tip;


    public void Fly(Vector3 force)
    {
        rigidbody.isKinematic = false;
        rigidbody.AddForce(force, ForceMode.Impulse);
        rigidbody.AddTorque(transform.right * torque);
        transform.SetParent(null);
    }

    public void DeactiveHit()
    {
        if (collider_body != null)
            collider_body.enabled = true;
        if (collider_tip != null)
            collider_tip.enabled = false;
    }
    
    void OnTriggerEnter(Collider collider)
    {
        //if (collider.CompareTag("Player")) return; Done on project config
        if (didHit) return;
        if(collider.CompareTag("Door")||collider.CompareTag("Wall")||collider.CompareTag("Terrain")){
            didHit=true;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.isKinematic = true;
            transform.SetParent(collider.transform); // Make arrow stick to the collider
        }
        
        //if (collider.CompareTag(bullTag))
        //{
        //      Do something in case the arrow hit a bull.
        //}
        
       
    }
    
    
}
