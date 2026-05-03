using UnityEngine;

[System.Serializable]
public class SkillData : MonoBehaviour
{
    public string skillName; //스킬 이름
    public float cooldown = 3f; //스킬 쿨타임
    public int manaCost = 10; //마나 소모량
    public Sprite icon; //스킬 아이콘 
}
