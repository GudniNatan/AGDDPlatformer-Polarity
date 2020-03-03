using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGDDPlatformer;

public class Pushable : MonoBehaviour
{
    private Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
    }

    void Reset() {
        transform.position = initialPos;
    }
}
