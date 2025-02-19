using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Mp3をAudioClipに変換
/// </summary>
public class Mp3ToAudioClip
{
    /// <summary>
    /// MP3 ファイルを AudioClip に変換
    /// </summary>
    /// <param name="filePath">MP3ファイルのパス</param>
    /// <returns>変換された AudioClip</returns>
    public async Task<AudioClip> Convert(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Err.Err.ViewErr($"指定されたMP3ファイルが見つかりません: {filePath}");
            return null;
        }

        string url = "file://" + Path.GetFullPath(filePath);
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            var request = www.SendWebRequest();

            while (!request.isDone)
            {
                await Task.Yield(); // 非同期で待機
            }

            if (www.result != UnityWebRequest.Result.Success)
            {
                Err.Err.ViewErr("MP3のロードに失敗: {www.error}");
                return null;
            }

            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
            if (audioClip == null)
            {
                Err.Err.ViewErr("AudioClip の取得に失敗しました");
                return null;
            }
            return audioClip;
        }
    }
}