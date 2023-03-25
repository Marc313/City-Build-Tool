using System.Threading.Tasks;
using UnityEngine;

public class AnimatedWindow : MovingObject
{
    public bool isDown;
    public bool isMoving;
    public float animationDuration = 0.5f;

    private Vector3 upPos;
    private Vector3 downPos;

    private Task currentTask;

    private void OnEnable()
    {
        upPos = transform.position;
        downPos = transform.position;
        downPos.y = -720 + 540;
    }

    private async void Update()
    {
        if (currentTask != null && !currentTask.IsCompleted) await currentTask;

        if (!isDown && Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
        }

        if (isDown && Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }
    }

    public void MoveUp()
    {
        if (!isDown || isMoving) return;
        isDown = false;
        isMoving = true;
        currentTask = MoveToInSeconds(downPos, upPos, animationDuration, () => isMoving = false);
    }

    public void MoveDown()
    {
        if (isDown || isMoving) return;

        isDown = true;
        isMoving = true;
        currentTask = MoveToInSeconds(upPos, downPos, animationDuration, () => isMoving = false);
    }
}
