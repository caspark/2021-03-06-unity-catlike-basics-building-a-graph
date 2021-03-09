using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    enum TransitionMode
    {
        Cycle,
        Random
    }

    [SerializeField] private Transform pointPrefab;
    [SerializeField, Range(10, 200)] private int resolution = 10;

    [SerializeField] private FunctionLibrary.FunctionName function;

    [SerializeField, Min(0f)] private float functionDuration = 1f, transitionDuration = 1f;

    [SerializeField] private TransitionMode transitionMode = TransitionMode.Cycle;


    private Transform[] points;

    private float duration;

    private bool transitioning;
    private FunctionLibrary.FunctionName transitionFunction;

    private void Awake()
    {
        points = new Transform[resolution * resolution];
        var position = Vector3.zero;
        float step = 2f / resolution;
        var scale = Vector3.one * step;
        for (int i = 00; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab, transform, false);
            point.localScale = scale;
            points[i] = point;
        }
    }

    private void Update()
    {
        duration += Time.deltaTime;
        if (transitioning)
        {
            if (duration >= transitionDuration)
            {
                duration -= transitionDuration;
                transitioning = false;
            }
        }
        else if (duration >= functionDuration)
        {
            duration -= functionDuration;
            transitioning = true;
            transitionFunction = function;
            PickNextFunction();
        }

        if (transitioning)
        {
            UpdateFunctionTransition();
        }
        else
        {
            UpdateFunction();
        }
    }

    private void PickNextFunction()
    {
        function = transitionMode == TransitionMode.Cycle
            ? FunctionLibrary.GetNextFunctionName(function)
            : FunctionLibrary.GetRandomFunctionNameOtherThan(function);
    }

    private void UpdateFunction()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }

            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = f(u, v, time);
            ;
        }
    }

    private void UpdateFunctionTransition()
    {
        FunctionLibrary.Function from = FunctionLibrary.GetFunction(transitionFunction);
        FunctionLibrary.Function to = FunctionLibrary.GetFunction(function);
        float progress = duration / transitionDuration;
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }

            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = FunctionLibrary.Morph(u, v, time, from, to, progress);
            ;
        }
    }
}