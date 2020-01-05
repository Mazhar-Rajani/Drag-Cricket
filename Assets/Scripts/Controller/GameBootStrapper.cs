using UnityEngine;

public class GameBootStrapper : MonoBehaviour
{
    public static event System.Action OnGameBootStrap;

    private void Start()
    {
#if UNITY_EDITOR
        Debug.Log("Game Start");
#endif
        OnGameBootStrap?.Invoke();
    }
}