using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FrameRateCounter : MonoBehaviour
{
    enum DisplayMode
    {
        FPS, MS
    }
    
    [SerializeField] private TextMeshProUGUI display;

    [SerializeField, Range(0.1f, 2f)] private float sampleDuration = 1f;

    [SerializeField] private DisplayMode displayMode = DisplayMode.FPS;

    private int frames;
    private float duration, bestDuration = float.MaxValue, worstDuration;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        frames += 1;
        duration += frameDuration;

        if (frameDuration < bestDuration)
        {
            bestDuration = frameDuration;
        }
        if (frameDuration > worstDuration)
        {
            worstDuration = frameDuration;
        }

        if (duration >= sampleDuration)
        {
            if (displayMode == DisplayMode.FPS)
            {
                display.SetText("FPS\n{0:0}\n{1:0}\n{2:0}",
                    1f / bestDuration,
                    frames / duration,
                    1f / worstDuration);
            }
            else
            {
                display.SetText("FPS\n{0:1}\n{1:1}\n{2:1}",
                    1000f * bestDuration,
                    1000f * (duration / frames),
                    1000f * worstDuration);
            }

            frames = 0;
            duration = 0f;
            bestDuration = float.MaxValue;
            worstDuration = 0f;
        }
    }
}