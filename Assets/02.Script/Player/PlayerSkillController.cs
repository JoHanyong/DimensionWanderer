using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private CharacterStats stats;

    [Header("Skill Slots")]
    [SerializeField] private SkillSlot skill_Q;

    [SerializeField] private SkillSlot skill_R;

    private void Awake()
    {
        if(stats == null)
        {
            stats = GetComponent<CharacterStats>();
        }
    }
    private void Update()
    {
        HandleSkillInput();
    }
    private void HandleSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryUseSkill(skill_Q);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            TryUseSkill(skill_R);
        }
    }
    private void TryUseSkill(SkillSlot slot)
    {
        // 슬롯이 비어 있으면 종료
        if (slot == null)
        {
            return;
        }

        slot.Use(stats);
    }
}
