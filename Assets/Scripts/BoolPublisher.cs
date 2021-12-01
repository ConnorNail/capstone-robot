using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class BoolPublisher : UnityPublisher<MessageTypes.Std.UInt64MultiArray>
    {
        public HandSync handSync;

        private MessageTypes.Std.UInt64MultiArray message;

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void FixedUpdate()
        {
            UpdateMessage();
        }

        private void InitializeMessage()
        {
            MessageTypes.Std.MultiArrayDimension[] dim = new MessageTypes.Std.MultiArrayDimension[1];
            dim[0] = new MessageTypes.Std.MultiArrayDimension("Buttons", 2, 2);
            message = new MessageTypes.Std.UInt64MultiArray
            {
                layout = new MessageTypes.Std.MultiArrayLayout()
                {
                    data_offset = 0,
                    dim = dim
                },
                data = new ulong[2]
            };
        }

        private void UpdateMessage()
        {
            GetButonState(message);

            Publish(message);
        }

        private void GetButonState(MessageTypes.Std.UInt64MultiArray array)
        {
            if (handSync.rButtonIndex)
            {
                array.data[0] = 1;
            } else
            {
                array.data[0] = 0;
            }

            if (handSync.rButtonA)
            {
                array.data[1] = 1;
            }
            else
            {
                array.data[1] = 0;
            }
        }
    }
}
