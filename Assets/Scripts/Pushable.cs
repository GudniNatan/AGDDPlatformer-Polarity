using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGDDPlatformer;

[RequireComponent(typeof(AudioSource))]
public class Pushable : MonoBehaviour
{
    private Vector3 initialPos;
    private GameManager gameManager;

    private Rigidbody2D rigidbody;

    private KinematicObject kinematicObject;

    private AudioSource source;

    float carryOver = 1f;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        gameManager = GameManager.instance;
        rigidbody = GetComponent<Rigidbody2D>();
        kinematicObject = GetComponent<KinematicObject>();
        source = GetComponent<AudioSource>();
        source.pitch = Random.Range(0.8f, 1.2f);
    }

    void Reset()
    {
        transform.position = initialPos;
    }

    void Update()
    {
        kinematicObject.isFrozen = gameManager.timeStopped;
        if (kinematicObject.isGrounded)
        {
            source.volume = kinematicObject.velocity.magnitude;
        }
    }
}
