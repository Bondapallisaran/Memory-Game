using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CardLayoutManager : MonoBehaviour
{
    public Transform cardContainer; // UI container that holds the cards (with GridLayoutGroup)

    public TMP_Dropdown layoutDropDown; 

    private int rows, columns; 

    public CreateGridCards createGridCards; // Reference to the script that instantiates card buttons
    public GameManager gameManager;         // Reference to the GameManager to initialize game logic

    private int lastWidth;
    private int lastHeight;
    private int selectedGrid;
    private int selectedRow;
    // Start is called before the first frame update
    void Start()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;

        layoutDropDown.onValueChanged.AddListener(OnLayoutChanged);
        selectedGrid = layoutDropDown.value;
        OnLayoutChanged(selectedGrid);
    }

    void Update()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            lastWidth = Screen.width;
            lastHeight = Screen.height;

            if (Screen.width > Screen.height)
            {
                Debug.Log("Switched to Landscape");
            }                
            else
            {
                Debug.Log("Switched to Portrait");
            }
            OnLayoutChanged(selectedGrid);
        }
    }

    //  Based on the dropdown(index), it sets the number of rows and columns.
    //  Instantiates exactly that many card buttons.
    //  Then initializes the game logic with these cards.
    void OnLayoutChanged(int index)
    {
        selectedGrid = layoutDropDown.value;
        switch (index)
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

        grid.constraintCount = columns;
        //Debug.Log($"rows ={r}, columns = {c}"); 
        
        float paddingLeft = grid.padding.left;
        float paddingRight = grid.padding.right;
        float paddingTop = grid.padding.top;
        float paddingBottom = grid.padding.bottom;
        float containerWidth = containerRect.rect.width ;
        float containerHeight = containerRect.rect.height ;

        float rValue = 0, cValue = 0;

        if (containerHeight < containerWidth)
        {
            rValue = r;
            cValue = c;
        }
        else 
        {
            rValue = c;
            cValue = r;
        }

        //Debug.Log($"paddingLeft ={paddingLeft}, paddingRight = {paddingRight}, paddingTop = {paddingTop}, paddingBottom = {paddingBottom}, containerWidth = {containerWidth} , containerHeight = {containerHeight} ");
        
        float spacing = grid.spacing.x;
        float totalWidthSpacing = paddingLeft + paddingRight + ((cValue - 1) * spacing);    
        float totalHeightSpacing = paddingTop + paddingBottom + ((rValue - 1) * spacing);

        //Debug.Log($"spacing ={spacing}, totalWidthSpacing = {totalWidthSpacing}, totalHeightSpacing = {totalHeightSpacing} ");


        float cellWidth = ((containerWidth - totalWidthSpacing))/cValue;
        float cellHeight = ((containerHeight - totalHeightSpacing)) / rValue;

        //Debug.Log($"cellWidth ={cellWidth}, cellHeight = {cellHeight}");


        float actuallCellSize = 0;
        if (containerHeight < containerWidth)
        {
            if (cellHeight < cellWidth) 
                actuallCellSize = cellHeight;
            else
                actuallCellSize = cellWidth;
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        }
        else
        {
            if (cellWidth < cellHeight )
                actuallCellSize = cellWidth;
            else
                actuallCellSize = cellHeight;

            grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        }
        
        //Debug.Log($"actuallCellSize ={actuallCellSize}");


        grid.cellSize = new Vector2 (actuallCellSize,actuallCellSize);
    }  
}

