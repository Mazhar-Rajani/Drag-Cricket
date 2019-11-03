using UnityEngine;

public class Pitch : MonoBehaviour
{
    [SerializeField] private Ball ball = default;
    [SerializeField] private Rigidbody ballRigidbody = default;
    [SerializeField] private Vector3 minSwingForce = default;
    [SerializeField] private Vector3 maxSwingForce = default;

    private Collider pithcCollider;

    private void Awake()
    {
        pithcCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != ballRigidbody)
            return;

        float xSwing = Random.Range(minSwingForce.x, maxSwingForce.x);
        float ySwing = Random.Range(minSwingForce.y, maxSwingForce.y);
        float zSwing = Random.Range(minSwingForce.z, maxSwingForce.z);

        if (ball.transform.position.x > 0)
        {
            ballRigidbody.AddForce(new Vector3(-xSwing, -ySwing, -zSwing), ForceMode.Force);
        }
        else if (ball.transform.position.x < 0)
        {
            ballRigidbody.AddForce(new Vector3(xSwing, -ySwing, -zSwing), ForceMode.Force);
        }
        pithcCollider.enabled = false;
    }
}