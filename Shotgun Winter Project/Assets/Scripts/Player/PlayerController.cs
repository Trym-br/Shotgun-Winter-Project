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

    [Header("GUN")] 
    [SerializeField] MagUIController _magUI;
    [SerializeField] private int _magSize = 8;
    [SerializeField] private float _blastForceModifier = 10f;
    [SerializeField] private float _fireRate = 0.2f;
    [SerializeField] private bool _momentum;
    private float _fireTimer = 0f;

    public int _ammo { get; private set; } = 0;

    private void Start()
    {
       _inputActions = GetComponent<InputActions>();
       _rigidbody2D = GetComponent<Rigidbody2D>();
       _mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
       _ammo = _magSize; 
       _magUI.UpdateMagUI(_ammo);
       SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        _mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        // Vector2 knockbackDir = new Vector2(_mousePos.y - transform.position.y, _mousePos.x - transform.position.x).normalized;
        Vector2 knockbackDir = new Vector2(_mousePos.x - transform.position.x,
                                           _mousePos.y - transform.position.y).normalized;
        // print("Dir: " + knockbackDir + "\nM/T pos:\t" + _mousePos + "\t" + transform.position);
        if (_inputActions.FirePressed && _ammo != 0 && _fireTimer < 0)
        {
            print("Shoot!");
            // Resets momentum, makes it less cheesed but also very easy to maneuver, so maybe don't?
            if(!_momentum){_rigidbody2D.linearVelocity = Vector2.zero;}
            _rigidbody2D.AddForce(knockbackDir * (_blastForceModifier * -1), ForceMode2D.Impulse);
            
            _ammo--;
            _magUI.UpdateMagUI(_ammo);
            
            _fireTimer = _fireRate;
        }
        _fireTimer -= Time.deltaTime;

        // Scene Reload
        if (_inputActions.ResetPressed) { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }
}
