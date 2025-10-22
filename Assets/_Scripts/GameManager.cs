using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    private static GameManager instance;
    public static bool sceneChanged = false;


    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else if (instance != this) Destroy(gameObject);
    }

    public static void changeScene(int sceneBuilderIndex) {
        SceneManager.LoadScene(sceneBuilderIndex);
        sceneChanged = true;
    }

    public static void changeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
        sceneChanged = true;
    }

    public static void nextScene() {
        changeScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

}
