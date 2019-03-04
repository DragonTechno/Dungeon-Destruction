using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class postCost : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshPro>().text = GetComponentInParent<placeRequirements>().cost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
