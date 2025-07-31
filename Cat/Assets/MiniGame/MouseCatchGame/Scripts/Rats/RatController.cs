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

    private RatGameManager gameManager;
    private RatData currentRatData;
    private int currentHealth;
    private bool isActive = false;
    private bool isCaught = false;
    private Coroutine currentSequence;

    public void Initialize(RatGameManager manager)
    {
        gameManager = manager;

        // 게임 매니저 이벤트 구독
        if (gameManager != null)
        {
            gameManager.OnGamePaused += OnGamePaused;
            gameManager.OnGameResumed += OnGameResumed;
        }
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.OnGamePaused -= OnGamePaused;
            gameManager.OnGameResumed -= OnGameResumed;
        }
    }

    private void OnGamePaused()
    {
        // 일시정지 처리
    }

    private void OnGameResumed()
    {
        // 재개 처리
    }

    public void SetupRat(RatData ratData)
    {
        currentRatData = ratData;
        currentHealth = currentRatData.health;
        ratSpriteRenderer.sprite = currentRatData.ratSprite;
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

        // 게임 매니저가 있으면 일시정지 체크
        if (gameManager != null)
        {
            yield return new WaitWhile(() => gameManager.IsPaused);
        }

        isActive = true;
        ratCollider.enabled = true;

        float remainingTime = currentRatData.showDuration;
        while (remainingTime > 0)
        {
            if (gameManager != null)
            {
                yield return new WaitWhile(() => gameManager.IsPaused);
            }
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
            if (gameManager != null)
            {
                yield return new WaitWhile(() => gameManager.IsPaused);
            }

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
        // 게임 매니저 체크
        if (gameManager != null && (gameManager.IsPaused || !gameManager.IsGamePlaying))
            return;

        if (isActive && !isCaught)
        {
            OnRatHit();
        }
    }

    private void OnRatHit()
    {
        if (currentRatData.hitSound != null && audioSource != null)
        {
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

        if (currentRatData.hitEffect != null)
        {
            Instantiate(currentRatData.hitEffect, transform.position, Quaternion.identity);
        }

        RatGameEvents.OnBombExploded?.Invoke(currentRatData.scorePenalty, currentRatData.timePenalty);
        HideRat();
    }

    private void HandleHelmetRat()
    {
        currentHealth--;
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

        RatGameEvents.OnRatCaught?.Invoke(currentRatData.ratType, currentRatData.scoreValue);

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
            if (gameManager != null)
            {
                yield return new WaitWhile(() => gameManager.IsPaused);
            }

            ratTransform.localPosition = originalPos + Vector3.right * 0.1f;
            yield return new WaitForSeconds(0.05f);

            if (gameManager != null)
            {
                yield return new WaitWhile(() => gameManager.IsPaused);
            }

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

        // 풀에 반환
        RatPool ratPool = FindObjectOfType<RatPool>();
        if (ratPool != null)
        {
            ratPool.ReturnRat(this);
        }
    }
}