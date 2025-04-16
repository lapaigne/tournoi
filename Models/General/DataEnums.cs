namespace TournoiServer.Models
{
    public enum Rank
    {
        Top,
        Second,
        Unranked
    }

    // ISO/IEC 5218 (2022) standard
    public enum Sex
    {
        NotKnown = 0,
        Male = 1,
        Female = 2,
        NotApplicable = 9
    }
}
