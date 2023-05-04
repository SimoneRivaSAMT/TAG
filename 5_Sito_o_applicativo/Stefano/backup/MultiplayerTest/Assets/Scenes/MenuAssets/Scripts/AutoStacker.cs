using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoStacker : MonoBehaviour
{
    private GameObject container;
    private List<GameObject> elements;
    int actualChildren = 0;

    private void Start()
    {
        elements = new List<GameObject>();
        container = gameObject;
        UpdateGameObjectList();
        UpdateView();
    }

    public void UpdateAll()
    {
        UpdateGameObjectList();
        UpdateView();
    }

    public void UpdateGameObjectList()
    {
        RectTransform[] tmpArrayGo = container.GetComponentsInChildren<RectTransform>();
        foreach (RectTransform transform in tmpArrayGo)
        {
            if (transform.gameObject.name.Equals("LobbyTemplate(Clone)"))
            {
                elements.Add(transform.gameObject);
                Debug.Log("nome: " + transform.gameObject.name);
            }
        }
        actualChildren = elements.Count;
    }

    public void UpdateView()
    {
        GameObject first;
        GameObject second;
        
        for (int i = 0; i < elements.Count - 1; i++)
        {
            first = elements[i];
            second = elements[i + 1];
            RectTransform firstRt = first.GetComponent<RectTransform>();
            RectTransform secondRt = second.GetComponent<RectTransform>();
            secondRt.offsetMax = new Vector2(0, firstRt.offsetMax.y - firstRt.rect.height);
            secondRt.offsetMin = new Vector2(0, firstRt.offsetMin.y - firstRt.rect.height);
            Debug.Log("offset");
        }
    }

    public void RemoveAllRows()
    {
        actualChildren = 0;
        foreach (GameObject item in elements)
        {
            Destroy(item);
        }
        elements.Clear();
    }
}