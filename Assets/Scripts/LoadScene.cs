using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class LoadScene
    {
        /// <summary>
        /// 指定名のシーンに遷移
        /// </summary>
        /// <param name="sceneName">移動先のシーン名</param>
        public static void SceneLoad(string sceneName)
        {
            Debug.Log("シーン遷移");
            SceneManager.LoadScene(sceneName);
        }
    }
}