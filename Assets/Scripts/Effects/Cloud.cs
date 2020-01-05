using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private Vector2 cloudRotateSpeed = default;
    [SerializeField] private Vector2Int cloudSpawnRadius = default;
    [SerializeField] private Vector2Int cloudHeight = default;

    private Vector3 lookDirection;

    private void Start()
    {
        Vector3 randomPos = Random.onUnitSphere * Random.Range(cloudSpawnRadius.x, cloudSpawnRadius.y);
        transform.position = new Vector3(randomPos.x, Random.Range(cloudHeight.x, cloudHeight.y), randomPos.y);
    }

    private void Update()
    {
        lookDirection = Vector3.zero - transform.position;
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        transform.RotateAround(Vector3.zero, Vector3.up, Random.Range(cloudRotateSpeed.x, cloudRotateSpeed.y) * Time.deltaTime);
    }
}