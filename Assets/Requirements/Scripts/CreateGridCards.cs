using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGridCards : MonoBehaviour
{
    [SerializeField] private Transform puzzleField;
    [SerializeField] private GameObject prefabCard;
    public int size;

    private void Awake()
    {
        for (int i = 0; i < size; i++)
        {
            GameObject button = Instantiate(prefabCard);
            button.name = "Button "+i.ToString();
            button.transform.SetParent(puzzleField.transform, false);
        }
    }

 
}
