using System;
using System.IO.Ports;
using UniRx;
using UnityEngine;
using System.Threading;

namespace Serial
{
    public class GetSerial
    {
        public delegate void SerialDataReceivedEventHandler(string message);

        public event SerialDataReceivedEventHandler OnDataReceived;
        
        private string _portName;
        private SerialManager _manager;
        private int _baud;
        private SerialPort _port;
        
        private bool _isRunning = false;
        private SerialUpdate _update;
        private string _message;
        private bool _isNewMessageReceived = false;

        public string Readline;
        
        private Thread _thread;
        
        public GetSerial(string portName, int baud, SerialManager manager,SerialUpdate update)
        {
            _portName = portName;
            _baud = baud;
            _manager = manager;
            _update = update;
            Open();
        }
    
        public void OnDestroy()
        {
            Close();
        }
        
    
        private void Open()
        {
            _port = new SerialPort(_portName, _baud);
            try
            {
                _port.Open();
                _isRunning = true;
                Debug.Log("通信開始");

                _thread = new Thread(Read);
                _thread.Start();

                _update.Timer.Subscribe(x =>
                {
                    if (_isNewMessageReceived)
                    {
                        OnDataReceived(_message);
                    }
                    _isNewMessageReceived = false;
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    
        private void Close()
        {
            _isRunning = false;
            if (_port != null)
            {
                if (_port.IsOpen)
                {
                    _port.Close();
                }
                _port.Dispose();
                Debug.Log("通信終了");
            }
        }
    
        private void Read()
        {
            while (_isRunning&& _port != null && _port.IsOpen)
            {
                try
                {
                    _message = _port.ReadLine();
                    _isNewMessageReceived = true;
                }
                catch (System.Exception e)
                {
                    Readline = e.Message;
                    Debug.LogWarning("1:" + Readline);
                }
            }
        }
    }
}