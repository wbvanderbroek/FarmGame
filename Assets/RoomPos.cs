using UnityEngine;

public class RoomPos : MonoBehaviour
{

    public RoomStatus status;
    public GameObject roomInPosition;

    private void Awake()
    {
        RoomGenerator.Instance.roomPositions.Add(transform.position, gameObject);
    }
}
public enum RoomStatus
{
    Empty,
    Yield,
    Completed
}
