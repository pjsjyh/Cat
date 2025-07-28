using UnityEngine;
// 2. 일반쥐 클래스
public class NormalRat : BaseRat
{
    public override void OnHit()
    {
        currentHealth--;

        // 사운드 재생
        if (ratData.hitSound != null)
        {
            AudioSource.PlayClipAtPoint(ratData.hitSound, transform.position);
        }

        if (currentHealth <= 0)
        {
            OnCaught();
        }
        else
        {
            // 맞았을 때 애니메이션 (일반쥐는 한 번에 잡히므로 실행되지 않음)
            animator.SetTrigger("Hit");
        }
    }
}
