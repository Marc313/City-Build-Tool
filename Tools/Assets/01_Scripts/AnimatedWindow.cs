using System.Threading.Tasks;
using UnityEngine;

public class AnimatedWindow : MovingObject
{
    public bool isDown;
    public float animationDuration = 0.5f;

    private Vector3 upPos;
    private Vector3 downPos;

    private Task currentTask;

    private void Start()
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
        isDown = false;
        currentTask = MoveToInSeconds(downPos, upPos, animationDuration);
    }

    public void MoveDown()
    {
        isDown = true;
        currentTask = MoveToInSeconds(upPos, downPos, animationDuration);
    }
}
