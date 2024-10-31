using Status;

namespace Start
{
    public class StatusHolder
    {
        public static StatusManager Status;

        public StatusHolder()
        {
            Status = new StatusManager();
        }
    }
}