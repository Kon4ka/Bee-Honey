using UnityEngine;

public class ReplaceCubes : MonoBehaviour
{
    // ���������� ��� �������� �������, ������� ����� ����������� ������ �����
    public GameObject prefab;

    // �����, ������� ���������� ��� ������� �����
    void Start()
    {
        // ������� ��� �������� ������� ������ 1, ������� �������� � ����� Cube
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Cube"))
            {
                // ��������� ��������� ������� � ����
                child.gameObject.SetActive(false);

                // �������� ��������� ���������� �������
                Collider collider = child.GetComponent<Collider>();

                // ��������� ���������� �����, � ������� ����� ������ ������
                Vector3 position = collider.bounds.center - collider.bounds.extents;

                // ������� ������ � ����������� ����� � ��� �� ���������, ��� � ������
                Instantiate(prefab, position, child.rotation, transform);
            }
        }
    }
}
