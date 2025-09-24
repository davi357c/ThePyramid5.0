using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FloorPainter))]
public class FloorPainterEditor : Editor
{
    private void OnSceneGUI()
    {
        Event e = Event.current;
        FloorPainter painter = (FloorPainter)target;

        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                float tileSizeX = painter.tileSize.x;
                float tileSizeZ = painter.tileSize.y;
                float heightY = painter.gridHeightY;
                Vector3 origin = painter.origin;

                // Calcula posição baseada na origin e tileSize
                Vector3 pos = hit.point;
                pos = new Vector3(
                    Mathf.Floor((pos.x - origin.x) / tileSizeX) * tileSizeX + origin.x + tileSizeX / 2f,
                    heightY,
                    Mathf.Floor((pos.z - origin.z) / tileSizeZ) * tileSizeZ + origin.z + tileSizeZ / 2f
                );

                // Instancia o chão
                GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(painter.floorPrefab);
                Undo.RegisterCreatedObjectUndo(obj, "Place Floor Tile");
                obj.transform.position = pos;

                e.Use(); // Impede que o evento continue sendo processado
            }
        }
    }
}
