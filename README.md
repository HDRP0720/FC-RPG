# Unity RPG Tutorial
패스트 캠퍼스 액션 RPG 클론입니다.
* Unity Version: 2021.3.30f1
* IDE Rider를 위한 RiderFlow 에셋 사용

## 캐릭터 이동 구현
* Rigidbody를 이용한 구현
  * Jump & Dash
    ```math
    \displaylines{
    jumpVelocity = \sqrt{(-2g * jumpHeight)}\\
    dashVelocity_{xz} = \vec{T_{xz}} \times dashDistance \times log{(t * drag  + 1)} \times { {1 \over t}}
    }
    ```
* Character Controller를 이용한 구현
* Navmesh agent를 이용한 구현

## Recources From
* mixamo character
  * Y bot
* mixamo animation
  * Standing short idle
  * Sad idle variation 1
  * Breathing idle
  * Walking with a swagger