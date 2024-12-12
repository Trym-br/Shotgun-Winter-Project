using System;
using Project.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }

    #region Audio
    [Header("Audio")]
    private AudioSource _audioSource;
    [SerializeField] AudioClip _menuMusic;
    [SerializeField] AudioClip _levelMusic;
    [SerializeField] AudioClip _endMusic;

    [SerializeField] private SceneField _menuScene;
    [SerializeField] private SceneField _endScene;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // switch (scene.name)
        // {
        //    case _menuScene.SceneName:
        //        _audioSource.Play();
        //        break;
        // }
        if (scene.name == _menuScene.SceneName)
        {
            _audioSource.clip = _menuMusic;
            _audioSource.Play();
        }
        else if (scene.name == _endScene.SceneName)
        {
            _audioSource.clip = _endMusic;
            _audioSource.Play();
        }
        else
        {
            if (_audioSource.clip != _levelMusic)
            {
                _audioSource.clip = _levelMusic;
                _audioSource.Play();
            }
        }
    }
    
}