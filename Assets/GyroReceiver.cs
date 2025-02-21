using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class GyroReceiver : MonoBehaviour
{
    public string portName = "COM10";
    public int baudRate = 96000;
    private float debugDelay = 1f;

    string message;

    private SerialPort serialPort;
    [SerializeField] private Transform capsulius;

    void Start()
    {
        // Opening the connection to the Arduino / gyro
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
        // Setting default player object rotation
        capsulius.localRotation = Quaternion.identity;
        // Starts reporting sensor data on a preset delay
        StartCoroutine(DelayDebugLog());
    }

    void Update()
    {
        if (serialPort.BytesToRead > 0)
        {
            message = "";
            message = serialPort.ReadLine();
            // Remove start and end statements
            message = message.Substring("<START>".Length, message.Length - "<START>".Length - "<END>".Length);
        }
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }

    IEnumerator DelayDebugLog()
    {
        while (true)
        {
        Debug.Log(message);
        yield return new WaitForSeconds(debugDelay);
        }
    }
}