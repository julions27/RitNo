using UnityEngine;
using TMPro; // Certifique-se de usar TextMeshPro

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        // Recupera o score salvo
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);

        // Atualiza o texto da tela
        if (scoreText != null)
        {
            scoreText.text = $"{finalScore}";
        }
    }
}
