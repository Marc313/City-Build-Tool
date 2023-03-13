using UnityEngine;

public class CamNavigation : MonoBehaviour
{
    Vector3 mouseGroundPos;
    private Builder builder;
    [SerializeField] private float speed;

    private void Start()
    {
        builder = FindObjectOfType<Builder>();
    }

    private void LateUpdate()
    {
        /*        if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    mouseGroundPos = builder.mouseHitPos;
                }
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    Vector3 mouseDelta = mouseGroundPos - builder.mouseHitPos;
                    mouseGroundPos = builder.mouseHitPos;
                    Vector3 movement = new Vector3(mouseDelta.x, mouseDelta.y);
                    transform.Translate(movement);
                }*/


        float vert = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");

        Vector3 direction = vert * Vector3.forward + hor * Vector3.up;
        Vector3 movement = direction * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
