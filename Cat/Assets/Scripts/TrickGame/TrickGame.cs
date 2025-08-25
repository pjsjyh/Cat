using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "TrickGame/Level")]
public class TrickGame : ScriptableObject
{
    [Min(1)] public int level = 1;   // ���� ��ȣ(ǥ��/�ĺ���)
    [Min(2)] public int cupCount = 3;
    [Min(1)] public int swapCount = 8;
}

