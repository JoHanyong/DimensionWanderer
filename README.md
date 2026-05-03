# Dimension Wanderer

2D 액션 로그라이크 스타일의 Unity 프로젝트입니다.
객체 지향 구조를 기반으로 유지보수가 쉬운 구조를 목표로 개발했습니다.

## 개발 환경
- Unity
- C#
- Visual Studio

- ## 주요 기능
- 플레이어 이동
- 점프
- 대쉬
- 패링 시스템
- 몬스터 FSM
- 스킬 시스템
- 증강 시스템
- UI 시스템

- ## 주요 구현 내용
### 플레이어
- 좌우 이동
- 점프
- 대쉬
- 공격

### 전투
- 패링
- 카운터 공격
- 원거리 스킬

### 몬스터 AI
- Idle
- Chase
- Attack
- Dead

 ## 레벨 시스템
- 몬스터 처치 시 경험치 획득
- 레벨업 기능
- 레벨 증가 시 능력치 상승
- 체력 회복 기능


## 증강 시스템
랜덤 선택형 증강 시스템 구현

예시:
- 공격력 증가
- 방어력 증가
- 치명타 확률 증가
- 체력 증가
- 스킬 강화

- ## 프로젝트 영상 / 실행 파일
빌드 파일 : https://drive.google.com/file/d/1m1Z5jpc8zWTRMzxDgD660oavajMRPJ44/view?usp=sharing
프로젝트 시연 영상 : https://drive.google.com/file/d/1qTyI_jq2ZhfkUiAzCw3gUquljI2VNQfq/view?usp=sharing

## 느낀 점
FSM 구조 설계와 UI 시스템 연동 과정에서 많은 어려움이 있었지만  
직접 문제를 해결하면서 Unity 구조 설계 능력을 키울 수 있었습니다. 그리고 UI를 다루는 실력이 많이 부족하다고 생각했습니다.
UI에서 버그가 생각보다 많았고, 씬 이동 중에 증강시스템과 UI쪽에 문제가 있었지만 잘 해결했습니다.
