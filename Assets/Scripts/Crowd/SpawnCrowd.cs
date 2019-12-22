using System.Collections.Generic;
using UnityEngine;

public class SpawnCrowd : MonoBehaviour
{
    [SerializeField] private GameObject[] crowdSprites = default;
    [SerializeField] private int maxCrowdCount = default;
    [SerializeField] private float offsetPos = default;
    [SerializeField] private List<CrowdPlacementData> crowdPlacementData = default;
    [SerializeField] private List<GameObject> spawnedCrowd = default;

    private Transform crowdParent;

    public void Spawn()
    {
        ResetCrowd();
        crowdParent = new GameObject("Crowd Parent").transform;
        for (int i = 0; i < maxCrowdCount; i++)
        {
            int randomIndex = Random.Range(0, crowdSprites.Length);
            GameObject randomCrowd = crowdSprites[randomIndex];
            spawnedCrowd.Add(randomCrowd);
            SpawnOnPlane(randomCrowd);
        }
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    private void SpawnOnPlane(GameObject objectToSpawn)
    {
        int randomIndex = Random.Range(0, crowdPlacementData.Count);
        CrowdPlacementData data = crowdPlacementData[randomIndex];
        int randomAngle = Random.Range(data.minAngle, data.maxAngle);
        Vector3 randomPos = new Vector3(Mathf.Sin(randomAngle * Mathf.Deg2Rad), data.height, Mathf.Cos(randomAngle * Mathf.Deg2Rad));
        GameObject spawnedObject = Instantiate(objectToSpawn, crowdParent);
        spawnedObject.transform.position = randomPos * (data.radius + Random.Range(-offsetPos, offsetPos));
        Vector3 lookDir = spawnedObject.transform.position - Vector3.zero;
        spawnedObject.transform.rotation = Quaternion.LookRotation(new Vector3(lookDir.x, lookDir.y, lookDir.z), Vector3.up);
    }

    public void ResetCrowd()
    {
        if (spawnedCrowd.Count > 0)
        {
            if (crowdParent)
            {
                DestroyImmediate(crowdParent.gameObject);
            }
            spawnedCrowd.Clear();
        }
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < crowdPlacementData.Count; i++)
        {
            Vector3 minPos = new Vector3(Mathf.Sin(crowdPlacementData[i].minAngle * Mathf.Deg2Rad), crowdPlacementData[i].height, Mathf.Cos(crowdPlacementData[i].minAngle * Mathf.Deg2Rad));
            Vector3 maxPos = new Vector3(Mathf.Sin(crowdPlacementData[i].maxAngle * Mathf.Deg2Rad), crowdPlacementData[i].height, Mathf.Cos(crowdPlacementData[i].maxAngle * Mathf.Deg2Rad));
            Gizmos.DrawLine(Vector3.zero, minPos * crowdPlacementData[i].radius);
            Gizmos.DrawLine(Vector3.zero, maxPos * crowdPlacementData[i].radius);

            for (int j = crowdPlacementData[i].minAngle; j < crowdPlacementData[i].maxAngle; j++)
            {
                Vector3 pos = new Vector3(Mathf.Sin(j * Mathf.Deg2Rad), crowdPlacementData[i].height, Mathf.Cos(j * Mathf.Deg2Rad));
                Gizmos.DrawWireSphere(pos * crowdPlacementData[i].radius, 0.5f);
            }
        }
    }
}