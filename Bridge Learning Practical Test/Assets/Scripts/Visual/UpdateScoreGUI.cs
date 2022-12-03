using TMPro;
using UnityEngine;


public sealed class UpdateScoreGUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTMP;

    private void Awake() 
    {
        UpdateGameVisual();
        PlayerData.OnScoreChanged += UpdateGameVisual;    
    }

    public void UpdateGameVisual()
    {
        scoreTMP.text = $"Score: {PlayerData.PlayerScore}";
    }
}
