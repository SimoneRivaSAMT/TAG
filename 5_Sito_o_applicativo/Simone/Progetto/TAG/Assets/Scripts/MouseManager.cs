using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MouseManager : MonoBehaviour
{
    Image bg;

    static Color W = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);

    void Awake()
    {
        bg = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
       // bg.color = W;
    }

    void OnMouseExit()
    {
        Debug.Log("Il mouse non è più su GameObject .");
    }
}
