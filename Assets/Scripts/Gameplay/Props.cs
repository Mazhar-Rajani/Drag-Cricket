using UnityEngine;

public class Props : MonoBehaviour
{
    [SerializeField] private GameObject[] props = default;

    private void OnEnable()
    {
        GameBootStrapper.OnGameBootStrap += DisableProps;
        Button_Play.OnPlayMatch += EnableProps;
    }

    private void OnDisable()
    {
        GameBootStrapper.OnGameBootStrap -= DisableProps;
        Button_Play.OnPlayMatch -= EnableProps;
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