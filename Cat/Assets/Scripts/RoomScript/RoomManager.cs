using JetBrains.Annotations;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
   public enum Room { MainRoom, CatRoom, GameRoom, StoreRoom}
   public static RoomManager instance;
   public Room currentRoom = Room.MainRoom;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeRoomState(Room getRoom)
    {
        switch (getRoom) { 
            case Room.MainRoom:
                break;
            case Room.CatRoom:
                break;
            case Room.GameRoom:
                break;
            case Room.StoreRoom:
                break;
        }
    }
}
