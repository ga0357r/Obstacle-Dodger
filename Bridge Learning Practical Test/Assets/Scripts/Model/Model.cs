using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Model : MonoBehaviour
{
    [SerializeField] private float shortWaitTime = 2f;

    public UnityEvent OnPlayerCollided = null;
    public event Action<Model> OnModelDeactivated = null;
    public event Action<Model> OnModelActivated = null;
    public bool HasStartedCountdown { get; set; } = false;
    public WaitForSeconds ShortWait { get; private set; }
    private void Awake() 
    {
        Init();
        SetShortTime(shortWaitTime);
    }

    private void SetShortTime(float waitTime)
    {
        ShortWait = new WaitForSeconds(waitTime);
    }

    protected virtual void Init()
    {
        OnPlayerCollided.AddListener(BeginCountdownToDeactivation);
        OnPlayerCollided.AddListener(PlayerData.IncrementPushedObjAmt);
    }

    protected virtual void OnCollisionEnter(Collision other) 
    {   
        if(HasStartedCountdown)
        {
            return;
        }

        if (other.transform.CompareTag("Player"))
        {
            //Update Scores
            OnPlayerCollided?.Invoke();
        }
    }

    protected void BeginCountdownToDeactivation()
    {
        StartCoroutine(StartCountdownToDeactivation());
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
        OnModelDeactivated?.Invoke(this);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        OnModelActivated?.Invoke(this);
    }

    private IEnumerator StartCountdownToDeactivation()
    {
        HasStartedCountdown = true;
        yield return ShortWait;
        Deactivate();
    }

    private void OnDisable() 
    {
        StopCoroutine(StartCountdownToDeactivation());
        HasStartedCountdown = false;
    }
}
