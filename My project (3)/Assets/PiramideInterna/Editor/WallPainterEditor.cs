using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WallPainter))]
public class WallPainterEditor : Editor
{
    private void OnSceneGUI()
    {
        Event e = Event.current;
        WallPainter painter = (WallPainter)target;

        // Só responde ao clique esquerdo sem ALT pressionado
        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            // Cria um plano perpendicular ao eixo Z (parede vertical)
            Plane wallPlane = new Plane(Vector3.forward, painter.origin + new Vector3(0, 0, painter.gridDepthZ));
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (wallPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);

                // Snap para a grade
                float tileSizeX = painter.tileSize.x;
                float tileSizeY = painter.tileSize.y;
                Vector3 origin = painter.origin;

                float snappedX = Mathf.Floor((hitPoint.x - origin.x) / tileSizeX) * tileSizeX + origin.x + tileSizeX / 2f;
                float snappedY = Mathf.Floor((hitPoint.y - origin.y) / tileSizeY) * tileSizeY + origin.y + tileSizeY / 2f;
                float fixedZ = origin.z + painter.gridDepthZ;

                Vector3 snappedPosition = new Vector3(snappedX, snappedY, fixedZ);

                // Instancia o tile
                GameObject newTile = (GameObject)PrefabUtility.InstantiatePrefab(painter.wallPrefab);
                Undo.RegisterCreatedObjectUndo(newTile, "Place Wall Tile");
                newTile.transform.position = snappedPosition;

                e.Use();
            }
        }
    }
}
