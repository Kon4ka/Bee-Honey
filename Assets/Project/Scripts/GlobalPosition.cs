using UnityEngine;

public class GlobalPosition : MonoBehaviour
{
    // переменная для хранения глобальных координат
    public Vector3 globalPosition;

    void Update()
    {
        // получаем глобальные координаты объекта с помощью свойства transform.position
        globalPosition = transform.position;

        // выводим глобальные координаты в консоль
        Debug.Log("Глобальные координаты объекта: " + globalPosition);
    }
}
