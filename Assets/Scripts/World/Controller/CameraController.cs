using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerChasingRoot;
    public Transform playerCamera;


    [Tooltip("Default = 150")]
    [Range(50, 500)]
    public float speed = 150.0f;

    [Tooltip("Default = 150")]
    [Range(50, 500)]
    public float zoom_speed = 150.0f;

    float xmove;
    float ymove;

    public bool cinemachine;

    void Start()
    {
        distZ = -6.5f;

        xmove = transform.localRotation.eulerAngles.x;
        ymove = transform.localRotation.eulerAngles.y;

        StartCoroutine(Init_CameraMotion());
    }



    IEnumerator Init_CameraMotion()
    {
        float count = 0;
        while (count < 1)
        {
            count += (Time.deltaTime / 3);
            transform.rotation = Quaternion.Euler(Mathf.Lerp(0, 30, count), 0, 0);
            playerCamera.localPosition = new Vector3(0, 0, Mathf.Lerp(-2, -6.5f, count));
            yield return null;
        }

        Debug.Log("오프닝끝");
        xmove = transform.localRotation.eulerAngles.x;
        ymove = transform.localRotation.eulerAngles.y;
    }

    void Update()
    {

        if (Input.GetMouseButton(1))
        {
            xmove += -Input.GetAxis("Mouse Y") * speed * Time.deltaTime;
            ymove += Input.GetAxis("Mouse X") * speed * Time.deltaTime;

            xmove = Mathf.Clamp(xmove, -60, 60);

            Quaternion rot = Quaternion.Euler(xmove, ymove, 0);
            transform.rotation = rot;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float posZ = playerCamera.localPosition.z + Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoom_speed;
            posZ = Mathf.Clamp(posZ, -10, -2);

            playerCamera.localPosition = new Vector3(0, 0, posZ);
            distZ = posZ;
        }
    }

    private void LateUpdate()
    {
        if (playerChasingRoot == null)
        {
            return;
        }
        transform.position = playerChasingRoot.position;


        Debug.DrawRay(transform.position, Vector3.Normalize(playerCamera.position - transform.position) * -distZ, Color.red, 2);
        MoveBlocker(-distZ);


        if (!cinemachine)
        {
            vcam1.transform.position = playerCamera.position;
            vcam1.transform.rotation = playerCamera.rotation;
        }
    }

    public float distZ;

    void MoveBlocker(float offset)
    {
        RaycastHit hit;

        Ray r = new Ray(transform.position, Vector3.Normalize(playerCamera.position - transform.position));

        if (Physics.Raycast(r, out hit, offset, LayerMask.GetMask("Blocker")))
        {
            //Debug.Log(hit.collider);

            float dist = (hit.point - transform.position).magnitude * 0.9f;
            //Debug.Log(dist);

            float posZ = Mathf.Clamp(-dist, -offset, -2);
            playerCamera.localPosition = new Vector3(0, 0, posZ);
        }
    }


    public GameObject vcam1;
    public GameObject vcam2;



    public void ChangeVcam2()
    {
        cinemachine = true;
        vcam2.SetActive(true);
    }
    public void ResetVcam()
    {
        StartCoroutine(Vcam1());
    }
    IEnumerator Vcam1()
    {
        vcam2.SetActive(false);
        yield return new WaitForSeconds(1);
        cinemachine = false;
    }
}
