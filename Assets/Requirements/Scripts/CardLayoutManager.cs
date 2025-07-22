using System.Buffers.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class CardLayoutManager : MonoBehaviour
{
    public Transform cardContainer; // UI container that holds the cards (with GridLayoutGroup)

    public TMP_Dropdown layoutDropDown; 

    private int rows, columns; 

    public CreateGridCards createGridCards; // Reference to the script that instantiates card buttons
    public GameManager gameManager;         // Reference to the GameManager to initialize game logic


    // Start is called before the first frame update
    void Start()
    {
        layoutDropDown.onValueChanged.AddListener(OnLayoutChanged);
        OnLayoutChanged(layoutDropDown.value);
    }
  //  Based on the dropdown(index), it sets the number of rows and columns.
  //  Instantiates exactly that many card buttons.
  //  Then initializes the game logic with these cards.
    void OnLayoutChanged(int index)
    {
        switch(index)
        {
            case 0: SetGridLayout(2, 2);break;
            case 1: SetGridLayout(2, 3); break;
            case 2: SetGridLayout(2, 4); break;
            case 3: SetGridLayout(2, 5); break;
        }
       int totalCards = rows * columns;
       createGridCards.CreateCards(totalCards);
        gameManager.SetupNewGame(createGridCards.buttons, totalCards);
    }

   // Dynamically adjusts card size and layout to fit rows × columns inside the container and maintains spacing all cards. 
   public void SetGridLayout(int r,int c)
    {
        rows = r; columns = c;

        GridLayoutGroup grid = cardContainer.GetComponent<GridLayoutGroup>();
        RectTransform containerRect = cardContainer.GetComponent<RectTransform>();

        float containerWidth = containerRect.rect.width ;
        float containerHeight = containerRect.rect.height ;

        float spacing = grid.spacing.x;
        float cellWidth = (containerWidth - (columns - 1) * spacing) / columns;
        float cellHeight = (containerHeight - (rows - 1) * spacing) / rows;

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.cellSize = new Vector2 (cellWidth, cellHeight);
    }

  
}
