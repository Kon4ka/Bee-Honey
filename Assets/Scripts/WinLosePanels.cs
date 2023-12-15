using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinLosePanel : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;

    public TextMeshProUGUI winScoreText;
    public TextMeshProUGUI loseScoreText;
    public BeeController beeController;
    public GameObject flowersGroup;

    private void Start()
    {
        // Подписываемся на событие обнуления счетчика пыльцы
        FindObjectOfType<BeeController>().OnPollenCountReset.AddListener(ResetFlowers);
        FindObjectOfType<BeeController>().OnPlayerHealthZero.AddListener(ShowLoseWindow);
    }


    public void ShowWinWindow(float score)
    {
        winPanel.SetActive(true);
        losePanel.SetActive(false);

        if (winScoreText != null)
        {
            winScoreText.text = score.ToString("F2");
        }

        // Запрещаем перемещение игроку
        beeController.CanMove = false;
    }

    public void ShowLoseWindow(float score)
    {
        winPanel.SetActive(false);
        losePanel.SetActive(true);

        if (loseScoreText != null)
        {
            loseScoreText.text = score.ToString();
        }
    }

    public void HideWindows()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    public void RestartLevel()
    {
        // Телепортация на начало уровня (предположим, у вас есть метод TeleportToStart() в BeeController)
        beeController.RestorePlayer();

        ResetFlowers();

        // Выключение WinPanel и LosePanel
        winPanel.SetActive(false);
        losePanel.SetActive(false);
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
}