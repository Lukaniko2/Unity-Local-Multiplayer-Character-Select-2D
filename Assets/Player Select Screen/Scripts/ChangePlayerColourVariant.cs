using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerColourVariant : MonoBehaviour
{
    //Sets the default colour to blue
    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(0.1921569f, 0.5333334f, 1f, 1f);
        Destroy(this);
    }

}
