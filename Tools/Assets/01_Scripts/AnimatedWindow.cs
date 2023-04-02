using UnityEngine;
using UnityEngine.Events;

public class AnimatedWindow : MovingObject
{
    public float animationDuration = 0.5f;
    public Transform destination;
    public UnityEvent onAnimation;

    private Vector3 startPos;
    private Vector3 destinationPos;
    private bool isShown = true;
    private bool isMoving;

    private void OnEnable()
    {
        startPos = transform.position;
        destinationPos = destination.position;
        Hide();
    }

    public void Hide()
    {
        if (!isShown || isMoving) return;
        isShown = false;
        isMoving = true;
        onAnimation?.Invoke();
        MoveToInSeconds(startPos, destinationPos, animationDuration, () => isMoving = false);
    }

    public void Show()
    {
        if (isShown || isMoving) return;

        isShown = true;
        isMoving = true;
        onAnimation?.Invoke();
        MoveToInSeconds(destinationPos, startPos, animationDuration, () => isMoving = false);
    }
}
