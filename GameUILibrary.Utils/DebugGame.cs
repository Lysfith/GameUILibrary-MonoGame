using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GameUILibrary.Utils
{
    public class DebugGame
    {
        private static Stopwatch _sw;
        private static Stopwatch _swFps;
        private static int _frames;
        private static int _lastCountFrames;

        public static void Log(string classe, string methode, string message)
        {
#if DEBUG
            Console.WriteLine(string.Format("[{0}][{1}] {2}", classe, methode, message));
#endif
        }

        public static void StartTimer()
        {
#if DEBUG
            if(_sw != null && _sw.IsRunning)
            {
                _sw.Stop();
                _sw = null;
            }

            _sw = new Stopwatch();
            _sw.Start();
#endif
        }

        public static void StopTimer()
        {
#if DEBUG
            if (_sw != null && _sw.IsRunning)
            {
                _sw.Stop();
                Console.WriteLine(string.Format("[Timer] {0} ms", _sw.ElapsedMilliseconds));
                _sw = null;
                
            }
#endif
        }

        public static void ShowFps()
        {
#if true
            if (_swFps != null && _swFps.IsRunning)
            {
                _swFps.Stop();
                _swFps = null;
            }

            _swFps = new Stopwatch();
            _swFps.Start();
#endif
        }

        public static void Update()
        {
#if true
            if (_swFps != null && _swFps.IsRunning)
            {
                if(_swFps.ElapsedMilliseconds < 1000)
                {
                    _frames++;
                }
                else
                {
                    _lastCountFrames = _frames;
                    _frames = 0;
                    _swFps.Reset();
                    _swFps.Start();
                }
            }
#endif
        }

        public static int GetFps()
        {
            return _lastCountFrames;
        }
    }
}
