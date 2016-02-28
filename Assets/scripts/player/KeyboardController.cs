//
//Filename: KeyboardCameraControl.cs
//

using System;
using UnityEngine;

[AddComponentMenu("Body-control/Keyboard")]//P.s Эта штука прямо будет созадвать новые элементы в списке либо юзать существующие. Чтобы юзать свои можно префикс зсдат что нибудь типа #_Camera и т.д чтобы они в переди были
public class KeyboardController : MonoBehaviour
{
    // Keyboard axes buttons in the same order as Unity
    public enum KeyboardAxis { Horizontal = 0, Vertical = 1, None = 3 }

    [System.Serializable]
    // Handles left modifiers keys (Alt, Ctrl, Shift)
    public class Modifiers
    {
        public bool leftAlt;
        public bool leftControl;
        public bool leftShift;

        public bool checkModifiers()
        {
            return (!leftAlt ^ Input.GetKey(KeyCode.LeftAlt)) &&
                (!leftControl ^ Input.GetKey(KeyCode.LeftControl)) &&
                (!leftShift ^ Input.GetKey(KeyCode.LeftShift));
        }
    }

    [System.Serializable]
    // Handles common parameters for translations and rotations
    public class KeyboardControlConfiguration
    {
        //Параметры для разных компонентов скрипта, YAW, PITCH, ROLL, ETC...
        public bool activate;
        public KeyboardAxis keyboardAxis;
        public Modifiers modifiers;
        public float sensitivity;

        public bool isActivated()
        {
            return activate && keyboardAxis != KeyboardAxis.None && modifiers.checkModifiers();
        }
    }

    // Vertical translation default configuration
    public KeyboardControlConfiguration verticalTranslation = new KeyboardControlConfiguration { keyboardAxis = KeyboardAxis.Vertical, modifiers = new Modifiers { leftControl = true }, sensitivity = 0.5F };

    // Horizontal translation default configuration
    public KeyboardControlConfiguration horizontalTranslation = new KeyboardControlConfiguration { keyboardAxis = KeyboardAxis.Horizontal, sensitivity = 0.5F };

    // Default unity names for keyboard axes
    public string keyboardHorizontalAxisName = "Horizontal";
    public string keyboardVerticalAxisName = "Vertical";

    private string[] keyboardAxesNames;

    public float speed = 0;

    private Rigidbody rigBody;

    void Start()
    {
        rigBody = GetComponent<Rigidbody>();

        //какие оси будет юзать клавиатура
        keyboardAxesNames = new string[] { keyboardHorizontalAxisName, keyboardVerticalAxisName };
    }

    public Bullet shot;
    public Transform shotSpawner;
    public float fireRate;

    private float nextFire;

    void Update()
    {
        bool shootPressCondition = Input.GetButton("Fire1"); //|| Input.GetButton("space");
        if (shootPressCondition  && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            Bullet currentBulltet = (Bullet) Instantiate(shot, shotSpawner.position, shotSpawner.rotation);
            currentBulltet.setSpawnerSpeed(rigBody.velocity);
        }
    }

    private bool isMouseDown = false;
    private Vector3 currentMousePos;
    private Vector3 downMousePos;

    private void processMouse()
    {
        //не удобно конечно без эвентов но отслеживать нажат или нет мышь к примеру можно так
        //для такого прцоессинга надо сапорт класс

        bool isRightButtonDown = Input.GetMouseButtonDown(1);
        bool isRightButtonUp = Input.GetMouseButtonUp(1);

        if (isRightButtonDown)
        {
            downMousePos = Input.mousePosition;
            isMouseDown = true;
        }
        else if (isRightButtonUp)
            isMouseDown = false;

        currentMousePos = Input.mousePosition;
       
    }

    // LateUpdate  is called once per frame after all Update are done
    void LateUpdate()
    {
        //Простое движение без ускорения без физики и т.д
        processMouse();

        if (isMouseDown)
        {
            //Очень просто получаем нормаль мыши на экране по X и если ндао по Y и умножаем на степень поворота
            float mouseDelta = (downMousePos.x - currentMousePos.x);

            int screenWidth = Screen.width;
            int halfScreenWidth = screenWidth / 2;
            float mouseX = (Input.mousePosition.x - halfScreenWidth) / halfScreenWidth;

            if (mouseX != 0)
            {
                float rotationX = 5 * mouseX;
                transform.Rotate(0, rotationX, 0);
                //transform.rotation = Quaternion.AngleAxis(mouseDelta, Vector3.up);
            }
        }

        /*if (yaw.isActivated())
        {
            float rotationX = Input.GetAxis(keyboardAxesNames[(int)yaw.keyboardAxis]) * yaw.sensitivity;
            transform.Rotate(0, rotationX, 0);
        }
        if (pitch.isActivated())
        {
            float rotationY = Input.GetAxis(keyboardAxesNames[(int)pitch.keyboardAxis]) * pitch.sensitivity;
            transform.Rotate(-rotationY, 0, 0);
        }
        if (roll.isActivated())
        {
            float rotationZ = Input.GetAxis(keyboardAxesNames[(int)roll.keyboardAxis]) * roll.sensitivity;
            transform.Rotate(0, 0, rotationZ);
        }*/
        /*if (verticalTranslation.isActivated())
        {
            float translateY = Input.GetAxis(keyboardAxesNames[(int)verticalTranslation.keyboardAxis]) * verticalTranslation.sensitivity;
            transform.Translate(0, translateY, 0);
        }
        if (horizontalTranslation.isActivated())
        {
            float translateX = Input.GetAxis(keyboardAxesNames[(int)horizontalTranslation.keyboardAxis]) * horizontalTranslation.sensitivity;
            transform.Translate(translateX, 0, 0);
        }
        if (depthTranslation.isActivated())
        {
            float translateZ = Input.GetAxis(keyboardAxesNames[(int)depthTranslation.keyboardAxis]) * depthTranslation.sensitivity;
            transform.Translate(0, 0, translateZ);
        }*/


        Vector3 moveingForwardVector = new Vector3(0, 0, 0);
        Vector3 moveingSideVector = new Vector3(0, 0, 0);

        float forwardMoveing = Input.GetAxis(keyboardAxesNames[(int)verticalTranslation.keyboardAxis]) * verticalTranslation.sensitivity;
        float sidewardMoveing = Input.GetAxis(keyboardAxesNames[(int)horizontalTranslation.keyboardAxis]) * horizontalTranslation.sensitivity;



        moveingForwardVector = transform.forward * forwardMoveing * speed;
        moveingSideVector = transform.right * sidewardMoveing * speed;


        rigBody.velocity = rigBody.velocity + moveingForwardVector + moveingSideVector;
        rigBody.velocity = rigBody.velocity * 0.95f;
    }
}