// Assets/Scripts/Core/TurnRightCommand.cs
using UnityEngine;
using System.Collections;

public class TurnRightCommand : ICommand
{
    private readonly float angle;
    private readonly float duration;

    public TurnRightCommand(float angle = 90f, float duration = 0.3f)
    {
        this.angle    = angle;
        this.duration = Mathf.Max(0.01f, duration);
    }

    public IEnumerator Execute(Transform actor)
    {
        Quaternion startRot = actor.rotation;
        Quaternion endRot   = startRot * Quaternion.Euler(0f, angle, 0f);
        float      elapsed  = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            actor.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        actor.rotation = endRot;
    }
}
