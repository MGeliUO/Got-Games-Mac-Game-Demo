using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackdrop : MonoBehaviour
{
    public float scrollSpeed = 5f;

    public float startXPos, resetThreshold;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime * GameManager.instance.scrollSpeedMultiplier);

        // Reset to the start position if we pass the rest threshold
        if (transform.position.x < resetThreshold)
        {
            transform.position = new Vector3(startXPos, transform.position.y, transform.position.z);
        }
    }
}
