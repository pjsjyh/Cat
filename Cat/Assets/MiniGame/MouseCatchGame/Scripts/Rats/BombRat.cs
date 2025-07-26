using UnityEngine;
// 3. 폭탄쥐 클래스
public class BombRat : BaseRat
{
    [Header("Bomb Settings")]
    [SerializeField] private int scorePenalty = -2;
    [SerializeField] private float timePenalty = 2f;

    public override void OnHit()
    {
        // 폭탄 효과 발생
        ExplodeBomb();
    }

    private void ExplodeBomb()
    {
        if (isCaught) return;

        isCaught = true;
        isActive = false;
        ratCollider.enabled = false;

        // 폭탄 사운드 재생
        if (ratData.hitSound != null)
        {
            AudioSource.PlayClipAtPoint(ratData.hitSound, transform.position);
        }

        // 폭탄 이펙트 생성
        if (ratData.hitEffect != null)
        {
            Instantiate(ratData.hitEffect, transform.position, Quaternion.identity);
        }

        // 점수 감소 및 시간 감소 이벤트 발생
        GameEvents.OnBombExploded?.Invoke(scorePenalty, timePenalty);

        // 폭발 애니메이션
        animator.SetTrigger("Explode");

        HideRat();
    }
}
