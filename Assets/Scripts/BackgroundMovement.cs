using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade do background (unidades por segundo)

    void Update()
    {
        // Move o background para a esquerda com base no tempo e na velocidade definida
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }
}
