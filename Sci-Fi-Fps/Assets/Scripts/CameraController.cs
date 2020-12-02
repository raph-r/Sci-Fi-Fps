using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 _cameraOriginalPosition;
    private Vector3 _cameraPositionWhileAiming;

    // Start is called before the first frame update
    void Start()
    {
        this._cameraPositionWhileAiming = GameObject.FindGameObjectWithTag(Constant.TAG_CAMERA_POSITION).transform.position;
        this._cameraOriginalPosition = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Garante que a Coroutine não seja chamada em uma situação indevida
    public void changeCameraPosition(bool isAimingOrShooting)
    {
        if (isAimingOrShooting)
        {
            
            if (this.transform.localPosition != this._cameraPositionWhileAiming)
            {
                StartCoroutine(
                    this.smoothTransactionBetweenCameraPositions(this.transform.localPosition, this._cameraPositionWhileAiming)
                );
            }
        }
        else
        {
            if (this.transform.localPosition != this._cameraOriginalPosition)
            {
                StartCoroutine(
                    this.smoothTransactionBetweenCameraPositions(this.transform.localPosition, this._cameraOriginalPosition)
                );
            }
        }
    }

    // efetua uma transicao suave entre as posições de cameras
    private IEnumerator smoothTransactionBetweenCameraPositions(Vector3 initialLocalPosition, Vector3 finalLocalPosition)
    {
        float elapseTime = 0.0f;
        while(elapseTime < Constant.DURATION_OF_CHANGING_CAMERA_POSITION)
        {
            this.transform.localPosition = Vector3.Lerp(initialLocalPosition, finalLocalPosition, (elapseTime / Constant.DURATION_OF_CHANGING_CAMERA_POSITION));
            elapseTime += Time.deltaTime;
            yield return null;  
        }   
        this.transform.localPosition = finalLocalPosition;
    }
}
