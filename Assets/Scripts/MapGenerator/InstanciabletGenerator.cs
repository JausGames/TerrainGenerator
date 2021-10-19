using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InstanciabletGenerator
{
    public static ObjectsData[] InstanciateObjects(InstanciableData objets, HeightMap heightMap, MeshSettings meshSetting, HeightMapSettings heightSettings)
    {
        if (objets == null) return null;
        Debug.Log("InstanciateObjects, objects lenght = " + objets.layers.Length);
        Debug.Log("InstanciateObjects, heightMap X lenght = " + heightMap.values.GetLength(0));
        Debug.Log("InstanciateObjects, heightMap Y lenght = " + heightMap.values.GetLength(1));
        ObjectsData[] objsData = new ObjectsData[objets.layers.Length];
        for(int i = 0; i < objets.layers.Length; i++)
        {
            var objData = new ObjectsData();
            objData.SetPrefab(objets.layers[i].visual);
            for(int x = 0; x < heightMap.values.GetLength(0); x++)
            {
                for (int y = 0; y < heightMap.values.GetLength(1); y++)
                {
                    var factor = meshSetting.meshScale * meshSetting.chunkSizeIndex;

                    Vector2 topLeft = new Vector2(-1, 1) * meshSetting.meshWorldSize / 2f;
                    Vector2 percent = new Vector2(x - 1, y - 1) / (meshSetting.numVertsPerLine - 3);
                    Vector2 vertexPosition2D = topLeft + new Vector2(percent.x, -percent.y) * meshSetting.meshWorldSize;

                    if (heightMap.values[x, y] < objets.layers[i].endHeight && heightMap.values[x, y] > objets.layers[i].startHeight)
                        objData.AddWorldPos(vertexPosition2D.x * Vector3.right + 
                                            heightSettings.EvaluateHeight(heightMap.values[x, y]) * Vector3.up + 
                                            vertexPosition2D.y * Vector3.forward);
                }
            }
            objsData[i] = objData;
        }
        return objsData;
    }

}
public class ObjectsData
{
    GameObject prefab;
    List<Vector3> worldPositions = new List<Vector3>();

    public GameObject GetPrefab()
    {
        return prefab;
    }
    public void SetPrefab(GameObject prefab)
    {
        this.prefab = prefab;
    }
    public void AddWorldPos(Vector3 pos)
    {
        Debug.Log("InstanciabmeGenerator, pos = " + pos);
        worldPositions.Add(pos);
    }
    public List<Vector3> GetWorldPositions()
    {
        return worldPositions;
    }
}
