using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DavidTielke.PersonManagementApp.Data.DataStoring
{
    class CsvSerializer
    {
        public IEnumerable<TType> Deserialize<TType>(string path)
        {
            var dataLines = File.ReadAllLines(path);
            var type = typeof(TType);
            var properties = type.GetProperties()
                .Where(p => !p.IsDefined(typeof(CsvIgnoreAttribute))).ToArray();

            var itemList = new List<TType>();

            for (var i = 1; i < dataLines.Length; i++)
            {
                var item = Activator.CreateInstance<TType>();

                var parts = dataLines[i].Split(',');
                for (var j = 0; j < parts.Length; j++)
                {
                    var propertyType = properties[j].PropertyType;
                    var value = Convert.ChangeType(parts[j], propertyType);
                    properties[j].SetValue(item, value);
                }

                itemList.Add(item);
            }

            return itemList;
        }

        public void Serialize<TType>(IEnumerable<TType> items, string path)
        {
            var sb = new StringBuilder();

            var type = typeof(TType);
            var properties = type.GetProperties()
                .Where(p => !p.IsDefined(typeof(CsvIgnoreAttribute)));
            var propertyNames = properties
                .Select(p => p.Name);
            var header = string.Join(";", propertyNames);

            sb.AppendLine(header);
            foreach (var item in items)
            {
                var values = new List<object>();
                foreach (var property in properties)
                {
                    var value = property.GetValue(item);
                    values.Add(value);
                }

                var dataLine = string.Join(";", values);
                sb.AppendLine(dataLine);
            }

            File.WriteAllText(path, sb.ToString());
        }
    }
}