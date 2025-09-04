
// Fruit.cs - 개별 과일 스크립트
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private FruitData fruitData;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isSliced = false;
    private bool isMissed = false;
    private float rotationSpeed; // 개별 회전 속도

    [Header("Effects")]
    public ParticleSystem sliceParticles;
    public ParticleSystem bombParticles;

    [Header("Rotation")]
    public float minRotationSpeed = 50f;  // 최소 회전 속도
    public float maxRotationSpeed = 200f; // 최대 회전 속도
    public bool IsSliced => isSliced;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(FruitData data)
    {
        fruitData = data;
        spriteRenderer.sprite = data.fruitSprite;
        isSliced = false;
        isMissed = false;

        // 랜덤 회전 속도 설정 (방향도 랜덤)
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        if (Random.Range(0f, 1f) > 0.5f)
        {
            rotationSpeed = -rotationSpeed; // 50% 확률로 반대 방향
        }

        // 아웃라인 효과 설정 - 인스턴스에서만 실행
        if (Application.isPlaying) // 런타임에만 실행
        {
            SetOutlineEffect();
        }
    }

    private void SetOutlineEffect()
    {
        // 프리팹 인스턴스인지 확인
        if (gameObject.scene.name == null) // 프리팹 에셋이면 실행 안함
        {
            Debug.LogWarning("프리팹 에셋에서는 아웃라인을 생성할 수 없습니다.");
            return;
        }

        // 간단한 색상 틴트로 구별
        if (fruitData.isBomb)
        {
            spriteRenderer.color = new Color(1f, 0.7f, 0.7f, 1f); // 빨간 틴트
        }
        else if (fruitData.isSpecial)
        {
            spriteRenderer.color = new Color(1f, 1f, 0.7f, 1f); // 노란 틴트
        }
        else
        {
            spriteRenderer.color = Color.white;
        }

        // 아웃라인 자식 오브젝트 생성 (런타임에서만)
        CreateSimpleOutline();
    }

    private void CreateSimpleOutline()
    {
        // 이미 아웃라인이 있는지 체크
        Transform existingOutline = transform.Find("Outline");
        if (existingOutline != null)
        {
            DestroyImmediate(existingOutline.gameObject);
        }

        // 아웃라인용 자식 오브젝트 생성
        GameObject outlineObj = new GameObject("Outline");
        outlineObj.transform.SetParent(transform, false); // false로 설정
        outlineObj.transform.localPosition = Vector3.zero;
        outlineObj.transform.localScale = Vector3.one * 1.15f; // 살짝 크게

        SpriteRenderer outlineRenderer = outlineObj.AddComponent<SpriteRenderer>();
        outlineRenderer.sprite = spriteRenderer.sprite;
        outlineRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;

        if (fruitData.isBomb)
        {
            outlineRenderer.color = new Color(1f, 0f, 0f, 0.5f); // 반투명 빨강
        }
        else if (fruitData.isSpecial)
        {
            outlineRenderer.color = new Color(1f, 1f, 0f, 0.5f); // 반투명 노랑
        }
        else
        {
            outlineRenderer.color = new Color(1f, 1f, 1f, 0.3f); // 반투명 하양
        }
    }

    private void Update()
    {
        // 회전 (각 과일마다 다른 속도로 휘리리릭!)
        if (!isSliced)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }

        // 화면 밖으로 나가면 제거 (카메라 기준으로 동적 계산)
        Camera cam = Camera.main;
        if (cam != null)
        {
            float cameraBottom = cam.transform.position.y - cam.orthographicSize - 2f;
            if (transform.position.y < cameraBottom && !isMissed)
            {
                OnMissed();
            }
        }
    }

    private void OnMouseDown()
    {
        // 이제 SlashController에서 처리하므로 제거
        // if (isSliced) return;
        // SliceFruit();
    }

    public void SliceFruit()
    {
        if (isSliced) return;
        isSliced = true;

        if (fruitData.isBomb)
        {
            // 폭탄 터짐
            ExplodeBomb();
        }
        else
        {
            // 과일 자르기
            SliceNormalFruit();
        }
    }

    private void SliceNormalFruit()
    {
        // 점수 추가
        FruitGameManager.Instance.AddScore(fruitData.baseScore, transform.position);
        FruitGameManager.Instance.AddCombo();

        // 파티클 효과
        CreateSliceParticles();

        // 조각들 생성
        CreateFruitPieces();

        // 스포너에서 제거
        FindObjectOfType<FruitSpawner>()?.RemoveFruit(gameObject);

        // 풀로 반환
        FruitPool.Instance.ReturnFruit(gameObject);
    }

    private void ExplodeBomb()
    {
        // 폭탄 효과
        FruitGameManager.Instance.OnBombHit();
        FruitGameManager.Instance.ResetCombo();

        // 폭발 파티클
        if (bombParticles != null)
        {
            bombParticles.transform.SetParent(null);
            bombParticles.Play();
            Destroy(bombParticles.gameObject, 2f);
        }

        FindObjectOfType<FruitSpawner>()?.RemoveFruit(gameObject);
        FruitPool.Instance.ReturnFruit(gameObject);
    }

    private void CreateSliceParticles()
    {
        if (sliceParticles != null)
        {
            var main = sliceParticles.main;
            main.startColor = fruitData.GetParticleColor();

            sliceParticles.transform.SetParent(null);
            sliceParticles.Play();
            Destroy(sliceParticles.gameObject, 2f);
        }
    }

    private void CreateFruitPieces()
    {
        if (fruitData.slicedSprites.Length >= 2)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject piece = new GameObject($"FruitPiece_{i}");
                piece.transform.position = transform.position;

                SpriteRenderer pieceRenderer = piece.AddComponent<SpriteRenderer>();
                pieceRenderer.sprite = fruitData.slicedSprites[i];

                Rigidbody2D pieceRb = piece.AddComponent<Rigidbody2D>();
                pieceRb.gravityScale = 2;
                pieceRb.AddForce(new Vector2(i == 0 ? -3f : 3f, Random.Range(2f, 5f)), ForceMode2D.Impulse);

                // 조각들도 휘릭휘릭 빠르게 회전!
                float pieceRotationSpeed = Random.Range(100f, 200f); // 매우 빠른 회전
                if (Random.Range(0f, 1f) > 0.5f)
                {
                    pieceRotationSpeed = -pieceRotationSpeed;
                }
                pieceRb.AddTorque(pieceRotationSpeed);

                // 조각 회전 컴포넌트 추가
                FruitPiece pieceScript = piece.AddComponent<FruitPiece>();
                pieceScript.rotationSpeed = pieceRotationSpeed;

                Destroy(piece, 3f);
            }
        }
    }

    private void OnMissed()
    {
        if (!fruitData.isBomb && !isSliced)
        {
            FruitGameManager.Instance.ResetCombo();
        }

        isMissed = true;
        FindObjectOfType<FruitSpawner>()?.RemoveFruit(gameObject);
        FruitPool.Instance.ReturnFruit(gameObject);
    }
}