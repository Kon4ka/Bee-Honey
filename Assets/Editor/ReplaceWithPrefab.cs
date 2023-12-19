/*using UnityEngine;
using UnityEditor;
using System.Diagnostics;

public class ReplaceWithPrefab : ScriptableWizard
{
    [SerializeField] private GameObject prefab; // ������, �� ������� ����� �������� ����

    [MenuItem("Tools/Replace With Prefab")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Replace With Prefab", typeof(ReplaceWithPrefab), "Replace");
    }

    void OnWizardCreate()
    {
        // �������� ��� ��������� ������� � ���������
        GameObject[] objects = Selection.gameObjects;

        // �������� �� ������� �������
        foreach (GameObject go in objects)
        {
            // ���������, ��� ������ ����� ���, ������������ � Cube
            if (go.name.StartsWith("Cube"))
            {
                // ���������� ��� ��������� ���������, ������� � �������
                Vector3 position = go.transform.localPosition;
                Quaternion rotation = go.transform.localRotation;
                Vector3 scale = go.transform.localScale;

                // ��������� ��������� ����
                go.GetComponent<Renderer>().enabled = false;

                // ������� ����� ������ �� �������
                GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                // ������������� ��� �� �� ��������� ���������, ������� � �������, ��� ���� � ����
                newObject.transform.localPosition = position;
                newObject.transform.localRotation = rotation;
                newObject.transform.localScale = scale;

                // �������� ������ �� ������ level1
                GameObject level1 = GameObject.Find("level1");

                // ���������, ��� level1 �� ����� null
                if (level1 != null)
                {
                    // ������ ����� ������ �������� level1
                    newObject.transform.SetParent(level1.transform);
                }
                else
                {
                    // ������� ��������� �� ������
                    Debug.LogError("�� ������� ����� ������ level1");
                }

                // ��������� ����� ������ � �����
                Undo.RegisterCreatedObjectUndo(newObject, "Replace With Prefabs");
            }
        }
    }
}
*/