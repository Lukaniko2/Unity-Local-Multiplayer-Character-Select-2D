using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="PlayerSelectEventHandler", menuName = "ScriptableObjects/Player Select Events")]
public class SO_PlayerSelectEvents : ScriptableObject
{
    //Pause Game Event
    public UnityEvent pauseGameEvent = new UnityEvent();

    public void PauseGameEventSend()
    {
        pauseGameEvent.Invoke();
    }

    //Pause Game Event
    public UnityEvent resumeGameEvent = new UnityEvent();

    public void ResumeGameEventSend()
    {
        resumeGameEvent.Invoke();
    }

}
