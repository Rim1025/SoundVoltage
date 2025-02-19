using System;
using System.IO.Ports;
using UniRx;
using System.Threading;
using Model;

namespace Serial
{
    /// <summary>
    /// シリアル通信
    /// </summary>
    public class GetSerial
    {
        public delegate void SerialDataReceivedEventHandler(string message);

        public event SerialDataReceivedEventHandler OnDataReceived;
        
        private string _portName;
        private int _baud;
        private SerialPort _port;
        
        private bool _isRunning = false;
        private string _message;
        private bool _isNewMessageReceived = false;
        
        private Thread _thread;
        
        public GetSerial(string portName, int baud)
        {
            _portName = portName;
            _baud = baud;
            Open();
        }
    
        public void OnDestroy()
        {
            Close();
        }
        
        /// <summary>
        /// 通信開始
        /// </summary>
        private void Open()
        {
            _port = new SerialPort(_portName, _baud);
            try
            {
                _port.Open();
                _isRunning = true;

                _thread = new Thread(Read);
                _thread.Start();

                GameEvents.UpdateGame.Subscribe(_ =>
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
    
        /// <summary>
        /// 通信終了
        /// </summary>
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
            }
        }
    
        /// <summary>
        /// 通信内容読み込み
        /// </summary>
        private void Read()
        {
            while (_isRunning&& _port != null && _port.IsOpen)
            {
                try
                {
                    _message = _port.ReadLine();
                    _isNewMessageReceived = true;
                }
                catch (Exception e)
                { 
                    Err.Err.ViewErr(e.Message);
                }
            }
        }
    }
}