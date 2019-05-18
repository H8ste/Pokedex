using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationX = 0F;
    float rotationY = 0F;

    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;

    private List<float> rotArrayY = new List<float>();
    float rotAverageY = 0F;

    public float frameCounter = 20;

    Quaternion originalRotation;

    public Transform collider;



    void Update()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            rotAverageY = 0f;
            rotAverageX = 0f;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;

            rotArrayY.Add(rotationY);
            rotArrayX.Add(rotationX);

            if (rotArrayY.Count >= frameCounter)
            {
                rotArrayY.RemoveAt(0);
            }
            if (rotArrayX.Count >= frameCounter)
            {
                rotArrayX.RemoveAt(0);
            }

            for (int j = 0; j < rotArrayY.Count; j++)
            {
                rotAverageY += rotArrayY[j];
            }
            for (int i = 0; i < rotArrayX.Count; i++)
            {
                rotAverageX += rotArrayX[i];
            }

            rotAverageY /= rotArrayY.Count;
            rotAverageX /= rotArrayX.Count;

            rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
            rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

            Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);

            Quaternion foundRotation = originalRotation * xQuaternion * yQuaternion;

            // collider.localRotation = new Quaternion(
            //     collider.localRotation.x, foundRotation.y, collider.localRotation.z, collider.localRotation.w
            // );
            // transform.parent.localRotation = new Quaternion(
            //     transform.parent.localRotation.x, foundRotation.y, transform.parent.localRotation.z, transform.parent.localRotation.w
            // );
            transform.localRotation = new Quaternion(
                foundRotation.x, foundRotation.y, foundRotation.z, foundRotation.w
            );
            // transform.localRotation = originalRotation * xQuaternion * yQuaternion;
            //parent y roation = this global y rotation
            // new Quaternion(
            //     originalRotation.x * xQuaternion.x * yQuaternion.x,
            //     originalRotation.y * xQuaternion.y * yQuaternion.y,
            //     originalRotation.z * xQuaternion.z * yQuaternion.z,
            //     originalRotation.w * xQuaternion.w * yQuaternion.w
            // );





        }
        else if (axes == RotationAxes.MouseX)
        {
            rotAverageX = 0f;

            rotationX += Input.GetAxis("Mouse X") * sensitivityX;

            rotArrayX.Add(rotationX);

            if (rotArrayX.Count >= frameCounter)
            {
                rotArrayX.RemoveAt(0);
            }
            for (int i = 0; i < rotArrayX.Count; i++)
            {
                rotAverageX += rotArrayX[i];
            }
            rotAverageX /= rotArrayX.Count;

            rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
            // transform.localRotation = originalRotation * xQuaternion;
            transform.localRotation = new Quaternion(
                originalRotation.x * xQuaternion.x,
                originalRotation.y * xQuaternion.y,
                originalRotation.z * xQuaternion.z,
                originalRotation.w * xQuaternion.w
            );
        }
        else
        {
            rotAverageY = 0f;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

            rotArrayY.Add(rotationY);

            if (rotArrayY.Count >= frameCounter)
            {
                rotArrayY.RemoveAt(0);
            }
            for (int j = 0; j < rotArrayY.Count; j++)
            {
                rotAverageY += rotArrayY[j];
            }
            rotAverageY /= rotArrayY.Count;

            rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);

            Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
            // transform.localRotation = originalRotation * yQuaternion;
            transform.localRotation = new Quaternion(
                originalRotation.x * yQuaternion.x,
                originalRotation.y * yQuaternion.y,
                originalRotation.z * yQuaternion.z,
                originalRotation.w * yQuaternion.w
            );
        }
    }

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
            rb.freezeRotation = true;
        originalRotation = transform.localRotation;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }
}
