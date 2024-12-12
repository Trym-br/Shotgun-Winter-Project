using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireworkSpawnerController : MonoBehaviour
{
    [SerializeField] private float _fireRateMin;
    [SerializeField] private float _fireRateMax;
    private float _fireTimer;
    [SerializeField] private GameObject _fireworkPrefab;

    private void Update()
    {
        _fireTimer -= Time.deltaTime;
        if (_fireTimer <= 0)
        {
            SpawnFirework();
            _fireTimer = Random.Range(_fireRateMin, _fireRateMax);
        }
    }

    private void SpawnFirework()
    {
        Vector3 pos = new Vector3(Random.Range(-8, 8), -3, 0);
        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(-15f, 15f));
        GameObject _fireworkRef = Instantiate(_fireworkPrefab, pos, rot);
        _fireworkRef.transform.parent = transform;
        FireworkController _fireworkControllerRef = _fireworkRef.GetComponent<FireworkController>();
        _fireworkControllerRef._color = Random.Range(1, 9);
        _fireworkControllerRef._delay = Random.Range(0.8f, 2f);
    }
}
