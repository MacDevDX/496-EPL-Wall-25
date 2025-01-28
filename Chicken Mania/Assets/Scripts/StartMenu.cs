using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void onStartButton ()
    {
        SceneManager.LoadScene("Game Scene"); //1 is next build index so next screen over
    }

    public void onQuitButton ()
    {
        Application.Quit();
    }

}
