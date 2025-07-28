using UnityEngine;
public abstract class BaseRat : MonoBehaviour
{
    [Header("Rat Settings")]
    [SerializeField] protected RatData ratData;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Collider2D ratCollider;

    protected int currentHealth;
    protected bool isActive = false;
    protected bool isCaught = false;

    protected virtual void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        if (ratData == null) return;

        currentHealth = ratData.health;
        spriteRenderer.sprite = ratData.ratSprite;
        StartCoroutine(ShowSequence());
    }

    // 런타임에 ratData 설정하는 메서드
    public virtual void SetRatData(RatData data)
    {
        ratData = data;
        Initialize();
    }

    protected System.Collections.IEnumerator ShowSequence()
    {
        // 쥐 등장 애니메이션
        animator.SetTrigger("Show");
        yield return new WaitForSeconds(0.5f); // 등장 애니메이션 시간

        isActive = true;
        ratCollider.enabled = true;

        // 일정 시간 후 사라짐
        yield return new WaitForSeconds(ratData.showDuration);

        if (!isCaught)
        {
            HideRat();
        }
    }

    protected virtual void OnMouseDown()
    {
        if (isActive && !isCaught)
        {
            OnHit();
        }
    }

    public abstract void OnHit();

    protected virtual void OnCaught()
    {
        if (isCaught) return;

        isCaught = true;
        isActive = false;
        ratCollider.enabled = false;

        // 점수 이벤트 발생
        GameEvents.OnRatCaught?.Invoke(ratData.ratType, ratData.scoreValue);

        // 잡힌 효과
        if (ratData.hitEffect != null)
        {
            Instantiate(ratData.hitEffect, transform.position, Quaternion.identity);
        }

        HideRat();
    }

    protected virtual void HideRat()
    {
        animator.SetTrigger("Hide");
        StartCoroutine(ReturnToPool());
    }

    protected System.Collections.IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(0.5f); // 사라지는 애니메이션 시간

        // 오브젝트 풀로 반환
        RatPool.Instance.ReturnRat(this);
    }
}