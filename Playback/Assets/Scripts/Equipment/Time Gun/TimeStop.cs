using UnityEngine;

public interface TimeStop
{
    public bool stopped { get; set; }

    public void StopTime();
    public void StartTime();
}