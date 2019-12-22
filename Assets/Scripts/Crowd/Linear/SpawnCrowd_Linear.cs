using System.Collections.Generic;
using UnityEngine;

public class SpawnCrowd_Linear : MonoBehaviour
{
    [SerializeField] private GameObject[] crowdSprites = default;
    [SerializeField] private int maxCrowdCount = default;
    [SerializeField] private float offsetPos = default;
    [SerializeField] private List<CrowdPlacementData_Linear> crowdPlacementData = default;
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
        CrowdPlacementData_Linear data = crowdPlacementData[randomIndex];

        GameObject spawnedObject = Instantiate(objectToSpawn, crowdParent);

        Vector3 minPos = new Vector3(Mathf.Sin(-data.angle * Mathf.Deg2Rad), data.height, Mathf.Cos(-data.angle * Mathf.Deg2Rad));
        Vector3 maxPos = new Vector3(Mathf.Sin(data.angle * Mathf.Deg2Rad), data.height, Mathf.Cos(data.angle * Mathf.Deg2Rad));
        minPos *= data.radius;
        maxPos *= data.radius;

        float randomXPos = Random.Range(minPos.x, maxPos.x);
        Vector3 pos = new Vector3(randomXPos, data.height * data.radius, minPos.z + Random.Range(-offsetPos, offsetPos));
        spawnedObject.transform.position = pos;

        Vector3 lookDir = spawnedObject.transform.position - Vector3.zero;
        spawnedObject.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, lookDir.z), Vector3.up);
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < crowdPlacementData.Count; i++)
        {
            Vector3 minPos = new Vector3(Mathf.Sin(-crowdPlacementData[i].angle * Mathf.Deg2Rad), crowdPlacementData[i].height, Mathf.Cos(-crowdPlacementData[i].angle * Mathf.Deg2Rad));
            Vector3 maxPos = new Vector3(Mathf.Sin(crowdPlacementData[i].angle * Mathf.Deg2Rad), crowdPlacementData[i].height, Mathf.Cos(crowdPlacementData[i].angle * Mathf.Deg2Rad));

            minPos *= crowdPlacementData[i].radius;
            maxPos *= crowdPlacementData[i].radius;

            Gizmos.DrawLine(transform.position, minPos);
            Gizmos.DrawLine(transform.position, maxPos);

            for (float j = minPos.x; j < maxPos.x; j += 1)
            {
                Vector3 pos = new Vector3(j, crowdPlacementData[i].height * crowdPlacementData[i].radius, minPos.z);
                Gizmos.DrawWireSphere(pos, 0.5f);
            }
        }
    }
}