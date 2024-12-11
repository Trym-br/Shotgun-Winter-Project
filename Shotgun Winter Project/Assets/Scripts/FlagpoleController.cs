using System;
using Project.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagpoleController : MonoBehaviour
{
    public SceneField scene;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(scene);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(scene);
        }
    }
}