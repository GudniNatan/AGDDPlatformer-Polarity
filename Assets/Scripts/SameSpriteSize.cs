using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameSpriteSize : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private SpriteRenderer emulating;
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.size = emulating.size;
    }
}
