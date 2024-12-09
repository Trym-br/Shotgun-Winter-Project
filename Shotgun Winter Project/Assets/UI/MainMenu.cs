using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{ 
    [SerializeField] private UIDocument uiDocument;

    private void Awake()
    {
        var root = uiDocument.rootVisualElement;
        
        var startButton = root.Q<Button>("StartButton");
        
        startButton.clicked += StartGame;
    }
    

    private void StartGame()
    {
        Debug.Log("Start button pressed");
        SceneManager.LoadScene("Level 1");
        
    }
}
