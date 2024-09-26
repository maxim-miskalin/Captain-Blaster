using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas _menu;
    [SerializeField] private PlayerUI _playerUI;
    [SerializeField] private MeteorSpawner _meteorSpawner;
    [SerializeField] private ShipControl _shipControl;

    private void Start()
    {
        Time.timeScale = 0f;
    }

    public void ExitMenu()
    {
        Time.timeScale = 0;
        _playerUI.gameObject.SetActive(false);
        _menu.gameObject.SetActive(true);
    }

    public void PlayGame()
    {
        _playerUI.gameObject.SetActive(true);
        _menu.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        _meteorSpawner.ResetPool();
        _shipControl.ResetPool();
        _playerUI.StartGame();
        PlayGame();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}

public interface IPoolObject
{
    public void Return();
}