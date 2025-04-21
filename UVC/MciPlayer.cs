using System;
using System.Text;
using System.Runtime.InteropServices;

namespace AudioCDPlayer
{
    public class MciPlayer
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder returnString, int returnLength, IntPtr winHandle);

        private StringBuilder returnData = new StringBuilder(128);

        public void PlayCDAudio(string driveLetter, int trackNumber)
        {
            Stop();
            mciSendString($"open {driveLetter} type cdaudio alias cd", returnData, 0, IntPtr.Zero);
            mciSendString($"set cd time format tmsf", returnData, 0, IntPtr.Zero);
            mciSendString($"play cd from {trackNumber}", returnData, 0, IntPtr.Zero);
        }

        public void Pause()
        {
            mciSendString("pause cd", returnData, 0, IntPtr.Zero);
        }

        public void Resume()
        {
            mciSendString("resume cd", returnData, 0, IntPtr.Zero);
        }

        public void Stop()
        {
            mciSendString("stop cd", returnData, 0, IntPtr.Zero);
            mciSendString("close cd", returnData, 0, IntPtr.Zero);
        }
    }
}