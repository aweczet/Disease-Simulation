using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SubjectController : MonoBehaviour
{
    private SubjectGenerator _subjectGenerator;
    private Vector2 _maxBoardSize;
    private EventHandler<TimeTickSystem.OnTickEvents> _tickSystemDelegate;
    private Rigidbody2D _rigidbody;

    private const float BirthProbability = 0.2f;
    private int _dayCounter;

    private Vector2 _moveDirection;
    private float _speed;
    public StatusType status;
    [SerializeField] private int _age;
    [SerializeField] private float _immunity;

    public bool isChild;

    // Function called at start of simulation
    private void Start()
    {
        _maxBoardSize = new Vector2(10f, 10f);
        _rigidbody = transform.GetComponent<Rigidbody2D>();
        _subjectGenerator = FindObjectOfType<SubjectGenerator>();
        _tickSystemDelegate = delegate
        {
            Age();
            Die();
            CheckStatus();
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

    // Check if value of immunity should change - if so - change it
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

    // Get enum of immunity
    private ImmunityType GetImmunityName()
    {
        ImmunityType immunityType;
        if (_immunity < 3)
            immunityType = ImmunityType.Low;
        else if (_immunity < 6)
            immunityType = ImmunityType.Middle;
        else
            immunityType = ImmunityType.High;
        return immunityType;
    }

    // Subjects should age every day
    private void Age()
    {
        _age += 1;
        if (_age >= 15 || !isChild)
            _immunity = CheckImmunity();
    }

    // Subjects should die if too old or too weak
    private void Die()
    {
        if (_age > 100 || _immunity <= 0)
        {
            TimeTickSystem.OnTick -= _tickSystemDelegate;
            Destroy(gameObject);
        }
    }

    // After collision: rotate, spread disease and/or give birth to a child(s)
    private void OnCollisionStay2D(Collision2D other)
    {
        transform.Rotate(Vector3.forward * Random.Range(0f, 360f), Space.Self);
        
        if (!other.transform.CompareTag("Subject")) return;
        SubjectController otherSubject = other.transform.GetComponent<SubjectController>();
        int otherAge = otherSubject._age;
        GiveBirth(otherAge);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Subject")) return;

        SubjectController otherSubject = other.transform.GetComponent<SubjectController>();
        var otherStatus = otherSubject.status;

        Dependency(otherSubject);
    }

    // Dependencies between subjects
    private void Dependency(SubjectController otherSubject)
    {
        StatusType otherStatus = otherSubject.status;
        StatusType newStatus = status;
        switch (status)
        {
            case StatusType.C:
                if (otherStatus == StatusType.Z)
                    _dayCounter = 0;

                if (otherStatus == StatusType.C)
                    _immunity = Mathf.Min(_immunity, otherSubject._immunity);

                break;
            case StatusType.Z:
                if (otherStatus == StatusType.Z || otherStatus == StatusType.ZD)
                    _immunity -= 1;

                if (otherStatus == StatusType.C && GetImmunityName() != ImmunityType.High)
                    newStatus = StatusType.C;

                break;
            case StatusType.ZD:
                if (otherStatus == StatusType.ZZ)
                    _immunity = _immunity - 1 <= 10 ? _immunity + 1 : _immunity;

                if (otherStatus == StatusType.C && GetImmunityName() != ImmunityType.High)
                    newStatus = StatusType.C;

                break;
            case StatusType.ZZ:
                if (otherStatus == StatusType.Z && GetImmunityName() == ImmunityType.Low)
                    newStatus = StatusType.Z;

                if (otherStatus == StatusType.C)
                    if (GetImmunityName() != ImmunityType.High)
                        newStatus = StatusType.Z;
                    else
                        _immunity -= 3;

                if (otherStatus == StatusType.ZZ)
                    _immunity = Mathf.Max(_immunity, otherSubject._immunity);

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (status == newStatus) return;
        status = newStatus;
        ChangeColor();
        _dayCounter = 0;
    }

    // Colliding subjects can give birth to a child(s)
    private void GiveBirth(int otherAge)
    {
        if (Random.value < BirthProbability && otherAge >= 20 && otherAge <= 40 && _age >= 20 && _age <= 40)
        {
            if (Random.value < .1f)
                _subjectGenerator.GenerateChild();
            _subjectGenerator.GenerateChild();
        }
    }

    // If object is child it should get not random values as init
    private void SetChildStats()
    {
        status = StatusType.ZZ;
        _age = 0;
        _immunity = 10;
    }

    // Each status have own color
    private void ChangeColor()
    {
        Color newColor;
        switch (status)
        {
            case StatusType.C:
                newColor = Color.red;
                break;
            case StatusType.Z:
                newColor = Color.yellow;
                break;
            case StatusType.ZD:
                newColor = new Color(1, .65f, 0);
                break;
            case StatusType.ZZ:
                newColor = Color.green;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        transform.GetComponent<Renderer>().material.color = newColor;
    }

    // Statuses transiting to another (also doing damage/healing subject) 
    private void CheckStatus()
    {
        var newStatus = status;
        switch (status)
        {
            case StatusType.Z:
                _immunity -= .1f;
                if (_dayCounter >= 2)
                    newStatus = StatusType.C;
                break;
            case StatusType.C:
                _immunity -= .5f;
                if (_dayCounter >= 7)
                    newStatus = StatusType.ZD;
                break;
            case StatusType.ZD:
                _immunity = _immunity <= 10 - .1f ? _immunity + .1f : _immunity;
                if (_dayCounter >= 5)
                    newStatus = StatusType.ZZ;
                break;
            case StatusType.ZZ:
                _immunity = _immunity <= 10 - .05f ? _immunity + .05f : _immunity;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (status != newStatus)
        {
            status = newStatus;
            ChangeColor();
            _dayCounter = 0;
        }

        _dayCounter++;
    }
}