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

[CreateAssetMenu()]
public class BoidSettings : ScriptableObject
{
    // Boid main settings
    [Header("Main Settings")]
    public float minSpeed = 100;
    public float maxSpeed = 500;
    public float slerpSpeed = 2.5f;

    public float percenptionRadius = 250;
    public float avoidanceRadius = 100;

    public float maxSteerForce = 100;

    public float alignWeight = 1.25f;
    public float cohesionWeight = 0.75f;
    public float seperationWeight = 1.75f;

    public float targetWeight = 2;

    // Boid collision settings
    [Header("Collision Settings")]
    public LayerMask obstacleMask;
    public float boundsRadius = 45;
    public float avoidCollisionWeight = 15;
    public float collisionAvoidDst = 350;
}
