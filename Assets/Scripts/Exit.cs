using System.IO;
using System;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public WinLosePanel winLosePanel;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BeeController beeController = other.GetComponent<BeeController>();
            if (beeController != null && beeController.GetPollenCount() > 0)
            {
                ShowWinPanel(beeController.GetPlayerLifetime());
            }
        }
    }

    void ShowWinPanel(float playerLifetime)
    {
        if (winLosePanel != null)
        {
            UpdateRecord(playerLifetime);
        }
            if (winLosePanel != null)
        {
            winLosePanel.ShowWinWindow(playerLifetime);
        }
    }
    private void UpdateRecord(float playerLifetime)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "record.txt");

        if (File.Exists(filePath))
        {
            // Чтение текущего рекорда из файла
            string recordString = File.ReadAllText(filePath);

            if (float.TryParse(recordString, out float currentRecord))
            {
                // Обновление рекорда, если текущий счет меньше
                if (playerLifetime < currentRecord)
                {
                    WriteRecord(filePath, playerLifetime);
                }
            }
        }
        else
        {
            // Если файла нет, создаем его и записываем текущий рекорд
            WriteRecord(filePath, playerLifetime);
        }
    }

    private void WriteRecord(string filePath, float playerLifetime)
    {
        try
        {
            File.WriteAllText(filePath, playerLifetime.ToString());
        }
        catch (Exception e)
        {
            Debug.LogError("Error writing record to file: " + e.Message);
        }
    }
}
