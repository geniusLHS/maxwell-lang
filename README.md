# maxwell-lang

maxwell programming language

```
∇²D + ∇²B + ∇·D + ∇×D + ∇²D = 0
∇·H = 0
∇²D + ∇·E - ∇×D - ∇·E + ∇×D + ∇²E + ∇·D + ∇²D = 0
∇²H = ∂B/∂t
∇²D + ∇·E + ∇×D + ∇×D + ∇²D = 0
∇·H = 0
∇²D + ∇·D + ∇×D + ∇²B + ∇²D = 0
∇²H = ∂B/∂t
∇²D + ∇·D + ∇·E + ∇·D + ∇×D = 0

∇²D = 0
∇·H = 0
∇²D + ∇·E + ∇×D + ∇²D = 0
∇²H = ∂B/∂t
∇²D + ∇·B = 0
```

# 기본 문법

1. 알파벳은 `E, D, B, H`만 사용되며, 각각의 알파벳은 기호 `∇· ∇× ∇² ∂?/∂t` 와 조합하여 명령어가 된다. 이 때 각각의 명령어는 안에서 띄어쓰면 안된다. (예를 들어 `∇× E` 와 같이 띄어쓰면 안됨.) 부호(`+, -`)는 띄어써도 된다.
2. 저장 공간은 4바이트 정수(int)로 이루어진 10000개의 배열로 이루어져 있고, 포인터 두개가 존재한다.
   <br>
   처음에 pointer1은 0번 자리를, pointer2는 1번 자리를 가리킨다. 0번자리의 초깃값은 1이고, 나머지는 모두 0이다.
3. 명령어가 있는 줄에는 반드시 등호(`=`)가 정확히 하나 있어야 한다.
4. `monopole` 은 한줄 주석을 의미한다.

# 기호(전자기장)의 사용 방법

## 전기장 `E` : 데이터(포인터가 가리키는 값) 관련 명령어

```
∇·E    : *ptr1 <- *ptr1 + *ptr2   ; pointer1이 가리키는 값을 pointer2가 가리키는 값만큼 증가
-∇·E   : *ptr1 <- *ptr1 - *ptr2
∇×E   : *ptr1 <- *ptr1 * *ptr2
-∇×E  : *ptr1 <- *ptr1 / *ptr2
∇²E    : *ptr1 <-> *ptr2          ; pointer1이 가리키는 값과 pointer2가 가리키는 값을 서로 교환(swap)
```

## 전기변위장 `D` : 포인터 관련 명령어

```
∇·D    : ptr1 <- ptr1 + 1   ; pointer1을 1만큼 증가
-∇·D   : ptr1 <- ptr1 - 1
∇×D   : ptr2 <- ptr2 + 1
-∇×D  : ptr2 <- ptr2 - 1
∇²D    : ptr1 <-> ptr2
```

## 자기장 `B` : 입출력 관련 명령어

```
∇·B    : *ptr1를 출력(숫자)
∇×B   : *ptr1를 출력(문자 ; ASCII 코드)
∇²B    : 콘솔에서 숫자를 입력받아 *ptr1 에 저장
```

## 자화력 `H` : 반복문 관련 명령어

h 또한 포인터이다. 처음에 0으로 초기화되어 있다.

```
∇·H    : 이 명령어가 적힌 줄의 번째수를 *h에 저장.
∇×H   : h <- h + 1
-∇×H  : h <- h - 1
∇²H    : *h 번째 줄로 이동.
```

# 이외 중요 문법

## = : 조건문 영역 구분

명령어가 적힌 모든 코드는 '`=`' 가 정확히 하나 있어야 한다. 등호의 왼쪽은 명령문 영역, 오른쪽은 조건문 영역이 된다. 조건문 영역의 모든 조건이 참이어야 명령문 영역이 실행된다.

```
∂E/∂t : *ptr1 = *ptr2 ; *ptr1과 *ptr2이 같으면 참, 다르면 거짓.
∂D/∂t : ptr1 = ptr2
∂B/∂t : *ptr1>=0 : *ptr1이 0 이상이면 참, 다르면 거짓.
∂H/∂t : *h가 정의되었으면 참, 아니면 거짓.
0       : 참
```

만약 조건문 앞에 `-`가 붙으면, 참 거짓이 반대로 바뀐다.(ex. `- ∂E/∂t` 은 `*ptr1`과 `*ptr2`이 같으면 거짓, 다르면 참.)<br>
모든 문장에는 등호가 정확히 1개 있어야 하므로, 만약 조건문을 적을 필요가 없다면 다음과 같이 오른쪽에 `" = 0 "` 을 적어 무조건 실행되도록 할 수 있다.
`∇×D = 0`

## + : 명령문 또는 조건문 결합

한 줄에 여러개의 명령문 또는 조건문을 적을 수 있다. 예를 들어 다음과 같은 코드가 있다.
<br>
`∇²D + ∇·E - ∇×D = ∂B/∂t - ∂D/∂t`
<br>
이는 다음과 같이 해석된다.
<br>
`만약 ∂B/∂t이 참이고, ∂D/∂t이 거짓이라면, 다음 명령어 모음을 순서대로 실행한다 : 명령어 ∇²D, 명령어 ∇·E, 명령어 - ∇×D`
