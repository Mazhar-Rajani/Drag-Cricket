using UnityEngine;

public class ResetGame : MonoBehaviour
{
    [SerializeField] private Controller_Ball ball = default;
    [SerializeField] private Controller_Bat bat = default;
    [SerializeField] private GameObject ui = default;

    public void OnClick_ResetGane()
    {
        ui.SetActive(false);
        ball.gameObject.SetActive(true);
        bat.gameObject.SetActive(true);
    }
}