using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public sealed class UpdateGUI : MonoBehaviour
{
    [SerializeField] private string winText;
    [SerializeField] private TextMeshProUGUI updateGUITextTMP;
    [SerializeField] private Image updateGUIPanel;
    [SerializeField] private float shortWaitTime = 1f;

    private WaitForSeconds ShortWait = null;

    public UnityEvent OnObjectDeactivated = null;

    private void Awake() 
    {
        SetShortTime(shortWaitTime);
    }

    public void UpdateGameVisual()
    {
        updateGUITextTMP.text = winText;
        updateGUITextTMP.transform.gameObject.SetActive(true);
        updateGUIPanel.gameObject.SetActive(true);
        StartCoroutine(StartCountdownToDeactivation());
    }

    private IEnumerator StartCountdownToDeactivation()
    {
        yield return ShortWait;
        Deactivate();
    }

    private void Deactivate()
    {
        updateGUITextTMP.transform.gameObject.SetActive(false);
        updateGUIPanel.gameObject.SetActive(false);
        OnObjectDeactivated?.Invoke();
    }

    private void SetShortTime(float waitTime)
    {
        ShortWait = new WaitForSeconds(waitTime);
    }

    private void OnDisable() 
    {
        StopCoroutine(StartCountdownToDeactivation());    
    }
}
