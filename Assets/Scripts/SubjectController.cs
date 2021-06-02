using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SubjectController : MonoBehaviour
{
    private EventHandler<TimeTickSystem.OnTickEvents> _tickSystemDelegate;
    private Rigidbody2D _rigidbody;

    private Vector2 _moveDirection;
    private float _speed;

    public StatusType status;
    private int _age;
    private float _immunity;

    private void Start()
    {
        _rigidbody = transform.GetComponent<Rigidbody2D>();
        _tickSystemDelegate = delegate
        {
            status = (StatusType) Random.Range(0, Enum.GetValues(typeof(StatusType)).Length);
            // Debug.Log(status);
        };
        TimeTickSystem.OnTick += _tickSystemDelegate;

        _speed = Random.Range(5f, 10f);
        status = (StatusType) Random.Range(0, Enum.GetValues(typeof(StatusType)).Length);
        Debug.Log(status);
    }

    private void Update()
    {
        _rigidbody.velocity = transform.up * _speed;
    }

    private float CheckImmunity()
    {
        float retImmunity = 0;

        if ((_age >= 0 && _age < 15) || (_age >= 70 && _age <= 100))
            retImmunity = _immunity > 3 ? Random.Range(0f, 3f) : _immunity;
        else if (_age >= 15 && _age < 40)
            retImmunity = _immunity > 6 ? Random.Range(3f, 6f) : _immunity;
        else if (_age >= 40 && _age < 70)
            retImmunity = _immunity;

        return retImmunity;
    }


    private void ChangeDirection()
    {
        transform.Rotate(Vector3.forward * 180, Space.Self);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ChangeDirection();
    }
}