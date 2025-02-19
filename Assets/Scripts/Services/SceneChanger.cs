using System;
using UnityEngine.SceneManagement;

namespace Services
{
    /// <summary>
    /// シーン遷移
    /// </summary>
    public class SceneChanger
    {
        /// <summary>
        /// シーン遷移
        /// </summary>
        /// <param name="name">シーン名</param>
        public void Change(string name)
        {
            try
            {
                SceneManager.LoadScene(name);
            }
            catch (Exception e)
            {
                Err.Err.ViewErr("シーン読み込み失敗\n" +e);
                throw;
            } 
        }
    }
}