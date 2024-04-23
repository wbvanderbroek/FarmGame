using UnityEngine;

public class RoomPos : MonoBehaviour
{

    public RoomStatus status;
    public GameObject roomInPosition;

    private void Awake()
    {
        //transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
        RoomGenerator.Instance.roomPositions.Add(transform.position, gameObject);
    }
}
public enum RoomStatus
{
    Empty,
    Yield,
    Completed
}
