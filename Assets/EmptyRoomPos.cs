using UnityEngine;

public class EmptyRoomPos : MonoBehaviour
{
    public enum RoomStatus
    {
        Empty,
        Yield,
        Completed
    }
    public RoomStatus status;
}
