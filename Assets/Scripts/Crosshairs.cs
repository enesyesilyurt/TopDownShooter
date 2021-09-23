using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshairs : MonoBehaviour
{
    public LayerMask targetMask;
    public SpriteRenderer dot;
    public Color dotHighLightColour;
    Color originalColour;

    private void Start()
    {
        Cursor.visible = false;
        originalColour = dot.color;
    }
    void Update()
    {
        transform.Rotate(Vector3.forward * -40 * Time.deltaTime);
    }
    public void DetectTargets(Ray ray)
    {
        if (Physics.Raycast(ray, 100, targetMask))
        {
            dot.color = dotHighLightColour;
        }
        else
        {
            dot.color = originalColour;
        }
    }
}
