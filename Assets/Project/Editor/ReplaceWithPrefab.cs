/*using UnityEngine;
using UnityEditor;
using System.Diagnostics;

public class ReplaceWithPrefab : ScriptableWizard
{
    [SerializeField] private GameObject prefab; // префаб, на который нужно заменить кубы

    [MenuItem("Tools/Replace With Prefab")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Replace With Prefab", typeof(ReplaceWithPrefab), "Replace");
    }

    void OnWizardCreate()
    {
        // получаем все выбранные объекты в редакторе
        GameObject[] objects = Selection.gameObjects;

        // проходим по каждому объекту
        foreach (GameObject go in objects)
        {
            // проверяем, что объект имеет имя, начинающееся с Cube
            if (go.name.StartsWith("Cube"))
            {
                // запоминаем его локальное положение, поворот и масштаб
                Vector3 position = go.transform.localPosition;
                Quaternion rotation = go.transform.localRotation;
                Vector3 scale = go.transform.localScale;

                // отключаем рендеринг куба
                go.GetComponent<Renderer>().enabled = false;

                // создаем новый объект из префаба
                GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                // устанавливаем ему те же локальное положение, поворот и масштаб, что были у куба
                newObject.transform.localPosition = position;
                newObject.transform.localRotation = rotation;
                newObject.transform.localScale = scale;

                // получаем ссылку на объект level1
                GameObject level1 = GameObject.Find("level1");

                // проверяем, что level1 не равен null
                if (level1 != null)
                {
                    // делаем новый объект дочерним level1
                    newObject.transform.SetParent(level1.transform);
                }
                else
                {
                    // выводим сообщение об ошибке
                    Debug.LogError("Не удалось найти объект level1");
                }

                // добавляем новый объект в сцену
                Undo.RegisterCreatedObjectUndo(newObject, "Replace With Prefabs");
            }
        }
    }
}
*/