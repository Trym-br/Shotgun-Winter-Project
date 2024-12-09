using System;
using Project.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VardeController : MonoBehaviour
{
    public SceneField scene;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(scene);
        }
    }
}