using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UltEvents;

public class AudioEventTimer : MonoBehaviour
{
    public AudioSource source;

    //Treated as a stack.
    public List<TimedEvent> events;
    public int untilNext;


    void Start()
    {
        TimedEvent last = null;
        foreach (TimedEvent ev in events) {
            //        \\       //        \\   <-- support beams
            if (last != null && ev.time <= last.time) {
                Debug.LogWarning("Invalid timed event ordering.");
            }
            last = ev;
        }
    }


    void Update()
    {
        if (events.Count > 0) {

            if (source.timeSamples >= events[0].time) {

                if (events.Count > 1) {
                    untilNext = events[1].time - source.timeSamples;
                }

                TimedEvent next = events[0];
                events.RemoveAt(0);
                next.trigger.Invoke();
            }
            else {
                untilNext = events[0].time - source.timeSamples;
            }
        }
        else {
            this.enabled = false;
        }
    }

    public void AddEventFront(int time, UltEvent trigger) {
        events.Insert(0, new TimedEvent { time = time, trigger = trigger });
        this.enabled = true;
    }

    public void AddEventBack(int time, UltEvent trigger) {
        events.Add(new TimedEvent { time = time, trigger = trigger });
        this.enabled = true;
    }
}

[Serializable]
public class TimedEvent
{
    public int time;
    public UltEvent trigger;
}
