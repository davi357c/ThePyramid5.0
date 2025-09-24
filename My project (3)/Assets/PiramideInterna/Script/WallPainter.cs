using UnityEngine;

public class WallPainter : MonoBehaviour
{
    public GameObject wallPrefab;

    [Header("Tile Settings")]
    public Vector2 tileSize = new Vector2(1f, 1f); // X (largura), Y (altura)
    public int gridSizeX = 5;
    public int gridSizeY = 5;
    public float gridDepthZ = 0f; // profundidade fixa
    public Vector3 origin = Vector3.zero;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // Desenha colunas (verticais)
        for (int x = 0; x <= gridSizeX; x++)
        {
            Vector3 start = origin + new Vector3(x * tileSize.x, 0, gridDepthZ);
            Vector3 end = origin + new Vector3(x * tileSize.x, gridSizeY * tileSize.y, gridDepthZ);
            Gizmos.DrawLine(start, end);
        }

        // Desenha linhas horizontais
        for (int y = 0; y <= gridSizeY; y++)
        {
            Vector3 start = origin + new Vector3(0, y * tileSize.y, gridDepthZ);
            Vector3 end = origin + new Vector3(gridSizeX * tileSize.x, y * tileSize.y, gridDepthZ);
            Gizmos.DrawLine(start, end);
        }
    }
}
