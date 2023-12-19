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
            // ���� ���� ����������, ��������� �������� �� ����
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

        // �������� ����� ClearScoreText ����� 10 ������
        Invoke("ClearScoreText", 1f);
    }


    private void ClearScoreText()
    {
        // ������� �����
        score.SetText(string.Empty);
    }
}
