using UnityEngine;

/// <summary>
/// Used as an actual timer or can be used as a counter
/// </summary>
[System.Serializable]
public struct Timer
{
    [SerializeField]
    float delay;
    float time;

    /// <summary>
    /// How long the timer is
    /// </summary>
    public float Delay { get => delay; }
    /// <summary>
    /// The current time value of the Timer
    /// </summary>
    public float Time { get => time; }

    /// <summary>
    /// The percent of time left in the timer
    /// </summary>
    public float PercentComplete
    {
        get => time / delay;
    }
    /// <summary>
    /// how much time is left in the timer
    /// </summary>
    public float TimeRemaining
    {
        get => delay - time;
    }

    /// <summary>
    /// Creates a timer where Time is set to 0 and the length is delay
    /// </summary>
    /// <param name="delay">how long the timer will be</param>
    public Timer(float delay)
    {
        this.time = 0;
        this.delay = delay;
    }


    /// <summary>
    /// Reset the timer to the startPoint
    /// </summary>
    /// <param name="startPoint">The time in seconds you want the timer to get set to</param>
    public void Reset(float startPoint = 0)
    {
        time = startPoint;
    }

    /// <summary>
    /// Increases timer by Time.deltaTime
    /// </summary>
    public void CountByTime()
    {
        time += UnityEngine.Time.deltaTime;
        time = Mathf.Max(time, 0);
        time = Mathf.Min(time, delay);
    }

    /// <summary>
    /// Increases timer by value
    /// </summary>
    /// <param name="value">The float value you want to add to timer</param>
    public void CountByValue(float value)
    {
        time += value;
        time = Mathf.Max(time, 0);
        time = Mathf.Min(time, delay);
    }

    /// <summary>
    /// Checks to see if the timer has reached or passed the delay
    /// </summary>
    /// <param name="resetOnTrue">Whether you want the timer to reset when IsComplete() is true</param>
    /// <returns>Returns true if timer is greater than or equal to delay</returns>
    public bool IsComplete(bool resetOnTrue = true)
    {
        if (time >= delay)
        {
            if (resetOnTrue)
                Reset();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether the timer has reached or passed the delay and if not count up
    /// </summary>
    /// <param name="resetOnTrue">Whether you want the timer to reset when it returns true</param>
    /// <returns>Returns true if timer is greater than or equal to delay</returns>
    public bool Check(bool resetOnTrue = true)
    {
        if (IsComplete(resetOnTrue))
            return true;

        CountByTime();

        return false;
    }

    /// <summary>
    /// Checks whether the timer has reached or passed the delay and if not count up by value
    /// </summary>
    /// <param name="value">value to count up by</param>
    /// <param name="resetOnTrue">Whether you want the timer to reset when it returns true</param>
    /// <returns>Returns true if timer is greater than or equal to delay</returns>
    public bool Check(float value, bool resetOnTrue = true)
    {
        if (IsComplete(resetOnTrue))
            return true;

        CountByValue(value);

        return false;
    }
}
