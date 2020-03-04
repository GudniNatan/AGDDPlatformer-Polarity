using System;
using UnityEngine;

namespace AGDDPlatformer
{
    public class KinematicObject : MonoBehaviour
    {
        [Header("Settings")]
        public float minGroundNormalY = 0.65f;
        public float gravityModifier = 1;

        [Header("Info")]
        public Vector2 velocity;
        public bool isGrounded;

        protected Vector2 groundNormal;
        protected Rigidbody2D body;
        protected ContactFilter2D contactFilter;
        protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

        protected const float minMoveDistance = 0.001f;
        protected const float shellRadius = 0.01f;

        public bool isFrozen;

        private bool wasGrounded;

        protected bool pushing;

        protected GameObject on; //if grounded, then this is the gameObject this is on top of

        private Transform originalParent;

        protected void OnEnable()
        {
            body = GetComponent<Rigidbody2D>();
            body.isKinematic = true;
        }

        protected void OnDisable()
        {
            body.isKinematic = false;
        }

        protected void Start()
        {
            contactFilter.useTriggers = false;
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            contactFilter.useLayerMask = true;
            originalParent = transform.parent;
        }

        protected void FixedUpdate()
        {
            if (isFrozen)
                return;

            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;

            isGrounded = false;
            on = null;

            Vector2 deltaPosition = velocity * Time.deltaTime;
            Vector2 groundVector = new Vector2(groundNormal.y, -groundNormal.x);
            Vector2 groundMove = groundVector * deltaPosition.x;
            Vector2 xmove = PerformMovement(groundMove, false);

            Vector2 airMove = Vector2.up * deltaPosition.y;
            Vector2 ymove = PerformMovement(airMove, true);
            body.MovePosition(body.position + xmove + ymove);

            wasGrounded = isGrounded;
            if (isGrounded)
            {
                if (body.sharedMaterial != null)  //do friction
                    velocity /= 1 + (body.sharedMaterial.friction * Time.deltaTime);

                // if on top of other kinematic object, and that object is moving then share velocity
                var kinOn = on.GetComponent<KinematicObject>();
                if (kinOn != null)
                {
                    if (kinOn.tag != "Player1" && kinOn.tag != "Player2" && tag != "Player1" && tag != "Player2")
                    {
                        // Vector2 move = PerformMovement(kinOn.velocity * Time.deltaTime, false);
                        if (transform.parent != on.transform)
                        {
                            transform.parent = on.transform;
                        }
                    }
                } else {
                    transform.parent = originalParent;
                }
            }
            else
            {
                //do air friction
                float y = velocity.y;
                if (body.sharedMaterial != null)
                    velocity /= 1 + (body.sharedMaterial.friction * Time.deltaTime * 0.5f);
                velocity.y = y;
            }

        }

        Vector2 PerformMovement(Vector2 move, bool yMovement)
        {
            float distance = move.magnitude;

            if (distance > minMoveDistance)
            {
                //check if we hit anything in current direction of travel
                int count = body.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
                for (int i = 0; i < count; i++)
                {
                    Vector2 currentNormal = hitBuffer[i].normal;

                    if (wasGrounded &&
                        !yMovement &&
                        hitBuffer[i].transform.tag == "Pushable" &&
                        velocity.y * Math.Sign(gravityModifier) <= 0)
                    {
                        //Push other object
                        KinematicObject other = hitBuffer[i].transform.GetComponent<KinematicObject>();
                        other.velocity += velocity * Math.Sign(gravityModifier);
                        velocity /= 2;
                        return new Vector2(0, 0);
                    }

                    //is this surface flat enough to land on?
                    if ((gravityModifier >= 0 && currentNormal.y > minGroundNormalY) ||
                        (gravityModifier < 0 && currentNormal.y < -minGroundNormalY))
                    {
                        isGrounded = true;
                        on = hitBuffer[i].transform.gameObject;
                        // if moving up, change the groundNormal to new surface normal.
                        if (yMovement)
                        {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }

                    if (isGrounded)
                    {
                        //how much of our velocity aligns with surface normal?
                        var projection = Vector2.Dot(velocity, currentNormal);

                        if (projection < 0)
                        {
                            //slower velocity if moving against the normal (up a hill).
                            velocity -= projection * currentNormal;
                        }
                    }
                    else
                    {
                        //We are airborne, but hit something, so cancel vertical up and horizontal velocity.
                        if (gravityModifier >= 0 && currentNormal.y < -0.01f)
                        {
                            velocity.y = Mathf.Min(velocity.y, 0);
                        }

                        else if (gravityModifier < 0 && currentNormal.y > 0.01f)
                        {
                            velocity.y = Mathf.Max(velocity.y, 0);
                        }

                        if (Mathf.Sign(currentNormal.x) != Mathf.Sign(velocity.x))
                        {
                            velocity.x = 0;
                        }
                    }

                    //remove shellDistance from actual move distance.
                    var modifiedDistance = hitBuffer[i].distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
            // Vector2 pos = body.position + move.normalized * distance;
            //body.MovePosition(pos);
            // body.position = pos;
            // body.position += move.normalized * distance;
            return move.normalized * distance;
        }
    }
}
