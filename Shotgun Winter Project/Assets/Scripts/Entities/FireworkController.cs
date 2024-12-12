using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireworkController : MonoBehaviour
{
    [SerializeField] private int _color = 1;
    [SerializeField] private float _speed;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private void Start()
    {
       _rigidbody2D = GetComponent<Rigidbody2D>(); 
       _animator = GetComponent<Animator>();
       Launch();
    }

    public void Launch()
    {
       _animator.SetInteger("Color", _color); 
      _rigidbody2D.rotation = Random.Range(0f, 360f);
      _rigidbody2D.linearVelocity = new Vector2(0, _speed);
    }
}
