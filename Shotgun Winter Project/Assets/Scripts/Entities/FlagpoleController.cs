using System;
using System.Collections;
using UnityEditor;
using Project.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagpoleController : MonoBehaviour
{
    public SceneField scene;
    [SerializeField] private Vector3 snapOffset = new Vector3(0,0,0);
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && scene != null)
        {
            other.gameObject.GetComponent<PlayerController>().Victory(scene.SceneName);
            // other.gameObject.transform.position = transform.position + new Vector3(0, 0, 0);
            StartCoroutine (ShmovePlayer(other.gameObject, transform.position + snapOffset, 1f));
            // SceneManager.LoadScene(scene);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && scene != null)
        {
            other.gameObject.GetComponent<PlayerController>().Victory(scene.SceneName);
            // other.gameObject.transform.position = transform.position + new Vector3(0, 0, 0);
            StartCoroutine (ShmovePlayer(other.gameObject, transform.position + snapOffset, 1f));
            // SceneManager.LoadScene(scene);
        }
    }

    IEnumerator ShmovePlayer(GameObject player, Vector3 targetPos, float time)
    {
        print("Shmooving to: " + targetPos.x);
        player.GetComponent<Rigidbody2D>().linearVelocityX = 0;
        while (player.transform.position != targetPos)
        {
            // player.transform.position = Vector3.MoveTowards(player.transform.position, targetPos, time * Time.deltaTime);
            player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(targetPos.x, player.transform.position.y, player.transform.position.z), time * Time.deltaTime);
            yield return new WaitForEndOfFrame ();
        } 
    }
}