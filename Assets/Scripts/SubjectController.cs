using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SubjectController : MonoBehaviour
{
    private SubjectGenerator _subjectGenerator;
    private Vector2 _maxBoardSize;
    private EventHandler<TimeTickSystem.OnTickEvents> _tickSystemDelegate;
    private Rigidbody2D _rigidbody;

    private const float BirthProbability = 0.015f;

    private Vector2 _moveDirection;
    private float _speed;
    public StatusType status;
    [SerializeField] private int _age;
    private float _immunity;

    public bool isChild;

    private void Start()
    {
        _maxBoardSize = new Vector2(7.9f, 3.9f);
        _rigidbody = transform.GetComponent<Rigidbody2D>();
        _subjectGenerator = FindObjectOfType<SubjectGenerator>();
        _tickSystemDelegate = delegate
        {
            Age();
            Die();
        };
        TimeTickSystem.OnTick += _tickSystemDelegate;
        Init();
        if (isChild)
            SetChildStats();
        ChangeColor();
    }

    private void Update()
    {
        _rigidbody.velocity = transform.up * _speed;
    }

    // Set random position, rotation, speed, status, age and immunity value
    private void Init()
    {
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
        if (other.transform.CompareTag("Subject"))
        {
            int otherAge = other.transform.GetComponent<SubjectController>()._age;
            if (Random.value < BirthProbability && otherAge >= 20 && otherAge <= 40 && _age >= 20 && _age <= 40)
            {
                if (Random.value < .1f)
                    _subjectGenerator.GenerateChild();
                _subjectGenerator.GenerateChild();
            }
        }

        ChangeDirection();
    }

    private void SetChildStats()
    {
        status = StatusType.ZZ;
        _age = 0;
        _immunity = 10;
    }

    private void ChangeColor()
    {
        Color newColor;
        switch (status)
        {
            // Chory
            case StatusType.C:
                newColor = Color.red;
                break;
            // Zarażony
            case StatusType.Z:
                newColor = Color.yellow;
                break;
            // Zdrowiejący
            case StatusType.ZD:
                newColor = new Color(1, .65f, 0);
                break;
            // Zdrowy
            case StatusType.ZZ:
                newColor = Color.green;
                break;
            default:
                newColor = Color.white;
                break;
        }

        transform.GetComponent<Renderer>().material.color = newColor;
    }
}