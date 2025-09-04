using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class FurnitureManager : MonoBehaviour
{
    //처음 사용자 가구 셋팅 매니저
    public static FurnitureManager Instance { get; private set; }
    public Transform furnitureParent; // 가구가 추가 될 부모

    private Dictionary<string, GameObject> placedFurniture = new(); //설치된 가구 리스트
    private Dictionary<string, FurnitureSaveData> furnitureSaveData = new(); //가구 saveData

    public Dictionary<string, Furniture> allFurnitureData= new(); //모든 가구 데이터

    private bool furnitureEditorModeOn = false;

    [SerializeField] 
    FloorNavGrid grid;     
    [SerializeField] 
    DepthSorter depthSorter;

    public event Action<string, int> OnPiecesChanged;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지되게

    }
    private void Start()
    {
        setAllFurnitureData();
        SpawnFurnitures(PlayerDataManager.Instance.playerData.roomData.furnitureList);
        StartCoroutine(EndOfFrameBuild()); // ← 한 프레임 뒤에 정렬/등록

    }
    private void OnEnable()
    {
        //처음 켜졌을 때 데이터 업데이트
        List<Furniture> getPlayerFurniture = PlayerFurnitureManager.Instance.GetFurnitures();
        foreach (var furniture in getPlayerFurniture)
        {
            if(placedFurniture.TryGetValue(furniture.furnitureId, out GameObject furnitureObj))
            {
                //설치되어있는 가구 일 경우 데이터 업데이트
                furnitureObj.GetComponent<FurnitureDragHandler>().SettingFurnitureData(furniture);
                placedFurniture[furniture.furnitureId] = furnitureObj;
            }
        }
    }
    private void OnDisable()
    {
        
    }
    public void SpawnFurnitures(List<FurnitureSaveData> furnitureList)
    {
        //게임 시작 시 설치되어있는 가구 셋팅
        foreach (var furnitureSaveData in furnitureList)
        {
            Furniture furnitureData = Resources.Load<Furniture>($"Data/Furniture/{furnitureSaveData.id}");
            furnitureData.isPlaced = furnitureSaveData.isPlaced;
            furnitureData.installPosition = furnitureSaveData.position;
            furnitureData.nowPeice = furnitureSaveData.nowPeice;

            //내 데이터 모든 가구 리스트에 정보 업데이트하기.
            allFurnitureData[furnitureData.furnitureId] = (furnitureData);

            //설치되어있는 가구 셋팅
            if (furnitureSaveData.isPlaced)
            {
                GameObject furnitureObj = Instantiate(furnitureData.FurniturePrefab, furnitureSaveData.position, Quaternion.identity, furnitureParent);
                furnitureObj.transform.SetParent(furnitureParent.transform, false);

                furnitureObj.GetComponent<FurnitureDragHandler>().SettingFurnitureData(furnitureData);
                placedFurniture[furnitureSaveData.id] = furnitureObj;
            }
           //내 가구에 등록
           PlayerFurnitureManager.Instance.AddFurniture( furnitureData );
        }
    }
    private void setAllFurnitureData()
    {
        //모든 가구 종류 list 저장해두기
        Furniture[] allFurnitures = Resources.LoadAll<Furniture>("Data/Furniture");
        foreach (Furniture f in allFurnitures)
        {
            allFurnitureData[f.furnitureId] = f;
        }
    }
    IEnumerator EndOfFrameBuild()
    {
        yield return null; // RectTransform 배치가 끝나도록 1프레임 대기

        if (grid) grid.Clear();

        foreach (var go in placedFurniture.Values)
        {
            go.GetComponent<FurnitureNav>()?.Register(); // 점유 셀 막기
        }
        depthSorter?.SortNow(); // 앞/뒤 정렬
    }
    public void AddPieces(string id, int add)
    {
        if (!allFurnitureData.TryGetValue(id, out var s)) return;
        
        
        s.nowPeice += add;
        // 저장 호출은 여기서: PlayerDataManager.Instance.SaveData();
        OnPiecesChanged?.Invoke(id, s.nowPeice);
    }
    public void AddFurniture(string getId, GameObject getObj)
    {
        //가구를 설치 리스트에 추가
        placedFurniture[getId] = getObj;
    }
    public bool FurnitureIsPlaced(string getId)
    {
        //가구 설치 여부 확인
        if (placedFurniture.ContainsKey(getId))
        {
            return true;
        }
        return false;
    }
    public GameObject FindFurniture(string getId)
    {
        if (placedFurniture.ContainsKey(getId))
        {
            return placedFurniture[getId];
        }
        return null;
    }
    public void RemoveFurnitureInPlace(string getId)
    {
        placedFurniture.Remove(getId);
        furnitureSaveData.Remove(getId);
    }
  
    public bool isFurnitureEditorModeOn()
    {
        return furnitureEditorModeOn;
    }
    public void SetFurnitureEditorMode(bool getMode)
    {
        furnitureEditorModeOn = getMode;
    }
    public List<Furniture> ReturnAllFurnitureData()
    {
        List<Furniture> furnitureList = new List<Furniture>(allFurnitureData.Values);
        return furnitureList;
    }

}
