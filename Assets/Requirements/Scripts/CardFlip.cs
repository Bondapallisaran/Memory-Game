using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlip : MonoBehaviour
{
     bool isFlipped = false;
     bool isMatched = false;

    private Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        Flip();
    }
    public void Flip()
     {
        if (isFlipped || isMatched)
            return;
        StartCoroutine(FlipAnimation());
     }

    IEnumerator FlipAnimation()
    {
        float duration = 0.4f;
        float t = 0f;

        RectTransform buttonRect = btn.GetComponent<RectTransform>();
        Quaternion startRotation = buttonRect.rotation;
        Quaternion endRotation = Quaternion.Euler(0, 180, 0);

        while(t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;
            float angle = Mathf.Lerp(0,180, progress);
            buttonRect.rotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }

        isFlipped = true;

    }

    
    
}
