using UnityEngine;

public class EnemyState : MonoBehaviour
{
   public enum enemyState
    {
        Idle, //대기
        Chase, //추적
        Attack,//공격
        Dead //사망 
    }
}
