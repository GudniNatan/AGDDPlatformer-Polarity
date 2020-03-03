using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGDDPlatformer;
using System.Linq;

public class Spikes : MonoBehaviour
{
    public string[] playerTags;

    private List<GameObject> players;

    private bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        if (playerTags == null || playerTags.Length == 0) {
            playerTags = new string[2] {"Player1", "Player2"};
        }
        if (players == null) {
            players = new List<GameObject>();
        }
        foreach (var tag in playerTags) {
            players.Add(GameObject.FindWithTag(tag));
        }
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col)
    {
       if (playerTags.Contains(col.tag) && active) {
           PlayerController player = col.GetComponent<PlayerController>();
           player.source.PlayOneShot(player.deathSound);
           StartCoroutine(Commons.DelayedAction(GameManager.instance.ResetLevel, 0.2f));
           GameManager.instance.timeStopped = true;
           active = false;
           StartCoroutine(Commons.DelayedAction(() => {active = true;}, 0.2f));
           StartCoroutine(Blink(player));
       }
    }


    IEnumerator Blink(PlayerController player) {
        SpriteRenderer p = player.GetComponentInChildren<SpriteRenderer>();
        for (int i = 0; i < 40; i++)
        {
            yield return new WaitForSeconds(1f/50f);
            p.enabled = !p.enabled;
        }
        p.enabled = true;
    }
}
