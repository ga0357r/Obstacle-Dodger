
using UnityEngine;

[CreateAssetMenu(fileName = "Model", menuName = "New Model/Model", order = 0)]
public class ModelSO : ScriptableObject 
{
    [field:SerializeField] public int Points { get; private set; }
    [field:SerializeField] public GameObject Prefab { get; private set; }
    [field:SerializeField] public bool IsSpawnedInScene { get; set; }
}

