using System;

namespace AdapterPattern
{
    // nie wolno ruszać kodu!
    public sealed class HyteraRadio
    {

        private RadioStatus status;

        public void Init()
        {
            status = RadioStatus.On;
        }

        public void SendMessage(byte channel, string content)
        {
            if (status == RadioStatus.On)
            {
                Console.WriteLine($"CHANNEL {channel}, MESSAGE {content}");
            }
        }

        public void Release()
        {
            status = RadioStatus.Off;
        }

        public enum RadioStatus
        {
            On,
            Off
        }

    }
}
