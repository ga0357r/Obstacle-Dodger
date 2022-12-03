using TMPro;
using UnityEngine;

public sealed class UpdateLivesGUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI livesTMP;

    private void Awake() 
    {
        UpdateGameVisual();
        PlayerData.OnLifeChanged += UpdateGameVisual;    
    }

    public void UpdateGameVisual()
    {
        livesTMP.text = $"Lives: {PlayerData.PlayerLives}";
    }
}
