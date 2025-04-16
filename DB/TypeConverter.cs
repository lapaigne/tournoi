namespace TournoiServer.DB
{
    public static class TypeConverter
    {
        public static string GetSQLType(string typeName) => typeName switch
        {
            "Int32" => "INTEGER",
            _ => "TEXT",
        };
    }
}
