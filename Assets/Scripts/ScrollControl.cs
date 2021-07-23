using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollControl : MonoBehaviour
{
    public RectTransform contentSize;
    public ScrollRect scrollRect;

    RectTransform rect;
    float previousHeight;

    private void Awake() {
        rect = GetComponent<RectTransform>();
    }

    void Update() {
        if (previousHeight != contentSize.sizeDelta.y) {
            previousHeight = contentSize.sizeDelta.y;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, contentSize.sizeDelta.y + 10);
            scrollRect.verticalNormalizedPosition = 0.0f;
        }
    }
}
