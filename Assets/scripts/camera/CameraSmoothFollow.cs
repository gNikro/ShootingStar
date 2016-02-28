using UnityEngine;
using System.Collections;

public class CameraSmoothFollow : MonoBehaviour
{
    //Таргет 
    public Transform target;
    //Дистанция на которую камера отдалена
    public float distance = 20.0f;
    //Высота относительно таргета
    public float height = 5.0f;
    //Величина на которую будет происходить изменение для эфекта плавности
    public float heightDamping = 2.0f;

    public float lookAtHeight = 0.0f;

    //Риг боди таргета
    public Rigidbody parentRigidbody;

    public float rotationSnapTime = 0.3F;

    public float distanceSnapTime;
    public float distanceMultiplier;

    //Поинт в который смотри камера
    private Vector3 lookAtVector;

    private float usedDistance;

    float wantedRotationAngle;
    float wantedHeight;

    float currentRotationAngle;
    float currentHeight;

    Quaternion currentRotation;
    Vector3 wantedPosition;

    private float yVelocity = 0.0F;
    private float zVelocity = 0.0F;

    void Start()
    {
        lookAtVector = new Vector3(0, lookAtHeight, 0);
    }

    void LateUpdate()
    {
        //Берем высоту исходя из target.position и заданой высоты и поулчаем желаемую
        wantedHeight = target.position.y + height;
        //Далее берем текущую высоту камеры
        currentHeight = transform.position.y;

        //Желаемый угол, т.е перед таргетом смотрим
        wantedRotationAngle = target.eulerAngles.y;
        //Ну и текущий угол камеры
        currentRotationAngle = transform.eulerAngles.y;

        //Функция производит интерполяцю между текущим углом и желаемым по неким коофициентам
        currentRotationAngle = Mathf.SmoothDampAngle(currentRotationAngle, wantedRotationAngle, ref yVelocity, rotationSnapTime);

        //Текущая высота тоже как интерполяция между нужной и текущей высотой с неким коофициентом
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        wantedPosition = target.position;
        wantedPosition.y = currentHeight;

        //Вроде как то на сколько камера пройдет отностиельно вектора движения таргета
        usedDistance = Mathf.SmoothDampAngle(usedDistance, distance + (parentRigidbody.velocity.magnitude * distanceMultiplier), ref zVelocity, distanceSnapTime);

        wantedPosition += Quaternion.Euler(0, currentRotationAngle, 0) * new Vector3(0, 0, -usedDistance);

        transform.position = wantedPosition;

        //Смотрим в точку, позиция + делта в лукат векторе
        transform.LookAt(target.position + lookAtVector);

    }

}