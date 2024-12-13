using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillboxController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Destroy(other.gameObject);
            other.gameObject.GetComponent<PlayerController>().Die();
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Destroy(other.gameObject);
            other.gameObject.GetComponent<PlayerController>().Die();
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
