using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public sealed class SpawnManager : MonoBehaviour
{
    [SerializeField] private ModelSO[] models;
    [SerializeField] private ObstacleSO obstacle;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int maxObstacles = 5;
    [field: SerializeField] public Transform Player { get;  private set; }

    private WaitForSeconds shortWait = new WaitForSeconds(2);
    private List<Model> spawnedModels = new List<Model>();
    private List<Model> activeSpawnedModels = new List<Model>();
    private List<Model> inactiveSpawnedModels = new List<Model>();

    public static SpawnManager Instance { get; private set; }

    private void Awake() 
    {
        if(!Instance)
        {
            Instance = this;
        }

        StartCoroutine(SpawnRandomModelCO());
        StartCoroutine(SpawnObstacles());
    }

    private void SpawnRandomModel()
    {
        int randomModelIndex = Random.Range(0, models.Length);
        int randomPointIndex = Random.Range(0, spawnPoints.Length);

        if(!models[randomModelIndex].IsSpawnedInScene)
        {
            SpawnModel(randomModelIndex, randomPointIndex);
            return;
        }

        randomModelIndex = RetrieveCorrectModelIndex(spawnedModels, models[randomModelIndex].Prefab.name);
        ActivateModel(randomModelIndex, randomModelIndex);
    }

    private IEnumerator SpawnRandomModelCO()
    {
        yield return shortWait;
        SpawnRandomModel();
    }

    private void SpawnModel(int modelIndex, int spawnPointIndex)
    {
        if(modelIndex >= models.Length)
        {
            Debug.LogError("Cannot spawn a model that does not exist");
            return;
        }

        if(spawnPoints == null || spawnPointIndex >= spawnPoints.Length)
        {
            Debug.LogError("Nowhere to spawn model");
            return;
        }

        var model = Instantiate(models[modelIndex].Prefab, spawnPoints[spawnPointIndex].position, Quaternion.identity).GetComponent<Model>();
        spawnedModels.Add(model);
        activeSpawnedModels.Add(model);
        model.OnModelDeactivated += OnModelDeactivated;
        model.OnModelActivated += OnModelActivated;
        model.OnPlayerCollided.AddListener(() => UpdateScore(models[modelIndex].Points));
        models[modelIndex].IsSpawnedInScene = true;
    }

    private void OnModelDeactivated(Model model)
    {
        activeSpawnedModels.Remove(model);
        inactiveSpawnedModels.Add(model);
        SpawnRandomModel();
    }

    private void OnObstacleDeactivated(Model model)
    {
        activeSpawnedModels.Remove(model);
        inactiveSpawnedModels.Add(model);
        StartCoroutine(EnableObstacle(model));
    }

    private void OnModelActivated(Model model)
    {
        inactiveSpawnedModels.Remove(model);
        activeSpawnedModels.Add(model);
    }

    private void ActivateModel(int modelIndex, int spawnPointIndex)
    {
        if(modelIndex < 0 )
        {
            //try again
            StartCoroutine(SpawnRandomModelCO());
            return;
        }

        if(modelIndex >= spawnedModels.Count)
        {
            Debug.LogError("Cannot enable a model that does not exist");
            return;
        }

        if(spawnPoints == null || spawnPointIndex >= spawnPoints.Length || spawnPointIndex < 0)
        {
            Debug.LogError("Nowhere to move model to");
            return;
        }

        var model  = spawnedModels[modelIndex];
        model.transform.position = spawnPoints[spawnPointIndex].position;
        model.Activate();
    }

    public void SetModelsToDefault()
    {
        foreach (ModelSO model in models)
        {
            model.IsSpawnedInScene = false;
        }
    }

    private void OnApplicationQuit() 
    {
        #if (UNITY_EDITOR)
        SetModelsToDefault();
        SetScoreToDefault();
        #endif
    }

    private void UpdateScore(int addedPoints)
    {
        PlayerData.IncreaseScore(addedPoints);
        Debug.Log("Update Score");
    }

    private int RetrieveCorrectModelIndex(List<Model> spawnedModels, string modelName)
    {
        return spawnedModels.IndexOf(spawnedModels.FirstOrDefault(model => model.name.Contains(modelName)));
    }

    private void SetScoreToDefault()
    {
        PlayerData.SetDefaultScore();
    }

    private IEnumerator SpawnObstacles()
    {
        int spawnedObstacles  = 0;
        int randomPointIndex = 0;
        yield return shortWait;

        while (spawnedObstacles < maxObstacles)
        {
            randomPointIndex = Random.Range(0, spawnPoints.Length);
            //spawn obstacles 1 after the other until max obstacles have been spawned
            SpawnObstacle(randomPointIndex);
            spawnedObstacles++;
            yield return shortWait;
        }
        yield return null;
        
    }

    private void SpawnObstacle(int spawnPointIndex)
    {
        if(!this.obstacle)
        {
            Debug.LogError("Cannot spawn an obstacle that does not exist");
            return;
        }

        if(spawnPoints == null || spawnPointIndex >= spawnPoints.Length)
        {
            Debug.LogError("Nowhere to spawn obstacle");
            return;
        }

        var obstacle = Instantiate(this.obstacle.Prefab, spawnPoints[spawnPointIndex].position, Quaternion.identity).GetComponent<Model>();
        spawnedModels.Add(obstacle);
        activeSpawnedModels.Add(obstacle);
        obstacle.OnModelDeactivated += OnObstacleDeactivated;
        obstacle.OnModelActivated += OnModelActivated;
        obstacle.OnPlayerCollided.AddListener(() => DecreasePlayerLife(this.obstacle.Damage));
        this.obstacle.IsSpawnedInScene = true;
    }

    private IEnumerator EnableObstacle(Model obstacle)
    {
        obstacle.HasStartedCountdown = true;
        yield return obstacle.ShortWait;
        obstacle.Activate();
    }

    private void OnDisable() 
    {
        StopAllCoroutines();
    }

    public void DeactivateAllRigidbodies()
    {
        StopAllCoroutines();

        foreach (Model spawnedModel in spawnedModels)
        {
            spawnedModel.gameObject.SetActive(false);
        }

        //make player disappear
        Player.gameObject.SetActive(false);
    }

    public void ActivateAllRigidbodies()
    {

        foreach (Model spawnedModel in spawnedModels)
        {
            spawnedModel.gameObject.SetActive(true);
        }

        //make player disappear
        Player.gameObject.SetActive(true);
    }

    private void DecreasePlayerLife(int byAmount)
    {
        PlayerData.DecreaseLife(byAmount);
        Debug.Log("Decrease Life");
    }
}