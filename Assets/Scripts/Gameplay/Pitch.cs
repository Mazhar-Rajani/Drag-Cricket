using UnityEngine;

public class Pitch : MonoBehaviour
{
    [SerializeField] private Ball ball = default;
    [SerializeField] private Rigidbody ballRigidbody = default;
    [SerializeField] private int[] swingForces = default;

    private Collider pithcCollider;

    private void Awake()
    {
        pithcCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != ballRigidbody)
            return;

        int randomSwingIndex = Random.Range(0, swingForces.Length);
        int swing = swingForces[randomSwingIndex];
        Debug.Log("Random Swing Is  - " + swing);

        if (ball.transform.position.x > 0)
        {
            ballRigidbody.AddForce(new Vector3(-swing, -swing, -swing), ForceMode.Force);
        }
        else if (ball.transform.position.x < 0)
        {
            ballRigidbody.AddForce(new Vector3(swing, -swing, -swing), ForceMode.Force);
        }
        pithcCollider.enabled = false;
    }
}