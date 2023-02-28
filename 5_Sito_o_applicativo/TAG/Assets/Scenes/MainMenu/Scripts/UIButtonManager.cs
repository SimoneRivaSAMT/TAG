using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButtonManager : MonoBehaviour
{
    [Header("Settings")]
    private Image image;
    [SerializeField] private Color hoverColor;
    [SerializeField] private float sizeDeltaX;

    private bool isExpanding = false;
    private bool isShrinking = false;
    
    private void Start()
    {
        image = GetComponent<Image>();
        image.color = hoverColor;
        image.rectTransform.sizeDelta = new Vector2(0, 10);
    }

    public void MouseOver()
    {
        if (isShrinking)
        {
            isShrinking = false;
        }
            
        StartCoroutine(ExpandImage());
    }

    public void MouseNotOver()
    {
        if (isExpanding)
        {
            isExpanding = false;
        }
        StartCoroutine(ShrinkImage());
    }

    private IEnumerator ExpandImage()
    {
        isExpanding = true;
        float currentDeltaX = image.rectTransform.sizeDelta.x;
        while (currentDeltaX < sizeDeltaX && !isShrinking)
        {
            currentDeltaX = image.rectTransform.sizeDelta.x;
            float tmp = currentDeltaX + 2;
            image.rectTransform.sizeDelta = new Vector2(tmp, 10);
            yield return new WaitForSeconds(.0001f);
        }
        isExpanding = false;
    }

    private IEnumerator ShrinkImage()
    {
        isShrinking = true;
        float currentDeltaX = image.rectTransform.sizeDelta.x;
        while (currentDeltaX > 0 && !isExpanding)
        {
            currentDeltaX = image.rectTransform.sizeDelta.x;
            float tmp = currentDeltaX - 2;
            image.rectTransform.sizeDelta = new Vector2(tmp, 10);
            yield return new WaitForSeconds(.0001f);
        }
        isShrinking = false;
    }
}
