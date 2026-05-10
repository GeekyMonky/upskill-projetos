using LibDB;
using System.Text;
using WebAPIBiblioteca.DTOs;

namespace WebAPIBiblioteca.Services
{
    public class DevService
    {
#if DEV_MODE
        public static string GetModel(List<string> models)
        {
            var result = new StringBuilder();

            string where = "";

            if (models != null && models.Count > 0)
            {
                var tableList = string.Join(",", models.Select(m => $"'{m}'"));
                where = $"WHERE TABLE_NAME IN ({tableList})";
            }

            string sql = $@"
                SELECT 
                    TABLE_NAME,
                    COLUMN_NAME,
                    DATA_TYPE
                FROM INFORMATION_SCHEMA.COLUMNS
                {where}
                ORDER BY TABLE_NAME, ORDINAL_POSITION";

            var rows = DALPro.Query<DevClassDTO>(sql);

            string currentTable = "";

            foreach (var r in rows)
            {
                if (currentTable != r.TABLE_NAME)
                {
                    if (currentTable != "")
                        result.AppendLine("}");

                    currentTable = r.TABLE_NAME;

                    result.AppendLine($"\npublic class {currentTable}");
                    result.AppendLine("{");
                }

                string type = r.DATA_TYPE switch
                {
                    "int" => "int",
                    "bigint" => "long",
                    "decimal" => "decimal?",
                    "numeric" => "decimal?",
                    "money" => "decimal?",
                    "nvarchar" => "string",
                    "varchar" => "string",
                    "datetime" => "DateTime?",
                    "date" => "DateTime?",
                    "bit" => "bool",
                    _ => "string"
                };

                result.AppendLine($"    public {type} {r.COLUMN_NAME} {{ get; set; }}");
            }

            if (currentTable != "")
                result.AppendLine("}");

            return result.ToString();
        }
#endif
    }
}