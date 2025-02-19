using System;
using UniRx;

namespace Err
{
    /// <summary>
    /// エラーを画面に表示
    /// </summary>
    public static class Err
    {
        private static Subject<string> _subject = new();
        public static IObservable<string> ErrEvents => _subject;
        private static string _errText = "";
        public static void ViewErr(string text)
        {
            _errText += _errText == "" ? text : "\n" + text;
            _subject.OnNext(_errText);
        }
    }
}

