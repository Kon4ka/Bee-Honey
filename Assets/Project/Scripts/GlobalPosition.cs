using UnityEngine;

public class GlobalPosition : MonoBehaviour
{
    // ���������� ��� �������� ���������� ���������
    public Vector3 globalPosition;

    void Update()
    {
        // �������� ���������� ���������� ������� � ������� �������� transform.position
        globalPosition = transform.position;

        // ������� ���������� ���������� � �������
        Debug.Log("���������� ���������� �������: " + globalPosition);
    }
}
