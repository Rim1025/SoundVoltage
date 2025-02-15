using Defaults;
using Services;

namespace Model
{
    public class StatusSaver
    {
        private MusicSelect _select;
        private MusicSetting _setting;
        public StatusSaver(MusicSelect select, MusicSetting setting)
        {
            _select = select;
            _setting = setting;
        }

        public void ChangeScene()
        {
            MusicStatus _status = new MusicStatus(_select.MusicName.Value,
                _setting.StatusValues.List[(int)StatusType.Delay],
                _setting.StatusValues.List[(int)StatusType.Speed],
                _setting.StatusValues.List[(int)StatusType.Voltage]);
            JsonSaver.Save(_status);
            SceneChanger _sceneChanger = new SceneChanger();
            _sceneChanger.Change(GameData.InGameScene);
        }
    }
}