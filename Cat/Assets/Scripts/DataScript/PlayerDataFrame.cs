using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataFrame : MonoBehaviour
{
    //플레이어가 저장할 데이터 총괄 관리

    public class PlayerData
    {
        public PlayerPersonalData playerPersonalData;
        public PlayerRoomData roomData;
        public PlayerCatData catData;

        public PlayerData()
        {
            playerPersonalData = new PlayerPersonalData
            {
                playerName = "newName",
                playerCoin = 0,
                playerCash = 0
            };

            roomData = new PlayerRoomData(); // 필요하면 내부 초기화 추가
            catData = new PlayerCatData();   // List 초기화 포함
        }
    }
    public class PlayerPersonalData
    {
        public string playerName;
        public int playerCoin;
        public int playerCash;
    }
    public class PlayerRoomData
    {

    }
    public class Furniture
    {
        //위치, 이름, 설치여부
    }

    public class PlayerCatData
    {
        List<CatData> catDataList = new List<CatData>();
    }

    public class CatData
    {
        //각 고양이 애니메이션, 코스튬, 장비
        public float health;
        public string name;
        public float jump;
        public string currentRoomNum;
    }
}
