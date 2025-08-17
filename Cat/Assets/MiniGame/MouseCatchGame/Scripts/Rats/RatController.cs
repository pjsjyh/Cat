using UnityEngine;
using System.Collections;

public class RatController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer ratSpriteRenderer;
    [SerializeField] private Transform ratTransform;
    [SerializeField] private Collider2D ratCollider;
    [SerializeField] private AudioSource audioSource;

    [Header("Animation Settings")]
    [SerializeField] private float hideY = -1f;
    [SerializeField] private float showY = 0f;
    [SerializeField] private float animationSpeed = 2f;

    [SerializeField] private RatGameManager gameManager;
    private RatData currentRatData;
    private int currentHealth;

    private bool isActive = false;
    private bool isCaught = false;
    private Coroutine currentSequence;

    // RatSpawner 참조 추가
    private RatSpawner ratSpawner;

    private void Awake()
    {
        // RatSpawner 찾기 (한 번만 찾아서 캐싱)
        ratSpawner = FindObjectOfType<RatSpawner>();
    }

    public void Initialize()
    {
        Debug.Log("RatController initialized without GameManager");

        // RatSpawner가 없다면 다시 찾기
        if (ratSpawner == null)
        {
            ratSpawner = FindObjectOfType<RatSpawner>();
        }
    }

    public void SetupRat(RatData ratData)
    {
        currentRatData = ratData;
        currentHealth = currentRatData.health;
        ratSpriteRenderer.sprite = currentRatData.ratSprite;
        audioSource.clip = currentRatData.hitSound;
        isCaught = false;
        isActive = false;

        Vector3 pos = ratTransform.localPosition;
        pos.y = hideY;
        ratTransform.localPosition = pos;

        ratCollider.enabled = false;

        if (currentSequence != null)
        {
            StopCoroutine(currentSequence);
        }

        currentSequence = StartCoroutine(ShowSequence());
    }

    private IEnumerator ShowSequence()
    {
        yield return StartCoroutine(MoveRatTo(showY));

        isActive = true;
        ratCollider.enabled = true;

        float remainingTime = currentRatData.showDuration;
        while (remainingTime > 0)
        {
            yield return null;
            remainingTime -= Time.deltaTime;
        }

        if (!isCaught)
        {
            HideRat();
        }
    }

    private IEnumerator MoveRatTo(float targetY)
    {
        Vector3 startPos = ratTransform.localPosition;
        Vector3 targetPos = new Vector3(startPos.x, targetY, startPos.z);

        float elapsedTime = 0f;
        float duration = 1f / animationSpeed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = 1f - Mathf.Pow(1f - t, 3f);

            ratTransform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        ratTransform.localPosition = targetPos;
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse clicked on rat!");
        OnRatHit();

        if (isActive && !isCaught)
        {
            Debug.Log("Rat hit!");

            OnRatHit();
        }
        else
        {
            Debug.Log($"Rat not hittable - isActive: {isActive}, isCaught: {isCaught}");
        }
    }

    private void OnRatHit()
    {
        Debug.Log($"Hit rat type: {currentRatData.ratType}");

        ratSpriteRenderer.sprite = currentRatData.ratHitSprite;

        if (currentRatData.hitSound != null && audioSource != null)
        {
            Debug.Log("sound");
            audioSource.PlayOneShot(currentRatData.hitSound);
        }

        switch (currentRatData.ratType)
        {
            case RatType.Normal:
                CatchRat();
                break;
            case RatType.Bomb:
                HandleBombRat();
                break;
            case RatType.Helmet:
                HandleHelmetRat();
                break;
            case RatType.Golden:
                CatchRat();
                break;
            case RatType.Shield:
                HandleShieldRat();
                break;
        }
    }

    private void HandleBombRat()
    {
        if (isCaught) return;

        isCaught = true;
        isActive = false;
        ratCollider.enabled = false;
        RatGameEvents.OnBombExploded?.Invoke(currentRatData.scorePenalty, currentRatData.timePenalty); // 예: -10점, -5초
        if (currentRatData.hitEffect != null)
        {
            Instantiate(currentRatData.hitEffect, transform.position, Quaternion.identity);
        }

        Debug.Log($"Bomb exploded! Penalty: {currentRatData.scorePenalty}");
        HideRat();
    }

    private void HandleHelmetRat()
    {
        currentHealth--;
        Debug.Log($"Helmet rat hit! Health remaining: {currentHealth}");

        if (currentHealth <= 0)
        {
            CatchRat();
        }
        else
        {
            StartCoroutine(HitEffect());
        }
    }

    private void HandleShieldRat()
    {
        currentHealth--;
        Debug.Log($"Shield rat hit! Health remaining: {currentHealth}");

        if (currentHealth <= 0)
        {
            CatchRat();
        }
        else
        {
            StartCoroutine(HitEffect());
        }
    }

    private void CatchRat()
    {
        if (isCaught) return;

        isCaught = true;
        isActive = false;
        ratCollider.enabled = false;
        // CurrentScore = CurrentScore + 10;
        Debug.Log($"Rat caught! Type: {currentRatData.ratType}, Score: {currentRatData.scoreValue}");
        RatGameEvents.OnRatCaught?.Invoke(currentRatData.ratType, currentRatData.scoreValue);

        // gameManager.HandleRatCaught(currentRatData.ratType, currentRatData.scoreValue);
        if (currentRatData.hitEffect != null)
        {
            Instantiate(currentRatData.hitEffect, transform.position, Quaternion.identity);
        }

        HideRat();
    }

    private IEnumerator HitEffect()
    {
        Vector3 originalPos = ratTransform.localPosition;

        for (int i = 0; i < 3; i++)
        {
            ratTransform.localPosition = originalPos + Vector3.right * 0.1f;
            yield return new WaitForSeconds(0.05f);

            ratTransform.localPosition = originalPos + Vector3.left * 0.1f;
            yield return new WaitForSeconds(0.05f);
        }

        ratTransform.localPosition = originalPos;
    }

    private void HideRat()
    {
        if (currentSequence != null)
        {
            StopCoroutine(currentSequence);
        }
        currentSequence = StartCoroutine(HideSequence());
    }

    private IEnumerator HideSequence()
    {
        yield return StartCoroutine(MoveRatTo(hideY));

        // 구멍을 비어있는 상태로 설정 (쥐가 완전히 사라진 후)
        if (ratSpawner != null && transform.parent != null)
        {
            ratSpawner.SetHoleOccupancy(transform.parent, false);
            Debug.Log($"Released hole: {transform.parent.name}");
        }

        // 풀에 반환
        RatPool ratPool = FindObjectOfType<RatPool>();
        if (ratPool != null)
        {
            ratPool.ReturnRat(this);
        }
        else
        {
            // 풀이 없으면 그냥 비활성화
            gameObject.SetActive(false);
        }
    }
}