using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade da câmera (unidades por segundo)

    void Update()
    {
        // Move a câmera para a direita com base no tempo e na velocidade definida
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }
}
