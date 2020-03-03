using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGDDPlatformer
{
    public class HighFive : MonoBehaviour
    {
        public MonoBehaviour[] receivers; //Recievers need to implement the HighFive method.
        [SerializeField] private PlayerController otherPlayer = null;

        private float distanceForHighFive = 0.8f;
        private float highFiveCooldown = 0.5f; //Cooldown for the high five in seconds
        private bool highFiveCooldownActive;

        private Collider2D otherPlayerCollider;
        private Collider2D playerCollider;


        // If the two players cross, then they should perform a high five that can send messages to other gameobjects.
        void Start()
        {
            highFiveCooldownActive = false;
            if (otherPlayer == null)
            {
                Destroy(this);
            }
            else
            {
                playerCollider = GetComponent<Collider2D>();
                otherPlayerCollider = otherPlayer.GetComponent<Collider2D>();
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!highFiveCooldownActive && otherPlayer != null)
            {
                Vector3 dist = transform.GetChild(0).position - otherPlayer.transform.GetChild(0).position;
                if (dist.sqrMagnitude < distanceForHighFive * distanceForHighFive)
                {
                    print("highFive!");
                    DoHighFive();
                    highFiveCooldownActive = true;
                    StartCoroutine(Commons.DelayedAction(() => highFiveCooldownActive = false, highFiveCooldown));
                }
            }
        }

        void DoHighFive()
        {
            PlayHighFiveSound();
            GameManager.StopTime(0.05f);
            ParticleSystem ps = GetComponent<ParticleSystem>();
            ParticleSystem.ShapeModule shape = ps.shape;
            shape.position = (Vector3) (otherPlayer.transform.position - transform.position) / 2f;
            GetComponent<ParticleSystem>().Play();

            foreach (var reciever in receivers)
            {
                reciever.Invoke("HighFive", 0.2f);
            }
        }

        void PlayHighFiveSound()
        {
            PlayerController player = GetComponent<PlayerController>();
            player.source.PlayOneShot(player.highFiveSound);
        }
    }
}
