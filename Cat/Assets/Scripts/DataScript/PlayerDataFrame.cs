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
                PlayerName = "newName222",
                PlayerCoin = 3000,
                PlayerCash = 0,
                PlayerPlace = 0, //플레이어 현재위치
            };

            roomData = new PlayerRoomData(); // 필요하면 내부 초기화 추가
            catData = new PlayerCatData();   // List 초기화 포함
        }
    }
    [System.Serializable]
    public class PlayerPersonalData
    {
        [SerializeField]  private string _playerName;
        [SerializeField]  private int _playerCoin;
        [SerializeField] private int _playerCash;
        [SerializeField] private int _playerPlace;

        public event System.Action<string> OnNameChanged; 
        public event System.Action<int> OnCoinChanged;
        public event System.Action<int> OnCashChanged;
        public event System.Action<int> OnPlaceChanged;
        public string PlayerName
        {
            get => _playerName;
            set
            {
                if (_playerName != value)
                {
                    _playerName = value;
                    OnNameChanged?.Invoke(value);
                }
            }
        }
        public int PlayerCoin
        {
            get => _playerCoin;
            set
            {
                if (_playerCoin != value)
                {
                    _playerCoin = value;
                    OnCoinChanged?.Invoke(value);
                }
            }
        }

        public int PlayerCash
        {
            get => _playerCash;
            set
            {
                if (_playerCash != value)
                {
                    _playerCash = value;
                    OnCashChanged?.Invoke(value);
                }
            }
        }
        public int PlayerPlace
        {
            get => _playerPlace;
            set
            {
                if (_playerPlace != value)
                {
                    _playerPlace = value;
                    OnPlaceChanged?.Invoke(value);
                }
            }
        }
    }
    [System.Serializable]
    public class PlayerRoomData
    {
        public List<FurnitureSaveData> furnitureList = new List<FurnitureSaveData>();
    }

    [System.Serializable]
    public class PlayerCatData
    {
        public List<CatSaveData> catDataList = new List<CatSaveData>();
        
    }
}
