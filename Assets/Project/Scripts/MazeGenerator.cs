using UnityEngine;
using UnityEditor;

public class MazeGenerator : MonoBehaviour
{
    public Texture2D levelImage;
    public GameObject levelParent;
    public GameObject bushPrefab;
    public GameObject thornyBushPrefab;
    public GameObject playerPrefab;
    public GameObject hivePrefab;
    public GameObject flowerPrefab;

    public float cellSize = 4.0f; // Установите cellSize на 4, если префабы в 4 раза больше

    private readonly Color GREEN = new Color(0.0f, 1.0f, 0.0f);
    private readonly Color RED = new Color(1.0f, 0.0f, 0.0f);
    private readonly Color BLUE = new Color(0.0f, 0.0f, 1.0f);
    private readonly Color YELLOW = new Color(1.0f, 1.0f, 0.0f);
    private readonly Color MAGENTA = new Color(1.0f, 0.0f, 1.0f);
    private readonly Color WHITE = new Color(1.0f, 1.0f, 1.0f);
    private const float COLOR_EPSILON = 0.03f;

    [ContextMenu("Generate Maze")]
    public void GenerateMaze()
    {
        if (levelImage == null || levelParent == null)
        {
            Debug.LogError("Level Image or Level Parent is not assigned.");
            return;
        }

        // Найти позицию игрока (синий пиксель)
        Vector2Int playerPosition = Vector2Int.zero;
        bool playerFound = false;

        for (int y = 0; y < levelImage.height && !playerFound; y++)
        {
            for (int x = 0; x < levelImage.width && !playerFound; x++)
            {
                Color pixelColor = levelImage.GetPixel(x, y);
                if (ColorsAreEqual(pixelColor, BLUE))
                {
                    playerPosition = new Vector2Int(x, y);
                    playerFound = true;
                }
            }
        }

        if (!playerFound)
        {
            Debug.LogError("Player position not found in the image.");
            return;
        }

        // Clear previous children
        Undo.RegisterFullObjectHierarchyUndo(levelParent, "Generate Maze");
        foreach (Transform child in levelParent.transform)
        {
            Undo.DestroyObjectImmediate(child.gameObject);
        }

        // Create collections
        GameObject bushCollection = new GameObject("bush_collection");
        GameObject thornyBushCollection = new GameObject("thorny_bush_collection");
        GameObject flowerCollection = new GameObject("flower_collection");
        GameObject player = new GameObject("player");

        Undo.RegisterCreatedObjectUndo(bushCollection, "Create bush_collection");
        Undo.RegisterCreatedObjectUndo(thornyBushCollection, "Create thorny_bush_collection");
        Undo.RegisterCreatedObjectUndo(flowerCollection, "Create flower_collection");
        Undo.RegisterCreatedObjectUndo(player, "Create player");

        bushCollection.transform.parent = levelParent.transform;
        thornyBushCollection.transform.parent = levelParent.transform;
        flowerCollection.transform.parent = levelParent.transform;
        player.transform.parent = levelParent.transform;

        // Iterate through each pixel in the image
        for (int y = 0; y < levelImage.height; y++)
        {
            for (int x = 0; x < levelImage.width; x++)
            {
                Color pixelColor = levelImage.GetPixel(x, y);
                Vector3 position = new Vector3((x - playerPosition.x) * cellSize, 0, (y - playerPosition.y) * cellSize);

                if (ColorsAreEqual(pixelColor, GREEN))
                {
                    GameObject instance = Instantiate(bushPrefab, position, Quaternion.identity, bushCollection.transform);
                    Undo.RegisterCreatedObjectUndo(instance, "Create Bush");
                }
                else if (ColorsAreEqual(pixelColor, RED))
                {
                    GameObject instance = Instantiate(thornyBushPrefab, position, Quaternion.identity, thornyBushCollection.transform);
                    Undo.RegisterCreatedObjectUndo(instance, "Create Thorny Bush");
                }
                else if (ColorsAreEqual(pixelColor, BLUE))
                {
                    GameObject instance = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, player.transform);
                    Undo.RegisterCreatedObjectUndo(instance, "Create Player");
                }
                else if (ColorsAreEqual(pixelColor, YELLOW)) // Yellow
                {
                    GameObject instance = Instantiate(hivePrefab, position, Quaternion.identity, levelParent.transform);
                    Undo.RegisterCreatedObjectUndo(instance, "Create Hive");
                }
                else if (ColorsAreEqual(pixelColor, MAGENTA)) // Magenta
                {
                    GameObject instance = Instantiate(flowerPrefab, position, Quaternion.identity, flowerCollection.transform);
                    Undo.RegisterCreatedObjectUndo(instance, "Create Flower");
                }
                else if (!ColorsAreEqual(pixelColor, WHITE))
                {
                    // Output debug information for unknown colors
                    Debug.LogWarning($"Unknown pixel color at ({x}, {y}): {pixelColor}");
                }
            }
            player.transform.Rotate(Vector3.up, 90);
        }
    }

    bool ColorsAreEqual(Color a, Color b, float epsilon = COLOR_EPSILON)
    {
        return Mathf.Abs(a.r - b.r) < epsilon &&
               Mathf.Abs(a.g - b.g) < epsilon &&
               Mathf.Abs(a.b - b.b) < epsilon &&
               Mathf.Abs(a.a - b.a) < epsilon;
    }
}
