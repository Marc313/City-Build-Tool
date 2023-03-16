using UnityEngine;

public class CamNavigation : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float zoomSpeed;

    [SerializeField] private float minDistanceToGround;
    [SerializeField] private float maxDistanceToGround;

    private Vector3 mouseGroundPos;
    private Builder builder;

    private void Start()
    {
        builder = FindObjectOfType<Builder>();
    }

    private void LateUpdate()
    {
        if (UIManager.Instance.isMenuOpen) return;

        /*  if (Input.GetKeyDown(KeyCode.Mouse1))
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

        // X and Z Movement
        float vert = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        float mouseScrollValue = Input.GetAxis("Mouse ScrollWheel");

        Vector3 direction = vert * Vector3.forward + hor * Vector3.right;
        Vector3 movement = direction * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        Vector3 zoomDirection = mouseScrollValue * transform.forward;
        Vector3 zoomMovement = zoomDirection * zoomSpeed * Time.deltaTime;
        Vector3 newPos = transform.position + zoomMovement;
        if (newPos.y > minDistanceToGround 
            && newPos.y < maxDistanceToGround)
        {
            transform.position = newPos;
        }
            // TODO: convert min/max DistanceToGround to actually be the distance to the ground, not just y-pos
    }
}
