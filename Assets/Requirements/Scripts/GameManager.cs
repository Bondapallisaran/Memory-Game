
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Button> buttons = new List<Button>(); // All card buttons
    public Sprite bgSprite;                           // Default background for cards
    public Sprite[] puzzles;                          // All available card images
    public List<Sprite> gamePuzzles = new List<Sprite>(); // Selected and shuffled card images

    private int countGuesses;             // Total number of guesses made
    private int countCorrectGuesses;      // Number of successful matches
    private int gameGuesses;              // Total number of pairs in current game


    private List<int> flippedIndexes = new List<int>();  // Stores indexes of flipped but unmatched cards

    private void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Sprites/Fruits");
    }

    void Start()
    {
        GetButtons();
        gameGuesses = gamePuzzles.Count / 2;
    }

    //  Find and store all cards in the scene tagged PuzzleButton.
    void GetButtons() 
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");
        for (int i = 0; i < objects.Length; i++)
        {
            buttons.Add(objects[i].GetComponent<Button>());
            buttons[i].image.sprite = bgSprite;
        }
    }

    // Attach PickPuzzle click logic to every card.
    void AddListeners()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(PickPuzzle);
        }
    }

    //Flip the clicked card.
   // Add its index to the list of flipped cards.
    //If 2 cards are flipped, trigger match-check.
    void PickPuzzle()
    {
        int index = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
        Button selectedButton = buttons[index];

        // Prevent re flipping the same card or already matched card
        if (flippedIndexes.Contains(index) || !selectedButton.interactable)
            return;

        // Flip immediately and show sprite
        flippedIndexes.Add(index);
        StartCoroutine(FlipCard(selectedButton, true));
        selectedButton.image.sprite = gamePuzzles[index];

        // Only check match when exactly 2 unmatched cards are flipped
        if (flippedIndexes.Count % 2 == 0)
        {
            int count = flippedIndexes.Count;
            int i1 = flippedIndexes[count - 2];
            int i2 = flippedIndexes[count - 1];

            countGuesses++;
            StartCoroutine(CheckMatch(i1, i2));
        }
    }

    // Animate the card Flip(rotate on Y axis 180 degrees or hide)
    IEnumerator FlipCard(Button button, bool show)
    {
        float duration = 0.3f;
        float t = 0f;
        RectTransform rect = button.GetComponent<RectTransform>();
        Quaternion start = rect.rotation;
        Quaternion end = show ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);

        while (t < duration)
        {
            t += Time.deltaTime;
            rect.rotation = Quaternion.Lerp(start, end, t / duration);
            yield return null;
        }

        rect.rotation = end;
    }

        //If match: disable and hide cards.
        // If not match: flip back and remove from flipped list.
        IEnumerator CheckMatch(int i1, int i2)
        {
        yield return new WaitForSeconds(0.5f); // delay to show second card

        string name1 = gamePuzzles[i1].name;
        string name2 = gamePuzzles[i2].name;

        if (name1 == name2)
        {
            // Matched
            yield return new WaitForSeconds(0.3f);
            buttons[i1].interactable = false;
            buttons[i2].interactable = false;

            buttons[i1].image.color = new Color(0, 0, 0, 0);
            buttons[i2].image.color = new Color(0, 0, 0, 0);

            countCorrectGuesses++;
            CheckIfGameIsFinished();
        }
        else
        {
            // Not matched – flip back and reset
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(FlipCard(buttons[i1], false));
            StartCoroutine(FlipCard(buttons[i2], false));
            buttons[i1].image.sprite = bgSprite;
            buttons[i2].image.sprite = bgSprite;

            // Remove from flippedIndexes
            flippedIndexes.Remove(i1);
            flippedIndexes.Remove(i2);
        }
    }

    // Display win message when all pairs are matched.
    void CheckIfGameIsFinished()
    {
        if (countCorrectGuesses == gameGuesses)
        {
            Debug.Log("Game Finished in " + countGuesses + " guesses");
        }
    }

    // Reset game with a new layout of cards (called after dropdown value)
    public void SetupNewGame(List<Button> newButtons, int totalCards)
    {
        buttons = newButtons;
        gamePuzzles.Clear();
        AddGamePuzzles(totalCards);
        Shuffle(gamePuzzles);
        AddListeners();

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].image.sprite = bgSprite;
            buttons[i].image.color = Color.white;
            buttons[i].interactable = true;
            buttons[i].name = i.ToString();
        }

        flippedIndexes.Clear();
        countGuesses = 0;
        countCorrectGuesses = 0;
        gameGuesses = totalCards / 2;
    }

    // Add paired puzzle sprites into the list (to match).
    void AddGamePuzzles(int totalCards)
    {
        int index = 0;
        for (int i = 0; i < totalCards; i++)
        {
            if (index == totalCards / 2) index = 0;
            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    // Randomize order of the cards before assigning to buttons.
    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(0, list.Count);
            Sprite temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}
