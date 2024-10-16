using System.Reflection;

namespace Helpers.Csv
{
    public class CsvHelpers
    {
        public static async Task<List<T>> ImportCsv<T>(IFormFile file) where T : new()
        {
            List<T> listData = new List<T>();

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var lines = new List<string>();

                while (await stream.ReadLineAsync() is string line)
                {
                    lines.Add(line);
                }

                if (lines.Count == 0)
                    return listData; 
                var headerLine = lines[0];
                var columns = headerLine.Split(',').Select((value, index) => new { Position = index, Name = value.Trim() }).ToList();
                var dataLines = lines.Skip(1).ToList();
                var type = typeof(T);
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                dataLines.ForEach(line =>
                {
                    T obj = new T();
                    var data = line.Split(',');
                    foreach (var prop in props)
                    {
                        var column = columns.SingleOrDefault(c => c.Name.Equals(prop.Name, StringComparison.OrdinalIgnoreCase));
                        if (column != null && column.Position < data.Length)
                        {
                            var value = data[column.Position];

                            if (!string.IsNullOrEmpty(value))
                            {
                                var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                                var convertedValue = Convert.ChangeType(value, targetType);
                                prop.SetValue(obj, convertedValue);
                            }
                            else
                            {
                                if (prop.PropertyType.IsValueType)
                                {
                                    prop.SetValue(obj, Activator.CreateInstance(prop.PropertyType));
                                }
                                else
                                {
                                    prop.SetValue(obj, null);
                                }
                            }
                        }
                    }

                    listData.Add(obj);
                });
            }

            return listData;
        }
        public static async Task<byte[]> ExportCsv<T>(List<T> listData, List<CsvItem> headers)
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms))
                {
                    // Header
                    var header = string.Join(",", headers.Select(x => x.header));
                    await sw.WriteLineAsync(header);

                    // Data
                    var listCode = headers.Select(x => x.key).ToList();
                    foreach (var dataItem in listData)
                    {
                        Type entType = dataItem.GetType();
                        var props = entType.GetProperties()
                            .Where(p => listCode.Any(p2 => p2.Equals(p.Name, StringComparison.OrdinalIgnoreCase)))
                            .ToList();

                        var values = new List<string>();
                        foreach (var prop in props)
                        {
                            var propValue = prop.GetValue(dataItem);
                            values.Add(propValue != null ? propValue.ToString() : string.Empty);
                        }

                        var str = string.Join(",", values);
                        await sw.WriteLineAsync(str);
                    }
                }
                await ms.FlushAsync();
                return ms.ToArray();
            }
        }

    }
}
