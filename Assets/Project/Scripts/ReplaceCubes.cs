using UnityEngine;

public class ReplaceCubes : MonoBehaviour
{
    // Переменная для хранения префаба, который будет создаваться вместо кубов
    public GameObject prefab;

    // Метод, который вызывается при запуске сцены
    void Start()
    {
        // Находим все дочерние объекты уровня 1, которые содержат в имени Cube
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Cube"))
            {
                // Выключаем видимость объекта в игре
                child.gameObject.SetActive(false);

                // Получаем компонент коллайдера объекта
                Collider collider = child.GetComponent<Collider>();

                // Вычисляем координаты точки, в которой будет создан префаб
                Vector3 position = collider.bounds.center - collider.bounds.extents;

                // Создаем префаб в вычисленной точке с тем же поворотом, что и объект
                Instantiate(prefab, position, child.rotation, transform);
            }
        }
    }
}
