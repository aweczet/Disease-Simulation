using System;
using UnityEngine;

public class SubjectController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection;
    private float _speed;
    private void Start()
    {
        _rigidbody = transform.GetComponent<Rigidbody2D>();
        _speed = 10f;
    }

    private void Update()
    {
        _rigidbody.velocity = transform.up * _speed;
    }
}
