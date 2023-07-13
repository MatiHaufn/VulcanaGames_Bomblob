using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameManager gameManager = (GameManager)target;

        if (GUILayout.Button("Draw Grid"))
        {
            gameManager.GridSystem.GetComponent<GridSystem>().DrawGrid(); 
        }
    }
}
