using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GachaSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject objListPanel;
    [SerializeField]
    private GameObject gachaBox;
    [SerializeField]
    private GameObject gachaPanel;

    [Header("Assign in Inspector")]
    public GachaBanner banner;

    [Header("Runtime State")]
    public int currency = 30000;   // ���� ��ȭ
    public int totalGachaCount = 0;     // ���� �̱� �� (�������� ������)

    // �����ۺ� ���� �� (�÷��̾� ����)
    private readonly Dictionary<string, int> shards = new Dictionary<string, int>();

    [Serializable]
    public struct RollResult
    {
        public string stageId;
        public Furniture item;
        public Rarity rarity;
       // public int shardGain;
      //  public int shardAfter;     // ���� �� ���� ����
       // public bool leveledUp;     // �̹� �������� nowOwned�� �ö󰬴���
       // public int ownedAfter;     // ��� �� nowOwned
    }

    // === Public APIs ===
    public List<RollResult> Roll10() => RollMulti(10);
    public List<RollResult> Roll35() => RollMulti(35);
    public List<RollResult> RollMulti(int count)
    {
        ClearObjListPanel();
        var list = new List<RollResult>(count);
        for (int i = 0; i < count; i++)
            list.Add(RollOnce());
        return list;
    }
    public void ClearObjListPanel()
    {
        //����Ʈ ui����
        for (int i = objListPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(objListPanel.transform.GetChild(i).gameObject);
        }
    }

    public RollResult RollOnce()
    {
        if (!banner) { Debug.LogWarning("Banner not assigned."); return default; }
        //if (currency < banner.costPerRoll) { Debug.LogWarning("Not enough currency."); return default; }

        // 1) �������� & ���
        var stage = GetStageFor(totalGachaCount);
        var rarity = DecideRarity(stage);

        // 2) ������ ����(�ش� ��� + ����ġ)
        var item = PickItemByWeight(banner.pool, rarity);
        if (!item) { Debug.LogWarning("No item candidate."); return default; }

        GameObject pickObj = Instantiate(gachaBox, objListPanel.transform);
        pickObj.GetComponent<GachaBoxItem>().ChangeImage(item.FurnitureThumbnail);

        var res = new RollResult
        {
            stageId = stage.stageId,
            item = item,
            rarity = rarity,
           // shardGain = gain,
            //shardAfter = curShards,
            //leveledUp = (item.nowOwned > beforeLevel),
            //ownedAfter = item.nowOwned
        };

        // 3) ���� ����
        //int gain = GetShardGain(rarity);
        AddShards(item.furnitureId, 1);
        return res;
        //        // 4) ������(��ȭ) ó��: ���� nowOwned �������� ��� ����
        //        int beforeLevel = item.nowOwned;
        //        while (item.nowOwned < item.maxOwned)
        //        {
        //            int need = CostForLevel(item.nowOwned);
        //            if (curShards < need) break;
        //            curShards -= need;
        //            item.nowOwned += 1;      // �� �װ� SO�� nowOwned�� ���� ���� �״��
        //        }
        //        shards[item.furnitureId] = curShards;

        //        // 5) �ڿ�/����
        //        currency -= banner.costPerRoll;
        //        totalGachaCount += 1;

        //        var res = new RollResult
        //        {
        //            stageId = stage.stageId,
        //            item = item,
        //            rarity = rarity,
        //            shardGain = gain,
        //            shardAfter = curShards,
        //            leveledUp = (item.nowOwned > beforeLevel),
        //            ownedAfter = item.nowOwned
        //        };

        //#if UNITY_EDITOR
        //        Debug.Log($"[{res.stageId}] {item.furnitureId} ({res.rarity}) +{res.shardGain} �� shards:{res.shardAfter}, owned:{res.ownedAfter}");
        //#endif
        //        return res;
    }
    public void OnClickGachaBtn(int cash, int boxnum)
    {
        //Ŭ���ؼ� ��í
        if (PlayerDataManager.Instance.ReturnPlayerCash() >= cash)
        {
            gachaPanel.SetActive(true);
            var r = RollMulti(boxnum);
            foreach (var kvp in shards)
            {
                FurnitureManager.Instance.AddPieces(kvp.Key, kvp.Value);
            }
            FurnitureManager.Instance.DataUpdateFurniture();

            totalGachaCount += 1; //��íȽ�� ����
        }
     
    }
    StageConfig GetStageFor(int totalRollsBefore)
    {
        //���° ��������(��í����)���� �˻�
        StageConfig pick = null;
        foreach (var s in banner.stages)
            if (totalRollsBefore >= s.minTotalRolls) pick = s;
        return pick ?? banner.stages[0];
    }

    Rarity DecideRarity(StageConfig stage)
    {
        // ���� 1.0�� �ƴ� ���� ������ ����ȭ
        float sum = 0f;
        for (int i = 0; i < stage.rates.Count; i++) sum += stage.rates[i].rate;
        if (sum <= 0f) return Rarity.Common;

        float r = UnityEngine.Random.value;
        float acc = 0f;
        for (int i = 0; i < stage.rates.Count; i++)
        {
            acc += stage.rates[i].rate / sum;
            if (r < acc) return stage.rates[i].rarity;
        }
        return stage.rates[^1].rarity;
    }

    Furniture PickItemByWeight(GachaPool pool, Rarity rarity)
    {
        //���� ������ ����Ʈ ���ÿ�
        var list = new List<(Furniture f, int w)>();
        int total = 0;

        foreach (var e in pool.entries)
        {
            //���� ã�� �������, ��í���� �´��� �˻�
            if (!e.item || !e.item.gachaEligible) continue;
            if (e.item.rarity != rarity) continue;

            int w = Mathf.Max(1, e.weight);
            list.Add((e.item, w));
            total += w;
        }

        // �ش� ����� �ĺ��� ������ ��ü���� ���� (����)
        if (list.Count == 0)
        {
            foreach (var e in pool.entries)
            {
                if (!e.item || !e.item.gachaEligible) continue;
                int w = Mathf.Max(1, e.weight);
                list.Add((e.item, w));
                total += w;
            }
            if (list.Count == 0) return null;
        }
        //���������� ���� ������ ����
        int r = UnityEngine.Random.Range(0, total);
        int acc = 0;
        foreach (var (f, w) in list)
        {
            acc += w;
            if (r < acc) return f;
        }
        return list[0].f;
    }
    public void AddPiece(string furnitureId, int amount)
    {
        // ����� ���� ��� �ҷ�����
        var list = PlayerDataManager.Instance.playerData.roomData.furnitureList;

        foreach (var data in list)
        {
            if (data.id == furnitureId)
            {
                data.nowPeice += amount;
                break;
            }
        }

        // �ʿ��ϸ� ����
        PlayerDataManager.Instance.SaveData();
    }
    int GetShardGain(Rarity rarity)
    {
        for (int i = 0; i < banner.shardDrops.Count; i++)
            if (banner.shardDrops[i].rarity == rarity)
                return UnityEngine.Random.Range(banner.shardDrops[i].range.x, banner.shardDrops[i].range.y + 1);
        return 1;
    }

    int CostForLevel(int level)
    {
        if (banner.shardCostsByLevel == null || banner.shardCostsByLevel.Count == 0) return int.MaxValue;
        int idx = Mathf.Clamp(level, 0, banner.shardCostsByLevel.Count - 1);
        return Mathf.Max(1, banner.shardCostsByLevel[idx]);
    }

    int GetShards(string id) => shards.TryGetValue(id, out var n) ? n : 0;
    void AddShards(string id, int add) => shards[id] = GetShards(id) + add;
}

[System.Serializable] 
public class RarityRate 
{
    //�� ��í ��� �� Ȯ��
    public Rarity rarity; 
    public float rate; 
}
[System.Serializable]
public class StageConfig
{
    public string stageId = "LV1";
    public int minTotalRolls = 0;   
    // �� �� �̻��̸� �� �������� ���� (���� ū min ���� �� �ִ� ä��) 0~2 = 1�ܰ� / 3~7 = 2�ܰ�
    public List<RarityRate> rates = new()
    {
       new RarityRate{ rarity = Rarity.Legendary, rate = 0.01f },
    new RarityRate{ rarity = Rarity.Epic,      rate = 0.04f },
    new RarityRate{ rarity = Rarity.Rare,      rate = 0.20f },
    new RarityRate{ rarity = Rarity.Common,    rate = 0.75f },
    };
}


[System.Serializable]
public class GachaEntry
{
    //�̹� �̱⿡ ���� �� �ִ� ������ ���
    //���� ��� ���� �� �������� ����(Ŭ���� �߻���)
    public Furniture item;
    public int weight = 100;
}




[System.Serializable] public class RarityShardRange { public Rarity rarity; public Vector2Int range = new(3, 3); }

