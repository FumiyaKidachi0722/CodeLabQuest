// Assets/Scripts/Core/ForwardCommand.cs
using UnityEngine;
using System.Collections;

public class ForwardCommand : ICommand
{
    private readonly float distance;
    private readonly float duration;

    public ForwardCommand(float distance = 1f, float duration = 0.5f)
    {
        this.distance = distance;
        this.duration = Mathf.Max(0.01f, duration);
    }

    public IEnumerator Execute(Transform actor)
    {
        Vector3 startPos = actor.position;
        Vector3 endPos   = startPos + actor.forward * distance;
        float   elapsed  = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            actor.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        actor.position = endPos;
    }
}
