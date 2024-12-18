using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
    public void NextLevel()
    {
        SceneManager.LoadScene("EndGame");
    }
}
