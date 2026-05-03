using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkilSlotUI : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private SkillSlot skillSlot;

    [Header("UI")] 
    [SerializeField] private Image iconImage; //스킬 아이콘 

    [SerializeField] private Image cooldownOverlay; //스킬 쿨타임 덮개

    [SerializeField] private TMP_Text cooldownText; //쿨타임 숫자

    private void Update()
    {
        RefreshIcon();
    }

    private void RefreshIcon()
    {
        if(skillSlot == null)
        {
            return; 
        }
        
        float remain = skillSlot.GetCooldownRemaining();
        //현재 남은 스킬쿨타임 가져오기

        if(remain > 0f)
        {
            if(cooldownText  != null)
            {
                cooldownText.text = Mathf.Ceil(remain).ToString();
                //1.2초를 2초로 보이게 처리 
            }

            if(cooldownOverlay != null)
            {
                cooldownOverlay.enabled = true;
                //덮개 이미지 켜기

                cooldownOverlay.fillAmount = skillSlot.GetCooldownNormalized();
                //남은 쿨타임 비율만큼 이미지 채우고 1이면 가득참, 0되면 사라짐
            }
        }
        else
        {
            //쿹타임 끝나면 숫자 텍스트 비우기
            if (cooldownText != null)
            {
                cooldownText.text = "";
            }
            if (cooldownOverlay != null)
            {
                //덮개 이미지 끄기 + 채워진 양도 0으로 초기화
                cooldownOverlay.enabled = false;

                cooldownOverlay.fillAmount = 0f;
            }
        }
    }
}
