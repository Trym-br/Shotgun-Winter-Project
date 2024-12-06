using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{       
    private InputActions _inputActions;
    private Rigidbody2D _rigidbody2D;
    private Camera _mainCam;
    private Vector3 _mousePos;
    
    private bool _isGrounded;

    #region GUN
    [Header("GUN")]
    [SerializeField] private GameObject _cartrigdePrefab;
    [SerializeField] MagUIController _magUI;
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
    #endregion
    
    #region Animation
    private Animator Upper_Body_Animator; 
    private Animator Lower_Body_Animator; 
    private Animator Shotgun_Animator; 
    #endregion


    private void Start()
    {
       _inputActions = GetComponent<InputActions>();
       _rigidbody2D = GetComponent<Rigidbody2D>();
       _mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
       _ammo = _magSize; 
       _magUI.UpdateMagUI(_ammo);
       SceneManager.sceneLoaded += OnSceneLoaded;
       
       Upper_Body_Animator = transform.GetChild(0).GetComponent<Animator>();
       Lower_Body_Animator = transform.GetChild(1).GetComponent<Animator>();
       Shotgun_Animator = transform.GetChild(2).GetComponent<Animator>();
       _shotgunTransform = transform.GetChild(2).transform;
    }

    private void Update()
    {
        // Shooting
        if (_inputActions.FirePressed && _ammo != 0 && _fireTimer < 0)
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
        
        //Ejects cartridge
        GameObject _ejectedCartridge = Instantiate(_cartrigdePrefab, _shotgunTransform.position, _shotgunTransform.rotation);
        // print("Shotgun forward: " + _shotgunTransform.up);
        // print("U/R|F: " + _shotgunTransform.up + " / " + _shotgunTransform.right + " / " + Vector3.Cross(_shotgunTransform.up, _shotgunTransform.right).normalized);
        // Vector3 dir = (_shotgunTransform.up + _shotgunTransform.right*-4)/2f; 
        Vector3 dir = (_shotgunTransform.up + _shotgunTransform.right*-1)/2f; 
        _ejectedCartridge.GetComponent<Rigidbody2D>().linearVelocity = dir * 8f;

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true; 
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
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }
}
