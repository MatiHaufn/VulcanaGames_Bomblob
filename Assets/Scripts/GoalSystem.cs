using System.Collections.Generic;
using UnityEngine;

public class GoalSystem : MonoBehaviour
{
    List<GameObject> _allGoals = new List<GameObject>();
    Vector3 _spawnPosition; 
    // Start is called before the first frame update
    void Start()
    {
        GameManager._instance._currentGoalY = transform.position.y + GameManager._instance._currentGoalYOffset;
        _spawnPosition = transform.position;
        SpawnNewGoal();
    }

    public void SpawnNewGoal()
    {
        GameObject newGoal = Instantiate(GameManager._instance._GoalPrefab);
        newGoal.transform.SetParent(transform);
        newGoal.transform.position = _spawnPosition + (Vector3.up * _allGoals.Count);
        GameManager._instance._currentGoal = newGoal;
        _allGoals.Add(newGoal); 
    }
}
