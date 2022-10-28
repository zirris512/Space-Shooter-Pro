using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Get text component
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _livesSprites;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
    }
    public void updateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void updateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprites[currentLives];
    }

    public void displayGameOver()
    {
        _gameOverText.gameObject.SetActive(true);
    }
}
