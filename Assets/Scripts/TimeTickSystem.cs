using System;
using UnityEngine;

public class TimeTickSystem : MonoBehaviour
{
    public class OnTickEvents : EventArgs
    {
        public int Tick;
    }

    public static event EventHandler<OnTickEvents> OnTick;
    
    // Number of seconds per tick
    private const float MAXTick = .5f;
    private int _tick;
    private float _tickTimer;

    private void Awake()
    {
        _tick = 0;
    }

    private void Update()
    {
        _tickTimer += Time.deltaTime;
        if (!(_tickTimer >= MAXTick)) return;
        _tickTimer -= MAXTick;
        _tick++;
        if (OnTick != null)
        {
            OnTick(this, new OnTickEvents()
            {
                Tick = _tick
            });
        }
    }
}
