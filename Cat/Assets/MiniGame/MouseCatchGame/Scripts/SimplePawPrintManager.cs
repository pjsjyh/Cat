using UnityEngine;
using System.Collections;

public class SimplePawPrintManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Sprite pawPrintSprite; // 발자국 스프라이트
    [SerializeField] private float duration = 1.5f; // 지속 시간
    [SerializeField] private int sortingOrder = 10; // 렌더링 순서
    [SerializeField] private float scaleAnimationTime = 0.2f; // 크기 애니메이션 시간
    [SerializeField] private float waitTime = 0.3f; // 대기 시간

    // 싱글톤 패턴
    public static SimplePawPrintManager Instance { get; private set; }

    private Camera mainCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            mainCamera = Camera.main;
            if (mainCamera == null)
                mainCamera = FindObjectOfType<Camera>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // 마우스 클릭 감지 - 모든 클릭에 발자국 생성
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            worldPos.z = 0; // 2D이므로 z값을 0으로 설정

            ShowPawPrint(worldPos);
        }
    }

    public void ShowPawPrint(Vector3 position)
    {
        if (pawPrintSprite == null) return;

        // 새 GameObject 생성
        GameObject pawPrint = new GameObject("PawPrint");
        pawPrint.transform.position = position;

        // SpriteRenderer 추가
        SpriteRenderer sr = pawPrint.AddComponent<SpriteRenderer>();
        sr.sprite = pawPrintSprite;
        sr.sortingOrder = sortingOrder;

        // 랜덤 회전 (더 자연스럽게)
        pawPrint.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-45f, 45f));

        // 약간의 랜덤 크기 변화
        float randomScale = Random.Range(0.8f, 1.2f);
        pawPrint.transform.localScale = Vector3.one * randomScale;

        // 애니메이션 시작
        StartCoroutine(PawPrintAnimation(pawPrint, sr, randomScale));
    }

    private IEnumerator PawPrintAnimation(GameObject pawPrint, SpriteRenderer spriteRenderer, float targetScale)
    {
        // 초기 설정
        Vector3 finalScale = Vector3.one * targetScale;
        Color originalColor = spriteRenderer.color;

        pawPrint.transform.localScale = Vector3.zero;

        // 1단계: 크기 애니메이션 (튀어나오는 효과)
        float elapsed = 0f;

        while (elapsed < scaleAnimationTime)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / scaleAnimationTime;

            // 이징 효과 (탄성 있게)
            float easedProgress = 1f - Mathf.Pow(1f - progress, 3f);
            pawPrint.transform.localScale = Vector3.Lerp(Vector3.zero, finalScale, easedProgress);
            yield return null;
        }

        pawPrint.transform.localScale = finalScale;

        // 2단계: 대기
        yield return new WaitForSeconds(waitTime);

        // 3단계: 페이드 아웃
        float fadeTime = duration - scaleAnimationTime - waitTime;
        elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeTime);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // 객체 제거
        Destroy(pawPrint);
    }

    // 게임 매니저에서 게임이 종료되면 모든 발자국 정리
    public void ClearAllPawPrints()
    {
        GameObject[] pawPrints = GameObject.FindGameObjectsWithTag("PawPrint");
        foreach (GameObject pawPrint in pawPrints)
        {
            Destroy(pawPrint);
        }
    }

    // 발자국 이펙트 활성화/비활성화
    [Header("Control")]
    [SerializeField] private bool enablePawPrints = true;

    public void SetPawPrintsEnabled(bool enabled)
    {
        enablePawPrints = enabled;
    }

    // Update에서 enablePawPrints 체크 추가
    private void LateUpdate()
    {
        if (!enablePawPrints) return;

        // 추가 기능: 게임이 일시정지되었을 때는 발자국 생성 안함
        if (Time.timeScale == 0) return;
    }
}