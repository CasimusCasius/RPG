using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;
    
   
    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            MoveToCursor();
        }
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        Vector3 velociy = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection( velociy ); //konwersja na lokaln¹
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("fowardSpeed", speed);
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            GetComponent<NavMeshAgent>().destination = hit.point;
        }
    }
}
