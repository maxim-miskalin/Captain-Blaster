using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private ShipControl _ship;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _gameOver;

    private int _score = 0;

    private void OnEnable()
    {
        _ship.Died += DiePlayer;
        _ship.DestroedMeteor += AddScore;
    }

    private void OnDisable()
    {
        _ship.Died -= DiePlayer;
        _ship.DestroedMeteor -= AddScore;
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        _score = 0;
        _scoreText.text = _score.ToString();
        _gameOver.gameObject.SetActive(false);
    }

    public void AddScore()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }

    public void DiePlayer()
    {
        _gameOver.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}
