
// 게임 이벤트 시스템
public static class GameEvents
{
    // 게임 상태 이벤트
    public static System.Action<int> OnScoreChanged;
    public static System.Action<float> OnTimeChanged;
}