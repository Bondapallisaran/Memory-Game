using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateGridCards : MonoBehaviour
{
    [SerializeField] private Transform puzzleField;
    [SerializeField] private GameObject prefabCard;

    public List<Button> buttons = new List<Button>();

    // Instantiate cards with background sprite and assign names
    public void CreateCards(int totalCards)
    {
        // Clear previous cards
        foreach (Transform child in puzzleField)
        {
            Destroy(child.gameObject);
        }
        buttons.Clear();

        // Instantiate new cards
        for (int i = 0; i < totalCards; i++)
        {
            GameObject card = Instantiate(prefabCard, puzzleField);
            card.name = i.ToString();

           Button btn = card.GetComponent<Button>();
           buttons.Add(btn);
        }
    }

}
