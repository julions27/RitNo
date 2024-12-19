using System.Collections.Generic;
using TMPro; // Não se esqueça de importar o namespace do TextMeshPro
using UnityEngine;



public class RhythmGame : MonoBehaviour
{
    public AudioSource music; // Música principal
    public AudioSource cueAudio; // Áudio curto de deixa
    public AudioSource hitAudio; // Som de acerto
    public AudioSource missAudio; // Som de erro
    public float tolerance = 0.2f; // Tolerância para acertar a batida
    public float moveDistance = 1f; // Distância de movimento por intervalo
    public float moveInterval = 0.5f; // Intervalo entre os movimentos automáticos (segundos)
    public int moveFrequency = 1; // Frequência de movimentos automáticos (ex.: 1 para cada intervalo, 2 para a cada 2 intervalos)

    public Animator characterAnimator; // Animator do personagem
    public TextAsset beatFile; // Arquivo de batidas
    private List<float> b3Times = new List<float>(); // Lista de tempos para B3
    private int currentB3Index = 0; // Índice do próximo tempo B3 a processar
    private bool cuePlayed = false; // Flag para evitar múltiplos toques de deixa

    private float lastMoveTime = 0f; // Momento do último movimento automático
    private int moveCounter = 0; // Contador para determinar quando mover o personagem

    public List<Animator> canaAnimators; // Lista de Animators das canas
    private List<int> b3CanaIndices = new List<int>(); // Índices das canas associadas a cada tempo B3

    public int score = 0; // Pontuação inicial
    public TextMeshProUGUI scoreText; // Agora é do tipo TextMeshProUGUI


    void Start()
    {
        // Processa o arquivo de batidas
        ProcessBeatFile();
        Debug.Log($"B3 Times carregados: {b3Times.Count}");

        // Inicia a música
        music.Play();

        // Inicializa a pontuação na interface
        UpdateScoreDisplay();
    }


    void Update()
    {
        float currentTime = music.time; // Tempo atual da música

        // Movimento automático do personagem
        if (currentTime >= lastMoveTime + moveInterval)
        {
            moveCounter++;
            if (moveCounter % moveFrequency == 0)
            {
                MoveCharacter();
            }
            lastMoveTime = currentTime;
        }

        // Atualiza o valor de cuter com base no input do jogador
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Trigger Cut ativado.");
            characterAnimator.SetTrigger("Cut"); // Dispara a animação
            CheckB3Hit(currentTime); // Verifica acerto ou erro
        }

        // Verifica se estamos no momento do próximo B3
        if (currentB3Index < b3Times.Count)
        {
            float b3Time = b3Times[currentB3Index]; // Próximo tempo B3

            // Reproduz o áudio de aviso 0,5 segundos antes da batida
            if (!cuePlayed && currentTime >= b3Time - 0.5f && currentTime < b3Time - 0.5f + tolerance)
            {
                cueAudio.Play();
                characterAnimator.SetTrigger("Cue"); // Dispara a animação de Cue
                cuePlayed = true; // Marca que o áudio foi tocado
            }

            // Avança automaticamente o índice se passar do B3 sem acerto
            if (currentTime > b3Time + tolerance)
            {
                characterAnimator.SetTrigger("ReturnToIdle");
                OnTickMiss(); // Erro automático se passou do B3 sem apertar
                currentB3Index++;
                cuePlayed = false; // Reseta o sinal
            }
        }
        // Verifica se a música terminou e muda para a próxima cena
        if (!music.isPlaying && music.time >= music.clip.length - 0.1f)
        {
            LoadNextScene();
        }

    }

    private void MoveCharacter()
    {
        transform.position += Vector3.right * moveDistance; // Move o personagem
    }

    private void CheckB3Hit(float currentTime)
    {
        if (currentB3Index < b3Times.Count)
        {
            float b3Time = b3Times[currentB3Index]; // Próximo tempo B3

            float timeDifference = Mathf.Abs(currentTime - b3Time);

            // Se estiver dentro da tolerância exata
            if (timeDifference <= tolerance)
            {
                AddScore(100); // +100 pontos para acerto exato
                OnTickHit(); // Acerto
                currentB3Index++; // Avança para o próximo B3
                cuePlayed = false; // Reseta o sinal
            }
            // Se o acerto for um pouco antes do tempo exato
            else if (timeDifference > tolerance && timeDifference <= tolerance * 1.5f)
            {
                AddScore(50); // +50 pontos para acerto um pouco antes
                OnTickHit(); // Acerto
                currentB3Index++; // Avança para o próximo B3
                cuePlayed = false; // Reseta o sinal
            }
            // Se o acerto for um pouco depois do tempo exato
            else if (timeDifference > tolerance * 1.5f && timeDifference <= tolerance * 2f)
            {
                AddScore(25); // +25 pontos para acerto um pouco depois
                OnTickHit(); // Acerto
                currentB3Index++; // Avança para o próximo B3
                cuePlayed = false; // Reseta o sinal
            }
            else
            {
                AddScore(-50); // -50 pontos para erro
                OnTickMiss(); // Erro
            }
        }
    }


    private void OnTickHit()
    {
        Debug.Log($"Acerto no B3 {currentB3Index}");

        // Toca o som de acerto
        if (hitAudio != null)
        {
            hitAudio.Play();
        }

        // Obtém o índice da cana associada a este tempo B3
        int canaIndex = b3CanaIndices[currentB3Index];
        if (canaIndex >= 0 && canaIndex < canaAnimators.Count)
        {
            // Dispara a animação da cana correta
            canaAnimators[canaIndex].SetTrigger("Cut");
        }
        else
        {
            Debug.LogWarning($"Índice de cana fora do intervalo: {canaIndex}");
        }
    }

    private void OnTickMiss()
    {
        Debug.Log($"Erro no B3 {currentB3Index}");

        // Toca o som de erro
        if (missAudio != null)
        {
            missAudio.Play();
        }
    }

    private void ProcessBeatFile()
    {
        if (beatFile == null)
        {
            Debug.LogError("Arquivo de batidas não definido!");
            return;
        }

        string[] lines = beatFile.text.Split('\n');
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split('\t');
            if (parts.Length >= 4) // Agora esperamos 4 colunas
            {
                try
                {
                    float time = float.Parse(parts[0].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                    string note = parts[2].Trim();
                    int canaIndex = int.Parse(parts[3].Trim()); // Índice da cana no arquivo

                    if (note == "B3")
                    {
                        b3Times.Add(time);
                        b3CanaIndices.Add(canaIndex); // Associa o tempo ao índice da cana
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Erro ao processar linha: {line}\n{ex.Message}");
                }
            }
            else
            {
                Debug.LogWarning($"Linha malformada ignorada: {line}");
            }
        }

        Debug.Log($"Tempos de B3 processados: {b3Times.Count}");
    }

    private void AddScore(int points)
    {
        score += points; // Adiciona os pontos

        // Garante que a pontuação nunca seja menor que zero
        if (score < 0)
        {
            score = 0;
        }

        // Atualiza a pontuação na interface
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Pontuação: {score}"; // Atualiza o texto com a pontuação atual
        }
    }

    private void LoadNextScene()
    {
        // Salva o score no PlayerPrefs
        PlayerPrefs.SetInt("FinalScore", score);

        // Carrega a próxima cena
        UnityEngine.SceneManagement.SceneManager.LoadScene("Score");
    }

}
