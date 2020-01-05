using UnityEngine;

public class GameToMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] props = default;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void EnableProps()
    {
        for (int i = 0; i < props.Length; i++)
        {
            props[i].SetActive(true);
        }
    }

    private void DisableProps()
    {
        for (int i = 0; i < props.Length; i++)
        {
            props[i].SetActive(false);
        }
    }
}