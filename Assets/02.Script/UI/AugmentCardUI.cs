using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AugmentCardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText; //이름
    [SerializeField] private TMP_Text descText; //설명
    [SerializeField] private TMP_Text tierText; //등급(브론즈, 실버, 골드)
    [SerializeField] private Button selectButton; //선택 버튼
    [SerializeField] private Button rerollButton; //리롤 버튼

    private int cardIndex; //카드가 몇 번째인지
    private AugmentSelectionUI augmentUI;
    
    //카드 초기 세팅
    public void Setup(AugmentData data, int index, AugmentSelectionUI ui, bool canReroll)
    {
        cardIndex = index; //몇번 째 카드인지 저장
        augmentUI = ui; //전체 UI 저장

        if(data != null) //데이터 있으면 텍스트 표시
        {
            if(nameText != null)
            {
                nameText.text = data.augmentName; //이름 표시
            }
            if(descText != null)
            {
                descText.text = data.description; // 설명 표시
            }
            if(tierText != null)
            {
                tierText.text = data.tier.ToString(); //등급표시
            }
            
        }
        if (selectButton != null)
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(OnClickSelect); //클릭 시 함수 실행
        }
        if (rerollButton != null)
        {
            rerollButton.onClick.RemoveAllListeners();
            rerollButton.onClick.AddListener(OnClickReroll);

            rerollButton.interactable = canReroll; //리롤 가능 여부
        }

    }
    private void OnClickReroll() //리롤 눌렀을 때
    {
        if(augmentUI != null)
        {
            augmentUI.RerollCard(cardIndex); // 카드 리롤 전체 증강 UI에 전달
        }
    }
    private void OnClickSelect()
    {
        if (augmentUI != null)
        {
            augmentUI.SelectCard(cardIndex); // 카드 선택 전체 증강 UI에 전달
        }
    }
    
}
