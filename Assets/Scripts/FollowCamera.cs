using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float altitude = 15f;
    public float speed = 2f;

    private GameObject player;

    public void Update()
    {
        if (player != null)
        {
            Vector3 position = player.transform.position;
            Vector3 targetPosition = new Vector3(position.x, altitude, position.z);
            float distance = Vector3.Distance(transform.position, targetPosition);

            if (distance > 0)
            {
                Vector3 direction = (targetPosition - transform.position).normalized;
                Vector3 newPosition = transform.position + direction * speed * Time.deltaTime * distance;
                float distanceAfterMoving = Vector3.Distance(newPosition, targetPosition);

                if (distanceAfterMoving > distance || distanceAfterMoving < 0.01f)
                {
                    newPosition = targetPosition;
                }

                transform.position = newPosition;
            }
        }
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;

        Vector3 position = player.transform.position;
        transform.position = new Vector3(position.x, altitude, position.z);

        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(90, 90, 0);
        transform.rotation = rotation;
    }
}
