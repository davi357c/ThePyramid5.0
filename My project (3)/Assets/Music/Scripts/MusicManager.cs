using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public AudioClip externoMusic;
    public AudioClip internoMusic;
    public AudioClip alertMusic;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "CenarioExterno")
            PlayExternoMusic();
        else if (scene.name == "SampleScene")
            PlayInternoMusic();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayExternoMusic() => PlayMusic(externoMusic);
    public void PlayInternoMusic() => PlayMusic(internoMusic);
    public void PlayAlertMusic() => PlayMusic(alertMusic);
}
