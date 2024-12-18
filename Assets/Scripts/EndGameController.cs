using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{
    // Start is called before the first frame update
    public void MenuGame() 
    {
        SceneManager.LoadScene("Home");
    }
}
