using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ToggleBox : MonoBehaviour
{
    public enum BoxColor
    {
        BLUE, RED
    }
    public BoxColor boxColor;
    public bool intiallyEnabled;

    private SpriteRenderer subBox;
    private SpriteRenderer outline;
    // Start is called before the first frame update
    void Start()
    {
        subBox = transform.Find("Box").GetComponent<SpriteRenderer>();
        subBox.gameObject.SetActive(intiallyEnabled);
        outline = GetComponent<SpriteRenderer>();
        Color color = Commons.ColorFromHex("007DFF");
        if (boxColor == BoxColor.RED)
        {
            color = Commons.ColorFromHex("FF004B");
        }
        subBox.color = color;
        outline.color = color;
    }

    void Reset() {
        Start();
    }

    void HighFive()
    {
        subBox.gameObject.SetActive(!subBox.gameObject.activeSelf);
    }
}
