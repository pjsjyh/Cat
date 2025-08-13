using UnityEngine;
using System.Collections;

public class PawPrintEffect : MonoBehaviour
{
    [Header("Paw Print Settings")]
    [SerializeField] private GameObject pawPrintPrefab; // 발자국 프리팹
    [SerializeField] private float fadeOutDuration = 1f; // 사라지는 시간
    [SerializeField] private float scaleAnimation = 0.2f; // 크기 애니메이션 시간
    [SerializeField] private Vector3 startScale = Vector3.zero; // 시작 크기
    [SerializeField] private Vector3 endScale = Vector3.one; // 끝 크기
    [SerializeField] private LayerMask clickLayerMask = -1; // 클릭 가능한 레이어

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            mainCamera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        // 마우스 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            CreatePawPrint();
        }
    }

    private void CreatePawPrint()
    {
        if (pawPrintPrefab == null || mainCamera == null) return;

        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        // 발자국 생성
        GameObject pawPrint = Instantiate(pawPrintPrefab, worldPos, Quaternion.identity);

        // 약간의 랜덤 회전 추가
        pawPrint.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-30f, 30f));

        // 애니메이션 시작
        StartCoroutine(AnimatePawPrint(pawPrint));
    }

    private IEnumerator AnimatePawPrint(GameObject pawPrint)
    {
        SpriteRenderer spriteRenderer = pawPrint.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) yield break;

        // 초기 설정
        pawPrint.transform.localScale = startScale;
        Color originalColor = spriteRenderer.color;

        // 크기 애니메이션 (커지는 효과)
        float elapsed = 0f;
        while (elapsed < scaleAnimation)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / scaleAnimation;
            pawPrint.transform.localScale = Vector3.Lerp(startScale, endScale, progress);
            yield return null;
        }

        pawPrint.transform.localScale = endScale;

        // 잠시 대기
        yield return new WaitForSeconds(0.3f);

        // 페이드 아웃
        elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // 발자국 제거
        Destroy(pawPrint);
    }

    // 특정 위치에 발자국을 생성하는 public 메서드
    public void CreatePawPrintAtPosition(Vector3 worldPosition)
    {
        if (pawPrintPrefab == null) return;

        GameObject pawPrint = Instantiate(pawPrintPrefab, worldPosition, Quaternion.identity);
        pawPrint.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-30f, 30f));
        StartCoroutine(AnimatePawPrint(pawPrint));
    }
}