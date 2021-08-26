/*
MIT License

Copyright (c) 2019 Sebastian Lague

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    BoidSettings settings;

    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;

    Vector3 velocity;
    Vector3 acceleration;

    [HideInInspector]
    public int numFlockmates;
    [HideInInspector]
    public Vector3 flockHeading;
    [HideInInspector]
    public Vector3 centreOfFlock;
    [HideInInspector]
    public Vector3 avoidanceHeading;

    Vector3[] rayDirections;

    Transform target;
    bool goToTarget = true;

    void Update()
    {
        // Simple logic to go to target or not
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (goToTarget == true)
            {
                goToTarget = false;
            }
            else if (goToTarget == false)
            {
                goToTarget = true;
            }
        }
    }

    // Sets up boid
    public void Initilise(BoidSettings settings, Transform target)
    {
        this.settings = settings;
        this.target = target;

        // Used for checking which direction to go if collision detacted
        rayDirections = new Vector3[] { transform.up, -transform.up, transform.right, -transform.right, transform.forward, -transform.forward };

        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        velocity = transform.forward * startSpeed;
    }

    public void UpdateBoid()
    {
        acceleration = Vector3.zero;

        // Add target steering to acceleration
        if (target != null && goToTarget == true)
        {
            Vector3 targetOffset = (target.position - position);
            acceleration += Steer(targetOffset) * settings.targetWeight;
        }

        // Add alignment, cohesion, and seperation steering to acceleration
        if (numFlockmates != 0)
        {
            centreOfFlock /= numFlockmates;

            Vector3 flockCentreOffset = (centreOfFlock - position);

            Vector3 alignment = Steer(flockHeading) * settings.alignWeight;
            Vector3 cohesion = Steer(flockCentreOffset) * settings.cohesionWeight;
            Vector3 seperation = Steer(avoidanceHeading) * settings.seperationWeight;

            acceleration += alignment;
            acceleration += cohesion;
            acceleration += seperation;
        }

        // Add collision steering to acceleration
        if (CollisionCheck())
        {
            Vector3 collisionAvoidDir = ObstacleRays();
            Vector3 collisionAvoidForce = Steer(collisionAvoidDir) * settings.avoidCollisionWeight;
            acceleration += collisionAvoidForce;
        }

        // Some last cals before updating boid
        velocity += acceleration * Time.deltaTime;
        float speed = velocity.magnitude;
        Vector3 dir = velocity / speed;
        speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
        velocity = dir * speed;

        // Updates boid position and forward 
        transform.position += velocity * Time.deltaTime;
        transform.forward = Vector3.Slerp(transform.forward, dir, settings.slerpSpeed * Time.deltaTime);
        position = transform.position;
        forward = dir;
    }

    bool CollisionCheck()
    {
        // Collision detection
        RaycastHit hit;
        if (Physics.SphereCast(position, settings.boundsRadius, forward, out hit, settings.collisionAvoidDst, settings.obstacleMask))
        {
            return true;
        }
        else { }
        return false;
    }

    Vector3 ObstacleRays()
    {
        // Checks which way a boid should go if collision detected
        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = transform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(position, dir);
            if (!Physics.SphereCast(ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask))
            {
                return dir;
            }
        }

        return forward;
    }

    Vector3 Steer(Vector3 vector)
    {
        // Implement Reynolds: Steering = Desired - Velocity.
        Vector3 v = vector.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, settings.maxSteerForce);
    }
}
