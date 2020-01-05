using UnityEngine;

public class Effects : MonoBehaviour
{
    [SerializeField] ParticleSystem hitParticles = default;
    [SerializeField] ParticleSystem ballTrail = default;
    [SerializeField] ParticleSystem confettiParticles = default;

    private void OnEnable()
    {
        Bat.OnShotPlayed += PlayHitEffect;
        Ball.OnBallReset += OnBallReset;
    }

    private void OnDisable()
    {
        Bat.OnShotPlayed -= PlayHitEffect;
        Ball.OnBallReset -= OnBallReset;
    }

    public void PlayHitEffect(Vector3 hitPoint)
    {
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();
        ballTrail.Play();
        confettiParticles.Play();
    }

    private void OnBallReset()
    {
        hitParticles.Stop();
        ballTrail.Stop();
        confettiParticles.Stop();
    }
}