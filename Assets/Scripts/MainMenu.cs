using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI score;
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void ScoreButton()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "record.txt");

        if (File.Exists(filePath))
        {
            // Если файл существует, считываем значение из него
            string scoreString = File.ReadAllText(filePath);

            if (int.TryParse(scoreString, out int playerScore))
            {
                score.SetText(playerScore.ToString());
            }
            else
            {
                score.SetText("Invalid Format");
            }
        }
        else
        {
            score.SetText("No Score Available");
        }

        // Вызываем метод ClearScoreText через 10 секунд
        Invoke("ClearScoreText", 1f);
    }


    private void ClearScoreText()
    {
        // Очищаем текст
        score.SetText(string.Empty);
    }
}
