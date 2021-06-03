using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SubjectController : MonoBehaviour
{
    private Vector2 _maxBoardSize;
    private EventHandler<TimeTickSystem.OnTickEvents> _tickSystemDelegate;
    private Rigidbody2D _rigidbody;

    private Vector2 _moveDirection;
    private float _speed;

    public StatusType status;
    private int _age;
    private float _immunity;

    private void Start()
    {
        _maxBoardSize = new Vector2(8f, 4f);
        _rigidbody = transform.GetComponent<Rigidbody2D>();
        _tickSystemDelegate = delegate
        {
            Age();
            Die();
        };
        TimeTickSystem.OnTick += _tickSystemDelegate;
        Init();
    }

    private void Update()
    {
        _rigidbody.velocity = transform.up * _speed;
    }

    private void Init()
    {
        // Set random position, rotation, speed, status, age and immunity value
        transform.position = new Vector2(Random.Range(-_maxBoardSize.x, _maxBoardSize.x),
            Random.Range(-_maxBoardSize.y, _maxBoardSize.y));
        transform.Rotate(Vector3.forward * Random.Range(0f, 360f), Space.Self);
        _speed = Random.Range(5f, 10f);
        status = (StatusType) Random.Range(0, Enum.GetValues(typeof(StatusType)).Length);
        _age = Random.Range(0, 61);
        _immunity = Random.Range(0f, 10f);
        _immunity = CheckImmunity();
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

        if (_immunity != retImmunity)
        {
            Debug.Log("Immunity changed at age: " + _age);
        }

        return retImmunity;
    }


    private void Age()
    {
        _age += 1;
        _immunity = CheckImmunity();
    }

    private void Die()
    {
        if (_age > 100 || _immunity <= 0)
        {
            TimeTickSystem.OnTick -= _tickSystemDelegate;
            Destroy(gameObject);
        }
    }

    private void ChangeDirection()
    {
        transform.Rotate(Vector3.forward * Random.Range(0f, 360f), Space.Self);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        ChangeDirection();
    }
}