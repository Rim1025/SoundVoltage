using System;
using UniRx;

namespace Err
{
    public static class Err
    {
        private static Subject<string> _subject = new Subject<string>();
        public static IObservable<string> ErrEvents => _subject;
        private static string _errText = "";
        public static void ViewErr(string text)
        {
            _errText += _errText == "" ? text : "\n" + text;
            _subject.OnNext(_errText);
        }
    }
}

