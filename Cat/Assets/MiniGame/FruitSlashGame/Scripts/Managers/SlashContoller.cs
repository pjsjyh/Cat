// SlashController.cs - 슬래시 입력 처리
using UnityEngine;
using System.Collections.Generic;

public class SlashController : MonoBehaviour
{
    [Header("Slash Settings")]
    public LayerMask fruitLayerMask = -1;
    public float slashRadius = 0.5f;

    [Header("Slash Trail")]
    public GameObject trailPrefab; // TrailRenderer가 붙은 프리팹
    public float trailWidth = 0.1f;
    public Color trailStartColor = Color.white;
    public Color trailEndColor = new Color(1, 1, 1, 0);
    public float trailTime = 0.5f;

    private Camera mainCamera;
    private bool isSlashing = false;
    private GameObject currentTrail;
    private List<Fruit> slashedFruits = new List<Fruit>();
    private Vector3 lastSlashPoint;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 컴포넌트가 비활성화되어 있거나 게임이 일시정지/종료 상태면 입력 처리하지 않음
        if (!enabled || IsGamePausedOrOver())
        {
            return;
        }

        HandleInput();
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

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartSlash();
        }
        else if (Input.GetMouseButton(0) && isSlashing)
        {
            ContinueSlash();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndSlash();
        }
    }

    private void StartSlash()
    {
        // 게임 상태 재확인
        if (IsGamePausedOrOver()) return;

        isSlashing = true;
        slashedFruits.Clear();

        // TrailRenderer 생성
        if (trailPrefab != null)
        {
            currentTrail = Instantiate(trailPrefab);
            SetupTrail(currentTrail.GetComponent<TrailRenderer>());
        }
        else
        {
            // 런타임에 TrailRenderer 생성
            currentTrail = new GameObject("SlashTrail");
            TrailRenderer trail = currentTrail.AddComponent<TrailRenderer>();
            SetupTrail(trail);
        }

        Vector3 mousePos = GetMouseWorldPosition();
        currentTrail.transform.position = mousePos;
        lastSlashPoint = mousePos;
    }

    private void SetupTrail(TrailRenderer trail)
    {
        trail.material = new Material(Shader.Find("Sprites/Default"));
        trail.startColor = trailStartColor;
        trail.endColor = trailEndColor;
        trail.startWidth = trailWidth;
        trail.endWidth = trailWidth * 0.1f; // 끝부분을 더 얇게
        trail.time = trailTime;
        trail.sortingOrder = 10;
        trail.receiveShadows = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    private void ContinueSlash()
    {
        // 게임 상태 재확인
        if (IsGamePausedOrOver())
        {
            ForceEndSlash();
            return;
        }

        Vector3 mousePos = GetMouseWorldPosition();

        if (currentTrail != null)
        {
            currentTrail.transform.position = mousePos;
        }

        // 이전 점과의 거리가 충분할 때만 체크 (성능 최적화)
        if (Vector3.Distance(lastSlashPoint, mousePos) > 0.1f)
        {
            CheckSlashHit(mousePos);
            lastSlashPoint = mousePos;
        }
    }

    private void EndSlash()
    {
        if (!isSlashing) return;

        isSlashing = false;

        if (currentTrail != null)
        {
            // TrailRenderer는 자동으로 페이드아웃됨
            Destroy(currentTrail, trailTime + 1f);
            currentTrail = null;
        }
    }

    private void ForceEndSlash()
    {
        if (!isSlashing) return;

        isSlashing = false;

        // 즉시 트레일 제거 (페이드 아웃 없이)
        if (currentTrail != null)
        {
            Destroy(currentTrail);
            currentTrail = null;
        }

        slashedFruits.Clear();
    }

    private void CheckSlashHit(Vector3 slashPoint)
    {
        // 게임이 일시정지되었거나 종료되었으면 충돌 검사하지 않음
        if (IsGamePausedOrOver()) return;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(slashPoint, slashRadius, fruitLayerMask);

        foreach (Collider2D hitCollider in hitColliders)
        {
            Fruit fruit = hitCollider.GetComponent<Fruit>();
            if (fruit != null && !slashedFruits.Contains(fruit) && !fruit.IsSliced)
            {
                slashedFruits.Add(fruit);
                fruit.SliceFruit();
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    // 외부에서 슬래시를 강제로 중단시킬 수 있는 메서드 (GameManager에서 호출)
    public void ClearCurrentTrail()
    {
        if (isSlashing)
        {
            ForceEndSlash();
        }
    }

    public void DisableSlash()
    {
        // 현재 슬래시 상태라면 즉시 중단
        if (isSlashing)
        {
            ForceEndSlash();
        }
        enabled = false;
    }

    public void EnableSlash()
    {
        enabled = true;
    }

    // 디버그용 - 현재 슬래시 상태 확인
    public bool IsSlashing => isSlashing;

    private void OnDrawGizmosSelected()
    {
        if (isSlashing)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetMouseWorldPosition(), slashRadius);
        }
    }
}