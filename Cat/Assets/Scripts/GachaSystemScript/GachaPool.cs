using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GachaPool", menuName = "Gacha/Pool")]
public class GachaPool : ScriptableObject
{
    //가구 뽑기 리스트
    public List<GachaEntry> entries = new List<GachaEntry>();
}
