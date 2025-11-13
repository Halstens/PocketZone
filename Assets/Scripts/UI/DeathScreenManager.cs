using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private Button restartButton;
   
    
    [Header("Settings")]
    [SerializeField] private string gameSceneName = "SampleScene";
        
    private HealthSystem _playerHealth;
    
    void Start()
    {
        _playerHealth = FindObjectOfType<PlayerController>()?.GetComponent<HealthSystem>();
        
        if (_playerHealth != null)
        {
            _playerHealth.OnDeath.AddListener(ShowDeathScreen);
        }
        
        
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        
        if (deathScreen != null)
            deathScreen.SetActive(false);
    }
    
    void ShowDeathScreen()
    {
       
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
        }
        
        
        Time.timeScale = 0f;
        
       
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        
       
        WeaponSystem weaponSystem = FindObjectOfType<WeaponSystem>();
        if (weaponSystem != null)
        {
            weaponSystem.enabled = false;
        }
        
    }
    
    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
    
    
    void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnDeath.RemoveListener(ShowDeathScreen);
        }
    }
}