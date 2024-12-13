using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{       
    private InputActions _inputActions;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _feetCollider;
    private Animator _animator;
    private Camera _mainCam;
    private Vector3 _mousePos;
    private Vector3 _worldCenter;
    private AudioSource _audioSource;
    private bool Alive;
    
    private bool _isGrounded;

    #region GUN
    [Header("GUN")]
    [SerializeField] private GameObject _cartrigdePrefab;
    // [SerializeField] MagUIController _magUI;
    [SerializeField] MagUIScalableController _magUI;
    [SerializeField] private int _magSize = 8;
    // way to make it accessible as read only, but still formatted correctly in inspector (very hacky solution)
    public int MagSize {
        private set => _magSize = value;
        get => _magSize;
    }
    [SerializeField] private float _blastForceModifier = 10f;
    [SerializeField] private float _fireRate = 0.2f;
    [SerializeField] private bool _momentum;
    private float _fireTimer = 0f;
    public int _ammo { get; private set; } = 0;
    private Transform _shotgunTransform;
    private ShotgunController _shotgunController;
    #endregion
    
    #region Animation
    private Animator Upper_Body_Animator; 
    private Animator Lower_Body_Animator; 
    private Animator Shotgun_Animator;
    [Header("Animation")]
    [SerializeField] private Sprite _neutralSprite;
    [SerializeField] private Sprite _deadSprite;
    [SerializeField] private float _deathDelay = 2;
    [SerializeField] private GameObject _fireworkPrefab;
    #endregion

    #region Audio
    [Header("Audio")]
    [SerializeField] private AudioClip _audioBunnyLand;
    [SerializeField] private AudioClip _audioFireFirework;
    #endregion

    private void Start()
    {
        Alive = true;
       _inputActions = GetComponent<InputActions>();
       _rigidbody2D = GetComponent<Rigidbody2D>();
       _spriteRenderer = GetComponent<SpriteRenderer>();
       _feetCollider = GetComponent<BoxCollider2D>();
       _audioSource = GetComponent<AudioSource>();
       _animator = GetComponent<Animator>();
       _mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
       _ammo = _magSize; 
       _magUI.InitUI(_magSize);
       SceneManager.sceneLoaded += OnSceneLoaded;
       
       Upper_Body_Animator = transform.GetChild(0).GetComponent<Animator>();
       // Upper_Body_Renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
       Lower_Body_Animator = transform.GetChild(1).GetComponent<Animator>();
       // Lower_Body_Renderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
       Shotgun_Animator = transform.GetChild(2).GetComponent<Animator>();
       Shotgun_Animator.SetFloat("AnimationSpeedModifier", 1/_fireRate);
       _shotgunTransform = transform.GetChild(2).transform;
       _shotgunController = transform.GetChild(2).GetComponent<ShotgunController>();
       
       // Get the main camera
       Camera mainCamera = Camera.main;
       // Find the center of the screen in screen-space coordinates
       Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, mainCamera.nearClipPlane);
       // Convert screen-space to world-space
       _worldCenter = mainCamera.ScreenToWorldPoint(screenCenter); 
       print("Worldcenter: " + _worldCenter);
    }

    private void Update()
    {
        // Shooting
        if (_inputActions.FirePressed && _ammo != 0 && _fireTimer < 0 && Alive)
        {
            Shoot();
            _fireTimer = _fireRate;
        }
        _fireTimer -= Time.deltaTime;

        // Scene Reload
        if (_inputActions.ResetPressed) { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    }

    void Shoot()
    {
        // Calculates knockbackDirection
        _mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 NormalizedMousePos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        if (NormalizedMousePos.x < 0.005)
        {
            _mousePos.x -= 0.5f;
        }
        Vector2 knockbackDir = new Vector2(_mousePos.x - transform.position.x,
                                           _mousePos.y - transform.position.y).normalized;
        // Starts Animations
        Shotgun_Animator.SetTrigger("Shoot");
        Upper_Body_Animator.SetTrigger("Flinch");
        print("Shoot!");
        
        // Resets momentum, makes it less cheesed but also very easy to maneuver, so maybe don't?
        if(!_momentum){_rigidbody2D.linearVelocity = Vector2.zero;}
        _rigidbody2D.AddForce(knockbackDir * (_blastForceModifier * -1), ForceMode2D.Impulse);
            
        // Adjusts ammo / Mag
        _ammo--;
        _magUI.UpdateMagUI(_ammo);
        
        //Ejection of cartridge happens in shotgun controller and animator
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // print("Collision: " + other.gameObject.name + " / " + other.GetContact(0).otherCollider);
        if (other.GetContact(0).otherCollider == _feetCollider && other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true; 
            _audioSource.pitch = Random.Range(1.5f, 1.6f);
            _audioSource.PlayOneShot(_audioBunnyLand, 2f);
            Lower_Body_Animator.SetBool("Grounded", true);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
           _isGrounded = false; 
            Lower_Body_Animator.SetBool("Grounded", false);
        }
    }

    public void Die()
    {
        Alive = false;
        StartCoroutine(DeathAnimation());
    }
    IEnumerator DeathAnimation()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        _spriteRenderer.sprite = _deadSprite;
        _spriteRenderer.enabled = true;
        yield return new WaitForSeconds(_deathDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Victory(String scene)
    {
        Alive = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        _spriteRenderer.sprite = _neutralSprite;
        _spriteRenderer.enabled = true;
        _spriteRenderer.flipX = true;
        StartCoroutine(VictoryAnimation(scene));
        MusicController.Instance._audioSource.Stop();
    }

    IEnumerator VictoryAnimation(String scene)
    { 
        yield return new WaitUntil(() => _isGrounded == true);
        yield return new WaitForSeconds(0.2f); 
        _animator.SetTrigger("Victory");
        yield return new WaitForSeconds(4.3f); 
        SceneManager.LoadScene(scene);
    }

    IEnumerator VictoryFirework()
    {
        MusicController.Instance.VictoryJingle();
        _audioSource.clip = _audioFireFirework;
        _audioSource.Play();
        yield return new WaitForSeconds(0);
        // Firework
        print("Worldcenter in use: " + _worldCenter);
        Vector3[] Arr = {new Vector3(_worldCenter.x -3, _worldCenter.y + 2, 0), new Vector3(_worldCenter.x, _worldCenter.y + 3, 0), new Vector3(_worldCenter.x + 3, _worldCenter.y + 2, 0) };
        for (int i = 0; i < Arr.Length; i++)
        {
            GameObject fireworkRef = Instantiate(_fireworkPrefab, Arr[i], Quaternion.identity);
            FireworkController fireworkControllerRef = fireworkRef.GetComponent<FireworkController>();
            fireworkControllerRef._speed = 0;
            fireworkControllerRef._delay = 1.5f;
        }
        // Vector3 pos = new Vector3(0,2,0);
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }
}