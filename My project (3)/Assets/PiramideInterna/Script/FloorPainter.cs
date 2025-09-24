using UnityEngine;

public class FloorPainter : MonoBehaviour
{
    public GameObject floorPrefab;

    [Header("Tile Settings")]
    public Vector2 tileSize = new Vector2(1f, 1f); // Novo tamanho do tile (X, Z)
    public int gridSizeX = 10;
    public int gridSizeZ = 5;
    public float gridHeightY = 2f; // Altura da grade (eixo Y)
    public Vector3 origin = Vector3.zero; // Posição inicial (origin)

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        // Desenha linhas horizontais (baseadas no tamanho do tile)
        for (int x = 0; x <= gridSizeX; x++)
        {
            Vector3 start = origin + new Vector3(x * tileSize.x, gridHeightY, 0);
            Vector3 end = origin + new Vector3(x * tileSize.x, gridHeightY, gridSizeZ * tileSize.y);
            Gizmos.DrawLine(start, end);
        }

        // Desenha linhas verticais (baseadas no tamanho do tile)
        for (int z = 0; z <= gridSizeZ; z++)
        {
            Vector3 start = origin + new Vector3(0, gridHeightY, z * tileSize.y);
            Vector3 end = origin + new Vector3(gridSizeX * tileSize.x, gridHeightY, z * tileSize.y);
            Gizmos.DrawLine(start, end);
        }
    }
}
