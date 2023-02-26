using SevenBoldPencil.EasyEvents;

namespace ClickerLogic
{
    public class SharedData
    {
        public readonly EventsBus EventsBus;

        public SharedData(EventsBus eventsBus)
        {
            EventsBus = eventsBus;
        }
    }
}
