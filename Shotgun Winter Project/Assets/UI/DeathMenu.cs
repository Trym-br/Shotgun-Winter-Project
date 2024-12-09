using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class deathmenu : MonoBehaviour
{ 
    [SerializeField] private UIDocument uiDocument;

    private void Awake()
    {
        var root = uiDocument.rootVisualElement;
        
        var retryButton = root.Q<Button>("RetryButton");
        
        retryButton.clicked += StartGame;
    }
    

    private void StartGame()
    {
        Debug.Log("Retry button pressed");
        SceneManager.LoadScene("mainMenu");
        
    }
}