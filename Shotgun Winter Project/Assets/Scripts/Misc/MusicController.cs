using System;
using Project.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }

    #region Audio
    public AudioSource _audioSource;
    [Header("Audio")]
    [Space(5)]
    [SerializeField] AudioClip _menuMusic;
    [Range(0,2)]
    [SerializeField] private float _menuMusicVolume = 1;
    [Space(10)]
    [SerializeField] AudioClip _levelMusic;
    [Range(0,2)]
    [SerializeField] private float _levelMusicVolume = 1;
    [Space(10)]
    [SerializeField] AudioClip _endMusic;
    [Range(0,2)]
    [SerializeField] private float _endMusicVolume = 1;
    [Space(10)]
    [SerializeField] AudioClip _victoryMusic;
    [Range(0,2)]
    [SerializeField] private float _victoryMusicVolume = 1;
    [Space(10)]
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

    public void VictoryJingle()
    {
        print("Jingling");
        _audioSource.loop = false;
        _audioSource.volume = _victoryMusicVolume;
        _audioSource.clip = _victoryMusic;
        _audioSource.Play();
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
            _audioSource.loop = true;
            _audioSource.volume = _menuMusicVolume;
            _audioSource.clip = _menuMusic;
            _audioSource.Play();
        }
        else if (scene.name == _endScene.SceneName)
        {
            _audioSource.loop = true;
            _audioSource.volume = _endMusicVolume;
            _audioSource.clip = _endMusic;
            _audioSource.Play();
        }
        else
        {
            if (_audioSource.clip != _levelMusic)
            {
                _audioSource.loop = true;
                _audioSource.volume = _levelMusicVolume;
                _audioSource.clip = _levelMusic;
                _audioSource.Play();
            }
        }
    }
}