using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMaterial : MonoBehaviour
{
    public Material defaultMaterial;
    public Material hitMaterial;
    public GameObject shield;
    public float changeTime = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        shield.GetComponent<Renderer>().material = defaultMaterial;
    }

    public IEnumerator changeMaterial()
    {
        shield.GetComponent<Renderer>().material = hitMaterial;
        yield return new WaitForSeconds(changeTime);
        shield.GetComponent<Renderer>().material = defaultMaterial;
        yield break;
    }
}
