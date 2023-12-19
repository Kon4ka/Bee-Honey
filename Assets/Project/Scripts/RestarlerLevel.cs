using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RestarlerLevel : MonoBehaviour
{
    public BeeController beeController;
    public GameObject flowersGroup;
    public WinLosePanel WinLosePanel;
    public Camera mainCamera;

    public void RestartLevel()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(0f, 0f, 0f); // Установите начальные координаты камеры
            mainCamera.transform.rotation = Quaternion.identity; // Сбросьте поворот камеры
        }
        // Телепортация на начало уровня (предположим, у вас есть метод TeleportToStart() в BeeController)
        beeController.RestorePlayer();

        ResetFlowers();
        // Выключение WinPanel и LosePanel
        WinLosePanel.winPanel.SetActive(false);
        WinLosePanel.losePanel.SetActive(false);
    }

    private void ResetFlowers()
    {
        // Получаем все цветки в группе
        Flower[] flowers = flowersGroup.GetComponentsInChildren<Flower>();

        // Вызываем метод ResetFlower для каждого цветка
        foreach (Flower flower in flowers)
        {
            flower.ResetFlower();
        }
    }

    public void EscapeToMainMenu()
    {
        SceneManager.LoadScene(0);
        RestartLevel();
    }


}
