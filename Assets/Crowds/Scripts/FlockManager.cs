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

public class FlockManager : MonoBehaviour
{
    public BoidSettings boidSettings;

    public Transform target;
    Boid[] boids;

    void Start()
    {
        boids = FindObjectsOfType<Boid>();
        foreach (Boid b in boids)
        {
            // Pass settings and target to boid
            b.Initilise(boidSettings, target);
        }
    }

    void Update()
    {
        if (boids != null)
        {
            int numBoids = boids.Length;
            FlockData[] FlockData = new FlockData[numBoids];

            for (int i = 0; i < numBoids; i++)
            {
                // Get initial data from boid
                FlockData[i].position = boids[i].position;
                FlockData[i].direction = boids[i].forward;
            }

            // Compute flock data
            ComputeData(numBoids, FlockData);

            for (int i = 0; i < numBoids; i++)
            {
                // Send flock data to boid
                boids[i].numFlockmates = FlockData[i].numFlockmates;
                boids[i].flockHeading = FlockData[i].flockHeading;
                boids[i].centreOfFlock = FlockData[i].flockCentre;
                boids[i].avoidanceHeading = FlockData[i].avoidanceHeading;

                boids[i].UpdateBoid();
            }
        }
    }

    void ComputeData(int numBoids, FlockData[] FlockData)
    {
        for (int i = 0; i < numBoids; i++)
        {
            for (int j = 0; j < numBoids; j++)
            {
                if (i != j)
                {
                    // Compares distances off square distances, to avoid square root cals
                    Vector3 offset = FlockData[j].position - FlockData[i].position;
                    float sqrDst = offset.x * offset.x + offset.y * offset.y + offset.z * offset.z;

                    if (sqrDst < boidSettings.percenptionRadius * boidSettings.percenptionRadius)
                    {
                        FlockData[i].numFlockmates += 1;
                        FlockData[i].flockHeading += FlockData[j].direction;
                        FlockData[i].flockCentre += FlockData[j].position;

                        if (sqrDst < boidSettings.avoidanceRadius * boidSettings.avoidanceRadius)
                        {
                            FlockData[i].avoidanceHeading -= offset / sqrDst;
                        }
                    }
                }
            }
        }
    }

    struct FlockData
    {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 flockHeading;
        public Vector3 flockCentre;
        public Vector3 avoidanceHeading;
        public int numFlockmates;
    }
}
