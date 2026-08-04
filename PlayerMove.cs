using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Vector3 CurrentInput { get; private set; }
    public float MoveSpeed = 5f;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + CurrentInput * MoveSpeed * Time.fixedDeltaTime);
    }

    public void SetMovementInput(Vector3 input)
    {
        CurrentInput = Vector3.ClampMagnitude(input, 1f);
        
    }
}
