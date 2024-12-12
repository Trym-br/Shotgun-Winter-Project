using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireworkController : MonoBehaviour
{
    // [SerializeField] private int _color = 1;
    // [SerializeField] private float _speed;
    // [SerializeField] private float _delay;
    // [SerializeField] private float _animationSpeed = 0.2f;
    // [SerializeField] private float _fadeTime = 2;
    public int _color = 1;
    public float _speed = 4;
    public float _delay = 1;
    public float _animationSpeed = 0.2f;
    public float _fadeTime = 2;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
       _rigidbody2D = GetComponent<Rigidbody2D>(); 
       _animator = GetComponent<Animator>();
       _spriteRenderer = GetComponent<SpriteRenderer>();
       Launch();
    }

    public void Launch()
    {
        //90 - 270
        // transform.rotation = Quaternion.Euler(0, 0, Random.Range(-15f, 15f));
        Vector3 dir = transform.up;
        _rigidbody2D.linearVelocity = dir * _speed;
        StartCoroutine(ExplosionAnimation());
        // print(this.name + ": rot: " + transform.rotation.eulerAngles);
        // print(this.name + ": dir: " + dir);
    }

    IEnumerator ExplosionAnimation()
    {
        yield return new WaitForSeconds(_delay);
        // _rigidbody2D.linearVelocity = Vector2.zero;
        _rigidbody2D.linearVelocity = new Vector2(0, -0.1f);
        
        _animator.speed = _animationSpeed;
        _animator.SetInteger("Color", _color);
        StartCoroutine(Fade(1f, 0f, _fadeTime));
    }
    IEnumerator Fade(float start, float end, float duration) {
        for (float t=0f;t<duration;t+=Time.deltaTime) {
            float normalizedTime = t/duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            float a = Mathf.Lerp(start, end, normalizedTime);
            _spriteRenderer.color = new Color(1, 1, 1, a);
            yield return null;
        }
        _spriteRenderer.color = new Color(1, 1, 1, end);
        // print("DEATH");
        Destroy(this.gameObject);
    }
}
