using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerInputReader : MonoBehaviour
{
    [Header("Input Action")] //입력 액션
    [SerializeField] private InputAction moveAction;  //이동 액션
    [SerializeField] private InputAction jumpAction;  //점프 액션
    [SerializeField] private InputAction dashAction;  //대쉬 액션
    [SerializeField] private InputAction attackAction; //공격 액션

    public float MoveX {  get; private set; } //좌우 이동 입력값 읽는 함수
    public bool JumpPressedThisFrame { get; private set; } //해당 키를 프레임에 눌렀는지
    public bool DashPressedThisFrame { get; private set; }
    public bool AttackPressedThisFrame { get; private set; }

    private void OnEnable() //오브젝트 활성화
    {
        moveAction.Enable();
        jumpAction.Enable();
        dashAction.Enable();
        attackAction.Enable();
    }
    private void OnDisable() //오브젝트 비활성화
    {
        moveAction.Disable();
        jumpAction.Disable();
        dashAction.Disable(); 
        attackAction.Disable();
    }
    private void Update()
    {
        MoveX = moveAction.ReadValue<Vector2>().x; //플랫폼 게임이라 좌우만 사용해서X좌표만 씀
        JumpPressedThisFrame = jumpAction.WasPressedThisFrame(); //눌렀는지 확인
        DashPressedThisFrame = dashAction.WasPressedThisFrame();
        AttackPressedThisFrame = attackAction.WasPressedThisFrame();
    }
}
