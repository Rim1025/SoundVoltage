using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Mp3ToAudioClipConverter : MonoBehaviour
{
    /// <summary>
    /// MP3 ファイルを AudioClip に変換
    /// </summary>
    /// <param name="filePath">MP3ファイルのパス</param>
    /// <returns>変換された AudioClip</returns>
    public async Task<AudioClip> ConvertMp3ToAudioClip(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"[Mp3ToAudioClipConverter] 指定されたMP3ファイルが見つかりません: {filePath}");
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
                Debug.LogError($"[Mp3ToAudioClipConverter] MP3のロードに失敗: {www.error}");
                return null;
            }

            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
            if (audioClip == null)
            {
                Debug.LogError("[Mp3ToAudioClipConverter] AudioClip の取得に失敗しました");
                return null;
            }

            Debug.Log($"[Mp3ToAudioClipConverter] MP3 のロード成功: {filePath}");
            return audioClip;
        }
    }
}