using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip lobbyAndMainMenuClip; // Lobbyscene과 Mainmenuscene에서 재생할 클립
    public AudioClip mainSceneClip; // Mainscene에서 재생할 클립

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않도록 설정
            audioSource = GetComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 중복 인스턴스 방지
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "LobbyScene":
            case "MainMenuScene":
                if (audioSource.clip != lobbyAndMainMenuClip)
                {
                    audioSource.clip = lobbyAndMainMenuClip;
                    audioSource.Play();
                }
                break;
            case "MainScene":
                Destroy(gameObject);
                break;
        }
    }
}
