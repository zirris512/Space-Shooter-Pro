using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver;

    private void Update()
    {
        if (Keyboard.current.rKey.isPressed && _isGameOver)
        {
            //Current Game Scene
            SceneManager.LoadScene(1);
        }

        if (Keyboard.current.escapeKey.isPressed)
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
