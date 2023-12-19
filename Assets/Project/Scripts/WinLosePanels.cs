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
        // ������������� �� ������� ��������� �������� ������
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

        // ��������� ����������� ������
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
        // ������������ �� ������ ������ (�����������, � ��� ���� ����� TeleportToStart() � BeeController)
        beeController.RestorePlayer();

        ResetFlowers();

        // ���������� WinPanel � LosePanel
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    private void ResetFlowers()
    {
        // �������� ��� ������ � ������
        Flower[] flowers = flowersGroup.GetComponentsInChildren<Flower>();

        // �������� ����� ResetFlower ��� ������� ������
        foreach (Flower flower in flowers)
        {
            flower.ResetFlower();
        }
    }
}