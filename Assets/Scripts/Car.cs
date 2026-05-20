using System;
using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

public class Car : MonoBehaviour
{
   private Vector3 startPosition;
   [SerializeField] [Self] private Rigidbody  rb;
   private Vector3 direction;
   public float speed = 1f;
   float turnSpeed = 100f;

   private void Start()
   {
      direction = new Vector3(0, 0, 0);
      startPosition = rb.position;
      
   }

   private void Update()
   {
     
   }

   void FixedUpdate()
   {
      Vector2 moveVec = InputHandler.Instance.MoveInput;
      
      
      
      if (moveVec.magnitude > 0.1f)
      {
         direction = new Vector3(moveVec.x, 0, moveVec.y).normalized;
         rb.velocity = new Vector3(0, 0, direction.z * speed);
         transform.position = rb.position;
         
         
         float turnAmount = direction.x * turnSpeed * Time.fixedDeltaTime;
         Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
         rb.MoveRotation(rb.rotation * turnRotation);
         transform.rotation = rb.rotation;
      }
   }
}
