using UnityEngine;

public class GameBootStrapper : MonoBehaviour
{
    public static event System.Action OnGameBootStrap;

    private void Start()
    {
        Debug.Log("Game Start");
        OnGameBootStrap?.Invoke();
    }
}