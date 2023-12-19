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
            mainCamera.transform.position = new Vector3(0f, 0f, 0f); // ���������� ��������� ���������� ������
            mainCamera.transform.rotation = Quaternion.identity; // �������� ������� ������
        }
        // ������������ �� ������ ������ (�����������, � ��� ���� ����� TeleportToStart() � BeeController)
        beeController.RestorePlayer();

        ResetFlowers();
        // ���������� WinPanel � LosePanel
        WinLosePanel.winPanel.SetActive(false);
        WinLosePanel.losePanel.SetActive(false);
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

    public void EscapeToMainMenu()
    {
        SceneManager.LoadScene(0);
        RestartLevel();
    }


}
