using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{       
    private InputActions _inputActions;
    private Rigidbody2D _rigidbody2D;
    private Camera _mainCam;
    private Vector3 _mousePos;
    [SerializeField] private float blastForceModifier = 10f;
    [SerializeField] MagUIController _magUI;
    
    [Header("GUN")]
    [SerializeField] private int _magSize = 8;

    public int _ammo { get; private set; } = 0;

    private void Start()
    {
       _inputActions = GetComponent<InputActions>();
       _rigidbody2D = GetComponent<Rigidbody2D>();
       _mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
       _ammo = _magSize; 
       _magUI.UpdateMagUI(_ammo);
    }

    private void Update()
    {
        _mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        if (_inputActions.FirePressed && _ammo != 0)
        {
            print("Shoot!");
            _rigidbody2D.AddForce(_mousePos * (blastForceModifier * -1), ForceMode2D.Impulse);
            _ammo--;
            _magUI.UpdateMagUI(_ammo);
        } 
    }
}
