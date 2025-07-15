using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataFrame : MonoBehaviour
{
    //플레이어가 저장할 데이터 총괄 관리
    [System.Serializable]
    public class PlayerData
    {
        public PlayerPersonalData playerPersonalData;
        public PlayerRoomData roomData;
        public PlayerCatData catData;

        public PlayerData()
        {
            playerPersonalData = new PlayerPersonalData
            {
                playerName = "newName222",
                playerCoin = 0,
                playerCash = 0
            };

            roomData = new PlayerRoomData(); // 필요하면 내부 초기화 추가
            catData = new PlayerCatData();   // List 초기화 포함
        }
    }
    [System.Serializable]
    public class PlayerPersonalData
    {
        public string playerName;
        public int playerCoin;
        public int playerCash;
    }
    [System.Serializable]
    public class PlayerRoomData
    {
        List<Furniture> furnitureList = new List<Furniture>();
    }

    [System.Serializable]
    public class PlayerCatData
    {
        List<Cat> catDataList = new List<Cat>();
    }
}
