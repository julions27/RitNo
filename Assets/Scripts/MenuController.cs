using UnityEngine;
using UnityEngine.SceneManagement; // Para carregar novas cenas
using UnityEngine.UI; // Para lidar com a UI

public class MenuController : MonoBehaviour
{
    // M�todo para iniciar a cena
    public void StartGame()
    {
        SceneManager.LoadScene("Explanation1"); // Substitua pelo nome da cena
    }

    // M�todo para sair do jogo
    public void QuitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Para parar no editor
#else
        Application.Quit(); // Para sair do jogo no build
#endif
    }
}
