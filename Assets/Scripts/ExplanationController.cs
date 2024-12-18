using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Importa o namespace para gerenciar as cenas

public class ExplanationController : MonoBehaviour
{
    // Nome da cena que voc� deseja carregar ao pressionar a tecla espa�o
    public string nextSceneName = "NomeDaCenaDesejada";

    // Update is called once per frame
    void Update()
    {
        // Verifica se a tecla Espa�o foi pressionada
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Carrega a pr�xima cena
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
