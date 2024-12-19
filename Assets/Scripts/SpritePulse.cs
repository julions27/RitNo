using UnityEngine;

public class SpritePulse : MonoBehaviour
{
    public float pulseSpeed = 2f; // Velocidade da pulsação (em ciclos por segundo)
    public float pulseIntensity = 0.1f; // Intensidade da pulsação (variação de escala)

    private Vector3 originalScale; // Escala original do sprite

    void Start()
    {
        // Armazena a escala inicial do sprite
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Calcula a nova escala usando uma função seno para criar o efeito de pulsação
        float scaleFactor = 1.0f + Mathf.PingPong(Time.time * pulseSpeed, 0.5f);
        transform.localScale = originalScale + new Vector3(scaleFactor, scaleFactor, 0);
    }
}
