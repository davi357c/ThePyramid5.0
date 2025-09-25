using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  
    public void PlayGame()
    {
        SceneManager.LoadScene("CenarioExterno");
    }

    // Botão Quit: fecha o jogo
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#else
            Application.Quit(); // fecha o .exe
#endif
    }
}
