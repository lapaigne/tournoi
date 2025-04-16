using Microsoft.Data.Sqlite;
using System.Reflection;
using static TournoiServer.DB.TypeConverter;

namespace TournoiServer.DB
{
    public partial class DBConnection
    {
        private SqliteCommand _command;

        private bool ExecuteCommand(string command, string displayText = "Unspecified Operation")
        {
            _command = Connection.CreateCommand();
            _command.CommandText = command;

            try
            {
                _command.ExecuteNonQuery();
            }
            catch (SqliteException e)
            {
                _command.CommandText = default;
                Console.WriteLine($"ERROR WHEN: {displayText}");
                Console.WriteLine(e.Message);
                return false;
            }

            _command.CommandText = default;
            Console.WriteLine($"OK: {displayText}");
            return true;
        }

        private bool CreateTable<T>(string tableName)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            IEnumerable<string> propStrings = properties.Select((PropertyInfo p) =>
            {
                string @base = $"{p.Name} {GetSQLType(p.PropertyType.Name)}";
                if (Attribute.IsDefined(p, typeof(DBPrimaryKey)))
                {
                    @base += " PRIMARY KEY AUTOINCREMENT";
                }

                if (p.PropertyType.IsEnum && p.Name == "Sex")
                {
                    @base += $" CHECK({p.Name} IN (0, 1, 2, 9))";
                }

                if (!Attribute.IsDefined(p, typeof(DBNullable)))
                {
                    @base += " NOT NULL";
                }

                return @base;
            });

            string commandText =
                $"CREATE TABLE {tableName} ("
                + string.Join(
                    ",\n",
                    propStrings)
                + "\n);";

            return ExecuteCommand(commandText, $"Created table '{tableName}'");
        }

        private bool AddRows<T>(string target, IEnumerable<T> data)
        {
            if (data is null) { return false; }

            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties()
                .Where(p => !Attribute.IsDefined(p, typeof(DBPrimaryKey)));

            List<string> valList = new List<string>();
            foreach (T d in data)
            {
                var values = properties.Select(p =>
                {
                    object? value = p.GetValue(d);
                    string result = (p.PropertyType.IsEnum ? (int)value : value).ToString();
                    return "\"" + result + "\"";
                });

                string res = "(" + string.Join(", ", values) + ")";
                valList.Add(res);
            };

            string valueString = string.Join(", ", valList);

            string propString = "(" + string.Join(", ", properties.Select(p => p.Name)) + ")";

            string commandText = $"INSERT INTO {target} {propString}\n VALUES {valueString};";

            return ExecuteCommand(commandText,
                $"Inserted list of values of type {typeof(T).Name} into {target}");
        }

        /// <summary>
        /// one shall validate data prior to calling the method
        /// </summary>
        /// <typeparam name="T">type of the table data</typeparam>
        /// <param name="target">name of the table</param>
        /// <param name="data">data of type T that is to be added to the table</param>
        /// <returns></returns>
        private bool AddRow<T>(string target, T data)
        {
            if (data is null) { return false; }

            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties()
                .Where(p => !Attribute.IsDefined(p, typeof(DBPrimaryKey)));

            var values = properties.Select(p =>
            {
                object? value = p.GetValue(data);
                string result = (p.PropertyType.IsEnum ? (int)value : value).ToString();
                return "\"" + result + "\"";
            });

            string propString = "(" + string.Join(", ", properties.Select(p => p.Name)) + ")";
            string valueString = "(" + string.Join(", ", values) + ")";

            string commandText = $"INSERT INTO {target} {propString}\n VALUES {valueString};";

            return ExecuteCommand(commandText, $"Inserted values of type {typeof(T).Name} into {target} ONCE");
        }

        private T? GetRow<T>(string table)
        {
            return default;
        }

        private IEnumerable<T>? GetTable<T>(string table)
        {
            return default;
        }
    }
}
