using Unity.VisualScripting;
using UnityEngine;

public class Skill_Parry : MonoBehaviour, IskillBehaviour
{
    [Header("참조")]
    [SerializeField] private HealthPointManager health;
    //패링 상태 켜 줄 스크립트

    private void Awake()
    {
        if (health == null)
        {
            health = GetComponent<HealthPointManager>();
        }
    }
    public void UseSkill()
    {
        if (health != null)
        {
            health.StartParry();
            SoundManager.Instance.PlayParrySFX();
        }
    }
    
}
