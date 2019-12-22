using UnityEngine;

public class BatHit : MonoBehaviour
{
    [SerializeField] private Collider ballCollider;
    [SerializeField] private Ball ball;

    private void OnTriggerEnter(Collider other)
    {
        if (other != ballCollider)
            return;

        ball.ResetThrow();
    }
}