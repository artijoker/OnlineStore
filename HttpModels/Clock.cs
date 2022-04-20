namespace HttpModels
{
    public class Clock : IClock 
    {
        public DateTime GetClock() => DateTime.Now;
    }
}
