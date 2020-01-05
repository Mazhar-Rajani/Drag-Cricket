using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] private AudioSource hitSound = default;
    [SerializeField] private AudioSource crowdCheerSound = default;

    private void OnEnable()
    {
        Bat.OnShotPlayed += PlayHitSound;
        // Bat.OnBoundaryScored += PlayCheerSound;
    }

    private void OnDisable()
    {
        Bat.OnShotPlayed -= PlayHitSound;
        // Bat.OnBoundaryScored -= PlayCheerSound;
    }

    private void PlayHitSound(Vector3 hitPoint)
    {
        hitSound.Stop();
        hitSound.Play();
    }

    private void PlayCheerSound()
    {
        crowdCheerSound.Stop();
        crowdCheerSound.Play();
    }
}