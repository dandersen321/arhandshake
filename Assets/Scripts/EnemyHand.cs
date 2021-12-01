using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class EnemyHand : MonoBehaviour
{
    private ManoGestureContinuous grab;
    private ManoGestureContinuous pinch;
    private ManoGestureTrigger click;

    private Transform originalParentTransform;
    private Vector3 originalLocalPosition;
    private Quaternion originalRotation;

    private GameObject playerHand;

    private bool handshakeStarted = false;
    private bool grabStarted = false;
    
    public float grabCooldown = 0;


    [SerializeField]
    private Material[] arCubeMaterial;
    [SerializeField]
    private GameObject smallCube;

    private string handTag = "Player";
    private Renderer cubeRenderer;

    private TMPro.TextMeshProUGUI textBox;

    private int shakeCount = 0;
    private bool shakedToTop = false;
    private bool shakedToBottom = false;
    private float startY = 0f;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        grab = ManoGestureContinuous.CLOSED_HAND_GESTURE;
        pinch = ManoGestureContinuous.HOLD_GESTURE;
        click = ManoGestureTrigger.CLICK;
        cubeRenderer = GetComponent<Renderer>();
        originalParentTransform = this.transform.parent;
        originalLocalPosition = this.transform.localPosition;
        originalRotation = this.transform.rotation;
        //cubeRenderer.sharedMaterial = Material.Create("");
        //cubeRenderer.material = arCubeMaterial[0];
        textBox = GameObject.FindGameObjectWithTag("DisplayText").GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void updateTextBox() {
        textBox.text = "Shakes: " + shakeCount + "\nShakeToBottom: " + shakedToBottom + "\nShakeToTop: " + shakedToTop;
    }

    private void Update() {
        grabCooldown -= Time.deltaTime;
        if(handshakeStarted) {
            transform.parent = playerHand.transform;
            if(ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_trigger == ManoGestureTrigger.RELEASE_GESTURE || ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_class == ManoClass.NO_HAND) {
                handshakeStarted = false;
                grabStarted = false;
                transform.parent = originalParentTransform;
                transform.position = transform.parent.position;
                transform.localPosition = originalLocalPosition;
                transform.rotation = transform.parent.rotation;
                cubeRenderer.sharedMaterial = arCubeMaterial[0];
                grabCooldown = 1;
            } else {
                float shakeHeight = 0.1f;
                if(playerHand.transform.position.y - startY > shakeHeight) {
                    shakedToTop = true;
                    updateTextBox();
                } else if ((playerHand.transform.position.y - startY) * -1 > shakeHeight) {
                    shakedToBottom = true;
                    updateTextBox();
                }

                if(shakedToBottom && shakedToTop) {
                    shakeCount +=1;
                    shakedToBottom = false;
                    shakedToTop = false;
                    updateTextBox();
                }
            }
            // if(ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_continuous == grab) {
            //     transform.parent = playerHand.transform;
            //     //transform.position = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.palm_center;
            //     // tracking = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
            //     // currentPosition = Camera.main.ViewportToWorldPoint(new Vector3(tracking.palm_center.x, tracking.palm_center.y, tracking.depth_estimation));
            //     // transform.position = currentPosition;
            // } else {
            //     //transform.parent = originalParentTransform;
            //     //transform.position = originalPosition;
                // transform.parent = originalParentTransform;
                // transform.position = originalPosition;
            //     handshakeStarted = false;
            // }
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other">The collider that stays</param>
    private void OnTriggerStay(Collider other)
    {
        MoveWhenGrab(other);
        // RotateWhenHolding(other);
        // SpawnWhenClicking(other);
    }

    /// <summary>
    /// If grab is performed while hand collider is in the cube.
    /// The cube will follow the hand.
    /// </summary>
    private void MoveWhenGrab(Collider other)
    {

            if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_continuous == grab && !handshakeStarted && grabCooldown <= 0)
            {
                grabStarted = true;
                if(grabStarted) {
                    handshakeStarted = true;
                    playerHand = other.gameObject;
                    cubeRenderer.sharedMaterial = arCubeMaterial[2];
                    startY = playerHand.transform.position.y;
                }
                // handshakeStarted = true;
                // playerHand = other.gameObject;
                // cubeRenderer.sharedMaterial = arCubeMaterial[2];
                //transform.parent = other.gameObject.transform;
            }
        
        // if(!handshakeStarted) {
        //     cubeRenderer.sharedMaterial = arCubeMaterial[1];
        
            // if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_continuous == grab)
            // {
            //     // handshakeStarted = true;
            //     // playerHand = other.gameObject;
            //     // cubeRenderer.sharedMaterial = arCubeMaterial[2];
            //     //transform.parent = other.gameObject.transform;
            // }
        // }

        // else
        // {
            // transform.parent = originalParentTransform;
            // transform.position = originalPosition;
        // }
    }

    /// <summary>
    /// If pinch is performed while hand collider is in the cube.
    /// The cube will start rotate.
    /// </summary>
    private void RotateWhenHolding(Collider other)
    {
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_continuous == pinch)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 50, Space.World);
        }
    }

    /// <summary>
    /// If pick is performed while hand collider is in the cube.
    /// The cube will follow the hand.
    /// </summary>
    private void SpawnWhenClicking(Collider other)
    {
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_trigger == click)
        {
            Instantiate(smallCube, new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 1.5f, transform.position.z), Quaternion.identity);
        }
    }

    /// <summary>
    /// Vibrate when hand collider enters the cube.
    /// </summary>
    /// <param name="other">The collider that enters</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == handTag)
        {
            //cubeRenderer.material.SetColor("green", Color.green);
            cubeRenderer.sharedMaterial = arCubeMaterial[1];
            Handheld.Vibrate();
            // if(grabStarted) {
            //     handshakeStarted = true;
            //     playerHand = other.gameObject;
            //     cubeRenderer.sharedMaterial = arCubeMaterial[2];
            // }
        }
    }

    /// <summary>
    /// Change material when exit the cube
    /// </summary>
    /// <param name="other">The collider that exits</param>
    private void OnTriggerExit(Collider other)
    {
        cubeRenderer.sharedMaterial = arCubeMaterial[0];
        //cubeRenderer.material.SetColor("red", Color.red);
    }
}