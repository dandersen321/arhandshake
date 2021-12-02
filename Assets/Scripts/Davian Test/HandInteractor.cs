using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInteractor : MonoBehaviour
{
    public Transform pointOfContact;
    public Vector3 originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    private void OnTriggerStay(Collider other) {     
        var yourHandPointOfContact = other.gameObject.transform.GetChild(1);
        if(transform.position != yourHandPointOfContact.position){
            transform.position = Vector3.MoveTowards(transform.position, yourHandPointOfContact.transform.position, 30.0f * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other) {
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, 30.0f * Time.deltaTime);
    }
}
