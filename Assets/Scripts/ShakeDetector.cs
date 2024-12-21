using UnityEngine;

public class ShakeDetector : MonoBehaviour
{
    public float shakeThreshold = 2.0f; // ��鸲�� ���� �Ӱ谪
    private Vector3 lastAcceleration;
    private float shakeTime = 0.5f; // ��鸲�� ���ӵǴ� �ð�
    private float shakeTimer;

    // ��鸲 �̺�Ʈ�� �߻����� �� ȣ��� �Լ�
    public delegate void OnShakeEvent();
    public static event OnShakeEvent OnShake;

    void Start()
    {
        lastAcceleration = Input.acceleration;
    }

    void Update()
    {
        // ���ӵ� ��ȭ���� ���
        Vector3 accelerationChange = Input.acceleration - lastAcceleration;
        float accelerationMagnitude = accelerationChange.magnitude;

        // ���ӵ��� ��ȭ�� �Ӱ谪�� ������ ��鸲���� ����
        if (accelerationMagnitude > shakeThreshold)
        {
            if (OnShake != null)
            {
                OnShake.Invoke(); // ��鸲 �̺�Ʈ ȣ��
            }
            shakeTimer = shakeTime; // ��鸲 Ÿ�̸� �ʱ�ȭ
        }

        // ��鸲 �̺�Ʈ�� ���� �� Ÿ�̸Ӹ� ���ҽ��� ���ӽð��� üũ
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
        }

        lastAcceleration = Input.acceleration; // ���ӵ� �� ����
    }
}
