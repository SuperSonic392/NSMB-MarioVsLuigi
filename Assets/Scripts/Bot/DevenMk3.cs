using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevenMk3 : BotController
{
    public enum Deven3State
    {
        Casual,
        Rundown,
        Defending,
        Empathic, //sharing powerups
        Flee,
        Teabag, //spam crouch
        Friendly_Approach, //walk up to a player
        Friendly_Mimic, //copy their actions with a delay
        Friendly_Sad, //crouch depressedly after being denied. 
    }

    public enum Deven3Emotion
    {
        Neutral,
        Mad,
        Panic,
        Sad,
        Friendly,
    }

    public enum Deven3Personality
    {
        Casual,
        Competitive, 
        Reserved, 
        Greedy,
        Friendly,
    }
    public Deven3State state;
    private Deven3State lastState;
    public Deven3Emotion emotion;
    public Deven3Personality personality;

    public float stateTimer;
    public float riskReward; // -1 = full risk; 1 = full reward. 

    private void Start()
    {
        int max = System.Enum.GetValues(typeof(Deven3Personality)).Length;
        personality = (Deven3Personality)Random.Range(0, max);
        if (Random.Range(0, 100) == 3) //1 in 100 chance. 
        {
            personality = Deven3Personality.Friendly;
        }
    }

    public override void Tick()
    {
        stateTimer += Time.fixedDeltaTime;

        switch (state)
        {
            case Deven3State.Casual:
                Deven3TickCasualState();
                break;
            default:
                SetState(lastState);
                break;
        }
    }

    public void OnStateChanged()
    {
        switch (state)
        {
            case Deven3State.Casual:
                Deven3StartCasualState();
                break;
        }
    }

    public void SetState(Deven3State newState)
    {
        if (state == newState) return;

        Debug.Log("[DevenMk3] " + player.Nickname + " has changed state to " + newState);
        lastState = state;
        state = newState;
        stateTimer = 0;
        OnStateChanged();
    }

    public void Deven3StartCasualState()
    {

    }

    public void Deven3TickCasualState()
    {

    }
}
