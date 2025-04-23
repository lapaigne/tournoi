namespace TournoiServer
{
    /// <summary>
    /// Property with this attribute should not be changed and/or passed by the program
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DBPrimaryKey : Attribute { }

    /// <summary>
    /// Property with this attribute is optional and will be defaulted to <c>NULL</c>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DBNullable : Attribute { }

    /// <summary>
    /// Property with this attribute is optional and will be defaulted to <c>NULL</c>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DBForeignKey : Attribute
    {
        public string TargerTableName { get; set; }
        public string TargetPropertyName { get; set; }
    }
}
