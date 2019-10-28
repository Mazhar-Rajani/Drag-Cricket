using UnityEngine;

public class Controller_Audio : MonoBehaviour
{
    [SerializeField] private AudioSource onShotPlayed = default;
    [SerializeField] private AudioSource crowdCheer = default;

    private void OnEnable()
    {
        Controller_Bat.OnShotPlayed += OnShotPlayed;
    }

    private void OnDisable()
    {
        Controller_Bat.OnShotPlayed -= OnShotPlayed;
    }

    private void OnShotPlayed()
    {
        onShotPlayed.Stop();
        onShotPlayed.Play();

        crowdCheer.Stop();
        crowdCheer.Play();
    }
}