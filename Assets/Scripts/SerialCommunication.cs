using System;
using System.Collections;
using System.IO;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


namespace TextEntry
{
    public class SerialCommunication : MonoBehaviour
    {

        public static int baudrate = 250000;
        public static string port_name = "\\\\.\\COM13";
        public static SerialPort stream = new SerialPort(port_name, baudrate);

        string ArduinoSerialData;

        public static Quaternion cRotation = Quaternion.identity;

        private static bool _button;
        public static bool buttonState { get; set; }

        private void OnDisable()
        {
            TryClose();
        }

        private void OnApplicationQuit()
        {
            TryClose();
        }

        void Update()
        {
            StartCoroutine(StartDelay(10));

            if (!stream.IsOpen)
                TryConnect();

            try
            {
                ArduinoSerialData = stream.ReadLine();
                string[] SplitData = ArduinoSerialData.Split(' ');

                if (SplitData.Length != 9)
                    return;

                float qw;
                float qx;
                float qy;
                float qz;

                string w = SplitData[0];
                string x = SplitData[1];
                string y = SplitData[2];
                string z = SplitData[3];

                qw = float.Parse(w);
                qx = -float.Parse(x);
                qy = float.Parse(y);
                qz = -float.Parse(z);

                float ya = float.Parse(SplitData[4]);
                float pi = float.Parse(SplitData[5]);
                float ro = float.Parse(SplitData[6]);

                cRotation = new Quaternion(qy, qz, qx, qw);

                buttonState = int.Parse(SplitData[7]) == 0;
            }
            catch (IOException)
            {
                TryClose();
            }
            catch (InvalidOperationException)
            {
                //Debug.Log("Glove not Connected");
            }
        }

        void TryClose()
        {
            try
            {
                stream.Close();
            }
            catch (Exception ex)
            {
                ArduinoSerialData = ex.Message;
            }
        }

        IEnumerator StartDelay(int ms)
        {
            yield return new WaitForSeconds(ms);
        }
        bool TryConnect()
        {
            try
            {
                stream.Open();
                StartCoroutine(StartDelay(10000));
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                return false;
            }
        }
    }
}