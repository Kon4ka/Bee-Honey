using UnityEngine;

public class Flower : MonoBehaviour
{
    private bool isPicked = false; // ����, �����������, �������� �� ������
    private Vector3 initialPosition; // �������� ������� ������

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
        // ����� ����� �������� �������� ����� ������ ��� ����� ��� ������ �������������� �������
        transform.position = new Vector3(initialPosition.x, -10f, initialPosition.z);
        Invoke("ResetFlower", 10f); // ��������� ����������� ������ ����� 10 ������
    }

    void ResetFlower()
    {
        isPicked = false;
        // ����� ����� �������� �������� ����������� ������ �� �����
        transform.position = initialPosition;
    }
}
