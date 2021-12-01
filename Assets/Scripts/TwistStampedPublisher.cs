using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class TwistStampedPublisher : UnityPublisher<MessageTypes.Geometry.TwistStamped>
    {
        public Transform PublishedTransform;
        public string FrameId = "Unity";

        public HandSync handSync;

        private float lastXPos;
        private float lastYPos;
        private float lastZPos;
        private float lastXAng;
        private float lastYAng;
        private float lastZAng;

        public float deltaX;
        public int counter;
        public int goal;
        public float scalePos;
        public float scaleAng;
        private float deltaY;
        private float deltaZ;
        private float deltaXAng;
        private float deltaYAng;
        private float deltaZAng;

        private Quaternion deltaRotation;
        private Quaternion lastRotation;

        private MessageTypes.Geometry.TwistStamped message;

        protected override void Start()
        {
            lastXPos = 0;
            lastRotation = PublishedTransform.localRotation.Unity2Ros();
            base.Start();
            InitializeMessage();
        }

        private void FixedUpdate()
        {
            UpdateMessage();
            counter++;
        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Geometry.TwistStamped
            {
                header = new MessageTypes.Std.Header()
                {
                    frame_id = FrameId
                }
            };
        }

        private void UpdateMessage()
        {
            message.header.Update();
            GetGeometryLinear(PublishedTransform.position.Unity2Ros(), message.twist.linear);
            //GetGeometryQuaternion(PublishedTransform.localEulerAngles.Unity2Ros(), message.twist.angular);
            GetGeometryQuaternion(PublishedTransform.localRotation.Unity2Ros(), message.twist.angular);

            Publish(message);
        }

        private void GetGeometryLinear(Vector3 position, MessageTypes.Geometry.Vector3 geometryLinear)
        {
            if (handSync.rButtonHand)
            {
                //deltaX = (position.y - lastYPos) / Time.deltaTime * scalePos;
                //deltaY = -(position.x - lastXPos) / Time.deltaTime * scalePos;
                //deltaZ = (position.z - lastZPos) / Time.deltaTime * scalePos;
                deltaX = (position.x - lastXPos) / Time.deltaTime * scalePos;
                deltaY = (position.y - lastYPos) / Time.deltaTime * scalePos;
                deltaZ = (position.z - lastZPos) / Time.deltaTime * scalePos;

                if (deltaX != 0)
                {
                    geometryLinear.x = deltaX;
                    if (counter >= goal)
                    {
                        handSync.deltaX = deltaX;
                        counter = 0;
                    }
                    
                }
                if (deltaY != 0)
                {
                    geometryLinear.y = deltaY;
                }
                if (deltaZ != 0)
                {
                    geometryLinear.z = deltaZ;
                }

                lastXPos = position.x;
                lastYPos = position.y;
                lastZPos = position.z;

                
            }
            else
            {
                geometryLinear.x = 0;
                geometryLinear.y = 0;
                geometryLinear.z = 0;
            }
        }

        private void GetGeometryQuaternion(Quaternion rotation, MessageTypes.Geometry.Vector3 geometryAngular)
        {
            if (handSync.rButtonHand)
            {
                ////deltaXAng = -(eulerAngles.y - lastYAng) / Time.deltaTime * (float)3.1415 / (float)180 * scaleAng;
                ////deltaYAng = (eulerAngles.x - lastXAng) / Time.deltaTime * (float)3.1415 / (float)180 * scaleAng;
                ////deltaZAng = -(eulerAngles.z - lastZAng) / Time.deltaTime * (float)3.1415 / (float)180 * scaleAng;
                //deltaXAng = -(eulerAngles.x - lastXAng) / Time.deltaTime * (float)3.1415 / (float)180 * scaleAng;
                //deltaYAng = -(eulerAngles.y - lastYAng) / Time.deltaTime * (float)3.1415 / (float)180 * scaleAng;
                //deltaZAng = -(eulerAngles.z - lastZAng) / Time.deltaTime * (float)3.1415 / (float)180 * scaleAng;

                //if (deltaXAng != 0)
                //{
                //    geometryAngular.x = deltaXAng;
                //}
                //if (deltaYAng != 0)
                //{
                //    geometryAngular.y = deltaYAng;
                //}
                //if (deltaZAng != 0)
                //{
                //    geometryAngular.z = deltaZAng;
                //}

                //print(deltaXAng);

                //lastXAng = eulerAngles.x;
                //lastYAng = eulerAngles.y;
                //lastZAng = eulerAngles.z;

                //deltaRotation = rotation.eulerAngles - lastRotation.eulerAngles;

                //if (deltaRotation != Vector3.zero)
                //{
                //    geometryAngular.x = deltaRotation.x;
                //    geometryAngular.y = deltaRotation.y;
                //    geometryAngular.z = deltaRotation.z;
                //    print(deltaRotation.x);
                //}

                deltaRotation = rotation * Quaternion.Inverse(lastRotation);

                if (deltaRotation != Quaternion.identity)
                {
                    //if (deltaRotation.eulerAngles.x >= 180)
                    //{
                    //    geometryAngular.x = deltaRotation.eulerAngles.x - 360;
                    //} else
                    //{
                    //    geometryAngular.x = deltaRotation.eulerAngles.x;
                    //}

                    //if (deltaRotation.eulerAngles.y >= 180)
                    //{
                    //    geometryAngular.y = deltaRotation.eulerAngles.y - 360;
                    //}
                    //else
                    //{
                    //    geometryAngular.y = deltaRotation.eulerAngles.y;
                    //}

                    //if (deltaRotation.eulerAngles.z >= 180)
                    //{
                    //    geometryAngular.z = deltaRotation.eulerAngles.z - 360;
                    //}
                    //else
                    //{
                    //    geometryAngular.z = deltaRotation.eulerAngles.z;
                    //}
                    deltaRotation.ToAngleAxis(out var angle, out var axis);

                    angle *= Mathf.Deg2Rad;

                    geometryAngular.x = (1.0f / Time.deltaTime) * angle * axis.x * scaleAng;
                    geometryAngular.y = (1.0f / Time.deltaTime) * angle * axis.y * scaleAng;
                    geometryAngular.z = (1.0f / Time.deltaTime) * angle * axis.z * scaleAng;

                }

                //geometryAngular.x = geometryAngular.x / Time.deltaTime * (float)3.1415 / (float)180 * scaleAng;
                //geometryAngular.y = geometryAngular.y / Time.deltaTime * (float)3.1415 / (float)180 * scaleAng;
                //geometryAngular.z = geometryAngular.z / Time.deltaTime * (float)3.1415 / (float)180 * scaleAng;

                print(geometryAngular.x);

                lastRotation = rotation;
            }
            else
            {
                geometryAngular.x = 0;
                geometryAngular.y = 0;
                geometryAngular.z = 0;
            }
        }

    }
}
