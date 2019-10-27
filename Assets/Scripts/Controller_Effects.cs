using System;
using UnityEngine;

public class Controller_Effects : MonoBehaviour
{
    [SerializeField] ParticleSystem hitParticles = default;
    [SerializeField] ParticleSystem ballTrail = default;

    private void OnEnable()
    {
        Controller_Bat.OnShotPlayed_Point += PlayHitEffect;
        Controller_Ball.OnBallReset += OnBallReset;
    }

    private void OnDisable()
    {
        Controller_Bat.OnShotPlayed_Point -= PlayHitEffect;
        Controller_Ball.OnBallReset -= OnBallReset;
    }

    public void PlayHitEffect(Vector3 hitPoint)
    {
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();
        ballTrail.Play();
    }

    private void OnBallReset()
    {
        hitParticles.Stop();
        ballTrail.Stop();
    }
}