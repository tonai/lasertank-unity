using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float speed = 2f;

    private GameObject player;

    public void Update()
    {
        if (player != null)
        {
            Vector3 position = player.transform.position;
            Vector3 targetPosition = new Vector3(position.x - 3, 4, position.z + 3);
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
        transform.position = new Vector3(position.x - 3, 4, position.z + 3);

        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(45, 135, 0);
        transform.rotation = rotation;
    }
}
