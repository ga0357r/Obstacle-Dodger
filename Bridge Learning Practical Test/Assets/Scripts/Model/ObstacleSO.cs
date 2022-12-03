using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle", menuName = "New Model/Obstacle", order = 0)]
public sealed class ObstacleSO : ModelSO 
{
    [field:SerializeField] public int Damage { get; private set; }
}
