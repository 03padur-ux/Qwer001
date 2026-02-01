using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float rotationSpeed = 10f;

    CharacterController controller;
    Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        // 로아 스타일은 마우스 커서가 보여야 하므로 커서 잠금을 하지 않습니다.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        // 1. 키보드 입력 (WASD)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // 2. 이동 방향 계산 (카메라와 상관없이 화면 위쪽이 북쪽인 로아 방식)
        Vector3 inputDir = new Vector3(horizontal, 0f, vertical).normalized;

        // 3. 움직임이 있을 때만 실행
        if (inputDir.magnitude >= 0.1f)
        {
            // 쉬프트 누르면 달리기
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

            // 캐릭터가 이동 방향을 부드럽게 바라보게 함
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 실제 이동
            controller.Move(inputDir * currentSpeed * Time.deltaTime);

            // 애니메이션 파라미터 전달 (0.5는 걷기, 1.0은 뛰기)
            float animSpeed = (currentSpeed == runSpeed) ? 1f : 0.5f;
            anim.SetFloat("Speed", animSpeed);
        }
        else
        {
            // 움직임이 없으면 대기 상태
            anim.SetFloat("Speed", 0f);
        }

        // 4. 공격 (마우스 왼쪽 클릭)
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
        }
    }
}