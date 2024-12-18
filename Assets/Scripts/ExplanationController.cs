using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Importa o namespace para gerenciar as cenas

public class ExplanationController : MonoBehaviour
{
    // Nome da cena que você deseja carregar ao pressionar a tecla espaço
    public string nextSceneName = "NomeDaCenaDesejada";

    // Update is called once per frame
    void Update()
    {
        // Verifica se a tecla Espaço foi pressionada
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Carrega a próxima cena
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
