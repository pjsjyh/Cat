using UnityEngine;
using System.Collections.Generic;

public class BladeController : MonoBehaviour
{
    [Header("Blade Settings")]
    public GameObject bladeTrailPrefab;
    public float minCuttingVelocity = 0.001f;

    private Camera mainCamera;
    private bool isCutting = false;
    private Vector2 previousMousePosition;
    private GameObject currentBladeTrail;
    private List<Vector3> bladePositions = new List<Vector3>();

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            mainCamera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        // 컴포넌트가 비활성화되어 있으면 아무것도 하지 않음
        if (!enabled) return;

        // 게임이 일시정지되었거나 종료되었으면 블레이드 입력 처리하지 않음
        if (IsGamePausedOrOver())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartCutting();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopCutting();
        }

        if (isCutting)
        {
            UpdateCutting();
        }
    }

    private bool IsGamePausedOrOver()
    {
        // GameManager가 존재하지 않으면 게임이 진행 중인 것으로 간주
        if (FruitGameManager.Instance == null)
            return false;

        // 게임이 일시정지되었거나 종료되었는지 확인
        bool isPaused = FruitGameManager.Instance.IsPaused;
        bool isGameOver = FruitGameManager.Instance.IsGameOver;

        return isPaused || isGameOver;
    }

    private void StartCutting()
    {
        // 게임 상태 재확인
        if (IsGamePausedOrOver()) return;

        isCutting = true;
        previousMousePosition = Input.mousePosition;
        bladePositions.Clear();

        Vector3 worldPosition = GetMouseWorldPosition();
        bladePositions.Add(worldPosition);

        // 검날 트레일 생성
        if (bladeTrailPrefab != null)
        {
            currentBladeTrail = Instantiate(bladeTrailPrefab);
            LineRenderer lr = currentBladeTrail.GetComponent<LineRenderer>();
            if (lr != null)
            {
                lr.positionCount = 1;
                lr.SetPosition(0, worldPosition);
            }
        }
    }

    private void UpdateCutting()
    {
        // 게임 상태 재확인
        if (IsGamePausedOrOver())
        {
            ForceStopCutting();
            return;
        }

        Vector2 currentMousePosition = Input.mousePosition;
        Vector2 velocity = currentMousePosition - previousMousePosition;

        if (velocity.magnitude > minCuttingVelocity)
        {
            Vector3 worldPosition = GetMouseWorldPosition();
            bladePositions.Add(worldPosition);

            // 트레일 업데이트
            if (currentBladeTrail != null)
            {
                LineRenderer lr = currentBladeTrail.GetComponent<LineRenderer>();
                if (lr != null)
                {
                    lr.positionCount = bladePositions.Count;
                    for (int i = 0; i < bladePositions.Count; i++)
                    {
                        lr.SetPosition(i, bladePositions[i]);
                    }
                }
            }

            // 충돌 검사 - 게임이 진행 중일 때만
            CheckForCollisions(worldPosition);
        }

        previousMousePosition = currentMousePosition;
    }

    private void StopCutting()
    {
        if (!isCutting) return;

        isCutting = false;

        // 트레일 페이드아웃
        if (currentBladeTrail != null)
        {
            StartCoroutine(FadeOutTrail(currentBladeTrail));
            currentBladeTrail = null;
        }

        bladePositions.Clear();
    }

    private void ForceStopCutting()
    {
        if (!isCutting) return;

        isCutting = false;

        // 즉시 트레일 제거 (페이드 아웃 없이)
        if (currentBladeTrail != null)
        {
            Destroy(currentBladeTrail);
            currentBladeTrail = null;
        }

        bladePositions.Clear();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = 10f; // 카메라로부터의 거리
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }

    private void CheckForCollisions(Vector3 bladePosition)
    {
        // 게임이 일시정지되었거나 종료되었으면 충돌 검사하지 않음
        if (IsGamePausedOrOver()) return;

        // 2D Collider 검사 (과일 게임이므로 2D 사용)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(bladePosition, 0.5f);

        foreach (Collider2D col in colliders)
        {
            // Fruit 컴포넌트 확인
            Fruit fruit = col.GetComponent<Fruit>();
            if (fruit != null && !fruit.IsSliced)
            {
                fruit.SliceFruit();
                continue;
            }

            // 다른 오브젝트들도 필요에 따라 처리
            // 예: 폭탄, 특수 아이템 등
        }
    }

    private System.Collections.IEnumerator FadeOutTrail(GameObject trail)
    {
        if (trail == null) yield break;

        LineRenderer lr = trail.GetComponent<LineRenderer>();
        if (lr != null)
        {
            Color originalColor = lr.material.color;
            float fadeTime = 0.5f;
            float elapsed = 0f;

            while (elapsed < fadeTime && trail != null)
            {
                // 일시정지 중에는 실제 시간으로 페이드
                elapsed += Time.unscaledDeltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeTime);

                if (lr != null)
                {
                    Color newColor = originalColor;
                    newColor.a = alpha;
                    lr.material.color = newColor;
                }

                yield return null;
            }
        }

        if (trail != null)
            Destroy(trail);
    }

    // 외부에서 블레이드를 강제로 중단시킬 수 있는 메서드
    public void DisableBlade()
    {
        // 현재 자르고 있는 상태라면 즉시 중단
        if (isCutting)
        {
            ForceStopCutting();
        }
        enabled = false;
    }

    public void EnableBlade()
    {
        enabled = true;
    }

    // GameManager에서 호출할 수 있는 메서드 - 트레일 즉시 제거
    public void ClearCurrentTrail()
    {
        if (isCutting)
        {
            ForceStopCutting();
        }
    }

    // 디버그용 - 현재 블레이드 상태 확인
    public bool IsCutting => isCutting;

    // OnDrawGizmosSelected를 사용해서 충돌 범위 시각화 (에디터에서만)
    private void OnDrawGizmosSelected()
    {
        if (isCutting && bladePositions.Count > 0)
        {
            Gizmos.color = Color.red;
            Vector3 lastPosition = bladePositions[bladePositions.Count - 1];
            Gizmos.DrawWireSphere(lastPosition, 0.5f);
        }
    }
}