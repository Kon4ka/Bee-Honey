using UnityEngine;

public class Flower : MonoBehaviour
{
    private bool isPicked = false; // Флаг, указывающий, подобран ли цветок
    private Vector3 initialPosition; // Исходная позиция цветка

    void Start()
    {
        initialPosition = transform.position;
    }

    public bool IsPicked
    {
        get { return isPicked; }
    }

    public void HideFlower()
    {
        isPicked = true;
        // Здесь можно добавить анимацию ухода цветка под землю или другие дополнительные эффекты
        transform.position = new Vector3(initialPosition.x, -10f, initialPosition.z);
        Invoke("ResetFlower", 10f); // Инвокация возвращения цветка через 10 секунд
    }

    void ResetFlower()
    {
        isPicked = false;
        // Здесь можно добавить анимацию возвращения цветка на место
        transform.position = initialPosition;
    }
}
