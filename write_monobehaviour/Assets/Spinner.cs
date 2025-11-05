using System;                     
using System.Collections.Generic;  
using UnityEngine;

//Spinner

public class Spinner : MonoBehaviour
{
    // The first feature I used is Statics (very simple)
    // This will track how many Spinner objects exist in the scene.
    // It's useful for debugging and demonstrating static shared state.
    public static int SpinnerCount = 0;

    // Second feature is Attributes.
    // [Range] exposes a slider in the Inspector -> 10 to 360 degrees
    // [SerializeField] keeps the field private but still visible.
    [Range(10f, 360f)]
    [SerializeField]
    private float rotationSpeed = 90f;

    // Third feature is Properties; here, property reacts when the speed changes (logs + triggers event)
    public float CurrentSpeed
    {
        get => rotationSpeed;
        set
        {
            rotationSpeed = value;
            Debug.Log($"[Spinner] Speed changed to {rotationSpeed:F1} deg/sec");
            OnSpeedChanged?.Invoke(rotationSpeed);   // event is triggered here!
        }
    }

    // Fourth feature is generics; List<float> stores all previously recorded speed values.
    private readonly List<float> speedHistory = new List<float>();

    // Fifth feature is Event(s)
    public event Action<float> OnSpeedChanged;

    private void Awake()
    {
        SpinnerCount++;
        Debug.Log($"[Spinner] Spinner objects in scene: {SpinnerCount}");

        // Subscribe an internal handler to track speed changes.
        OnSpeedChanged += HandleSpeedChanged;
    }

    private void OnDestroy()
    {
        SpinnerCount--;
        OnSpeedChanged -= HandleSpeedChanged;
    }

    // Logs the speed history whenever the event fires.
    private void HandleSpeedChanged(float newSpeed)
    {
        speedHistory.Add(newSpeed);
    }

    // Finally, this is out parameter(s); It returns the average speed using an out parameter.
    private bool TryGetAverageSpeed(out float avg)
    {
        if (speedHistory.Count == 0)
        {
            avg = 0f;
            return false;
        }

        float sum = 0f;
        for (int i = 0; i < speedHistory.Count; i++)
            sum += speedHistory[i];

        avg = sum / speedHistory.Count;
        return true;
    }

    // Now it will run the game object in real time
    private void Update()
    {
        // Spins the GameObject every frame. 
        // If I want to increase/decrease the speed, I can change the degrees.
        transform.Rotate(0f, 300f * Time.deltaTime, 0f);

        // Animate the speed smoothly between 45 and 180 degrees/second.
        float animatedSpeed = Mathf.Lerp(45f, 180f, Mathf.PingPong(Time.time * 0.25f, 1f));

        if (!Mathf.Approximately(animatedSpeed, rotationSpeed))
        {
            CurrentSpeed = animatedSpeed;  // Uses property, then event happens, then logs
        }

        // Every 120 frames (~2 seconds), it will print average speed.
        if (Time.frameCount % 120 == 0)
        {
            if (TryGetAverageSpeed(out float average))
                Debug.Log($"[Spinner] Average speed so far: {average:F1} deg/sec");
        }
    }
}