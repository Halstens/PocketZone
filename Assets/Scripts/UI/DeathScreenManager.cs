using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private Button restartButton;
    //[SerializeField] private Button menuButton;
    
    [Header("Settings")]
    [SerializeField] private string gameSceneName = "SampleScene";
        //[SerializeField] private string menuSceneName = "MenuScene";
    
    private HealthSystem _playerHealth;
    
    void Start()
    {
        // Находим здоровье игрока
        _playerHealth = FindObjectOfType<PlayerController>()?.GetComponent<HealthSystem>();
        
        if (_playerHealth != null)
        {
            _playerHealth.OnDeath.AddListener(ShowDeathScreen);
        }
        
        // Настраиваем кнопки
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
            
        // if (menuButton != null)
        //     menuButton.onClick.AddListener(GoToMenu);
        
        // Скрываем экран смерти при старте
        if (deathScreen != null)
            deathScreen.SetActive(false);
    }
    
    void ShowDeathScreen()
    {
        // Показываем экран смерти
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
        }
        
        // Останавливаем игру
        Time.timeScale = 0f;
        
        // Отключаем управление игроком
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        
        // Отключаем оружие
        WeaponSystem weaponSystem = FindObjectOfType<WeaponSystem>();
        if (weaponSystem != null)
        {
            weaponSystem.enabled = false;
        }
        
        //Debug.Log("Экран смерти показан");
    }
    
    void RestartGame()
    {
        // Возобновляем время
        Time.timeScale = 1f;
        
        // Перезагружаем сцену
        SceneManager.LoadScene(gameSceneName);
    }
    
    // void GoToMenu()
    // {
    //     // Возобновляем время
    //     Time.timeScale = 1f;
    //     
    //     // Загружаем меню
    //     SceneManager.LoadScene(menuSceneName);
    // }
    
    void OnDestroy()
    {
        // Отписываемся от событий
        if (_playerHealth != null)
        {
            _playerHealth.OnDeath.RemoveListener(ShowDeathScreen);
        }
    }
}