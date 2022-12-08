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

            GetGeometryLinear(new Vector3(handSync.velocities[0], handSync.velocities[1], handSync.velocities[2]).Unity2Ros(), message.twist.linear);

            GetGeometryQuaternion(new Vector3(handSync.velocities[3], handSync.velocities[4], handSync.velocities[5]).Unity2Ros(), message.twist.angular);

            Publish(message);
        }

        private void GetGeometryLinear(Vector3 position, MessageTypes.Geometry.Vector3 geometryLinear)
        {
            if (handSync.rButtonHand)
            {
                geometryLinear.x = position.x * scalePos;
                geometryLinear.y = position.y * scalePos;
                geometryLinear.z = position.z * scalePos;
            }
            else
            {
                geometryLinear.x = 0;
                geometryLinear.y = 0;
                geometryLinear.z = 0;
            }
        }

        private void GetGeometryQuaternion(Vector3 rotation, MessageTypes.Geometry.Vector3 geometryAngular)
        {
            if (handSync.rButtonHand)
            {
                geometryAngular.x = -rotation.x * scaleAng;
                geometryAngular.y = -rotation.y * scaleAng;
                geometryAngular.z = -rotation.z * scaleAng;
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
