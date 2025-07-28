
// 6. 게임 이벤트 시스템
public static class GameEvents
{
    // 쥐 관련 이벤트
    public static System.Action<RatType, int> OnRatCaught;
    public static System.Action<int, float> OnBombExploded; // 점수감소, 시간감소

    // 게임 상태 이벤트
    public static System.Action<int> OnScoreChanged;
    public static System.Action<float> OnTimeChanged;
}