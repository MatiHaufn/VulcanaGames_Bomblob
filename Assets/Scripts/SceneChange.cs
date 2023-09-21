using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1); //Loads the Scene No. 1 ( = Starts the game) 
    }
}
