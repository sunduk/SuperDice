using UnityEngine;

public class ShakeDetector : MonoBehaviour
{
    public float shakeThreshold = 2.0f; // 흔들림의 감지 임계값
    private Vector3 lastAcceleration;
    private float shakeTime = 0.5f; // 흔들림이 지속되는 시간
    private float shakeTimer;

    // 흔들림 이벤트가 발생했을 때 호출될 함수
    public delegate void OnShakeEvent();
    public static event OnShakeEvent OnShake;

    void Start()
    {
        lastAcceleration = Input.acceleration;
    }

    void Update()
    {
        // 가속도 변화량을 계산
        Vector3 accelerationChange = Input.acceleration - lastAcceleration;
        float accelerationMagnitude = accelerationChange.magnitude;

        // 가속도의 변화가 임계값을 넘으면 흔들림으로 감지
        if (accelerationMagnitude > shakeThreshold)
        {
            if (OnShake != null)
            {
                OnShake.Invoke(); // 흔들림 이벤트 호출
            }
            shakeTimer = shakeTime; // 흔들림 타이머 초기화
        }

        // 흔들림 이벤트가 끝난 후 타이머를 감소시켜 지속시간을 체크
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
        }

        lastAcceleration = Input.acceleration; // 가속도 값 갱신
    }
}
