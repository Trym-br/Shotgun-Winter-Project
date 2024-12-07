using UnityEngine;

public class Shotgun : MonoBehaviour
{
    private Camera _mainCam;
    private Vector3 _mousePos;

    [SerializeField] private GameObject _cartrigdePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate / follow mouse
        _mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = _mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    public void EjectCartridge()
    {
        GameObject _ejectedCartridge = Instantiate(_cartrigdePrefab, transform.position + new Vector3(0.16f, 0, 0), transform.rotation);
        // Vector3 dir = (transform.up + transform.right * -1) / 2f; 
        // print("Shotgun Angle: " + (transform.rotation.eulerAngles.z - 90));
        // Vector3 dir = new Vector3(-0.5f * transform.right.x, 0.5f * transform.up.y, 0f);
        Vector3 dir = new Vector3(-0.5f * transform.right.x, 0.5f, 0f);
        // Vector3 dir = new Vector3(0f, 0.5f, 0f);
        _ejectedCartridge.GetComponent<Rigidbody2D>().linearVelocity = dir * 8f;
        // print("Shotgun forward: " + _shotgunTransform.up);
        // print("U/R|F: " + _shotgunTransform.up + " / " + _shotgunTransform.right + " / " + Vector3.Cross(_shotgunTransform.up, _shotgunTransform.right).normalized);
        // Vector3 dir = (_shotgunTransform.up + _shotgunTransform.right*-4)/2f; 
        // GameObject _ejectedCartridge = Instantiate(_cartrigdePrefab, _shotgunTransform.position, _shotgunTransform.rotation);
        // Vector3 dir = (_shotgunTransform.up + _shotgunTransform.right * -1) / 2f; 
        // _ejectedCartridge.GetComponent<Rigidbody2D>().linearVelocity = dir * 8f;
    }
}
