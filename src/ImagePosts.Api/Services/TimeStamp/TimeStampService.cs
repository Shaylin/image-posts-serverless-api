namespace ImagePostsAPI.Services.TimeStamp;

public class TimeStampService : ITimeStampService
{
    public DateTime GetUtcNow()
    {
        return DateTime.UtcNow;
    }
}