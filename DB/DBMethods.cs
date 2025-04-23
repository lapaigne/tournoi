using Microsoft.Data.Sqlite;
using System.Reflection;
using static TournoiServer.DB.TypeConverter;

namespace TournoiServer.DB
{
    public partial class DBConnection
    {
        private SqliteCommand _command;

        private bool ExecuteCommand(string command, string commandName = "Unspecified Operation")
        {
            _command = Connection.CreateCommand();
            _command.CommandText = command;

            try
            {
                _command.ExecuteNonQuery();
            }
            catch (SqliteException e)
            {
                Console.WriteLine($"ERROR WHEN: {commandName}");
                Console.WriteLine(e.Message);
                return false;
            }

            Console.WriteLine($"OK: {commandName}");
            return true;
        }

        private bool ExecuteQuery(string command, out SqliteDataReader? reader, string commandName = "Unspecified Operation")
        {
            _command = Connection.CreateCommand();
            _command.CommandText = command;

            try
            {
                reader = _command.ExecuteReader();
            }
            catch (SqliteException e)
            {
                reader = null;
                Console.WriteLine($"ERROR WHEN: {commandName}");
                Console.WriteLine(e.Message);
                return false;
            }

            Console.WriteLine($"OK: {commandName}");
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
            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties()
                .Where(p => !Attribute.IsDefined(p, typeof(DBPrimaryKey)));

            List<string> valList = new List<string>();
            foreach (T d in data)
            {
                var values = properties.Select(p =>
                {
                    object? value = p.GetValue(d);
                    object? result = p.PropertyType.IsEnum && value is not null ? (int)value : value;
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

        private bool AddRow<T>(string target, T data)
        {
            if (data is null)
            {
                return false;
            }

            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties()
                .Where(p => !Attribute.IsDefined(p, typeof(DBPrimaryKey)));

            var values = properties.Select(p =>
            {
                object? value = p.GetValue(data);
                object? result = p.PropertyType.IsEnum && value is not null ? (int)value : value;
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

        private List<Dictionary<string, string>> GetTable(string table, params string[] propertyNames)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            string propString = string.Join(", ", propertyNames);

            string command = $"SELECT {propString} FROM {table}";

            if (ExecuteQuery(command, out var reader, $"Read data with fields {propString} from {table}"))
            {
                if (reader is null || !reader.HasRows)
                {
                    return result;
                }

                while (reader.Read())
                {
                    Dictionary<string, string> row = new Dictionary<string, string>();
                    foreach (string p in propertyNames)
                    {
                        row[p] = (string)reader[p];
                    }
                    string current = string.Join("\t", row);
                    Console.WriteLine(current);
                    result.Add(row);
                }
            }

            return result;
        }

        private List<T> GetTable<T>(string table)
        {
            List<T> result = new List<T>();
            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties()
                .Where(p => !Attribute.IsDefined(p, typeof(DBPrimaryKey)));

            string propString = string.Join(", ", properties.Select(p => p.Name));

            string command = $"SELECT {propString} FROM {table}";

            if (ExecuteQuery(command, out var reader, $"Read data with fields {propString} from {table}"))
            {
                if (reader is null || !reader.HasRows)
                {
                    return result;
                }
                while (reader.Read())
                {
                    string current = string.Join("\t", properties.Select(p => reader[p.Name]));
                    Console.WriteLine(current);
                }
            }

            return result;
        }
    }
}
