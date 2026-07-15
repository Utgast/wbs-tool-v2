#r "nuget: Microsoft.Data.Sqlite, 8.0.0"

using System;
using Microsoft.Data.Sqlite;

string dbPath = @"C:\Projekte\wbs-tool\src\backend\WbsTool.Api\wbstool.db";
string connectionString = "Data Source=" + dbPath;

var connection = new SqliteConnection(connectionString);
connection.Open();

var command = connection.CreateCommand();
command.CommandText = @"
SELECT
    ProjectId,
    VisibleWbsId,
    COUNT(*) AS DuplicateCount
FROM WbsNodes
GROUP BY ProjectId, VisibleWbsId
HAVING COUNT(*) > 1
ORDER BY ProjectId, VisibleWbsId;
";

var reader = command.ExecuteReader();

Console.WriteLine("Doppelte WBS-IDs pro Projekt:");
Console.WriteLine("--------------------------------");

bool foundAny = false;

while (reader.Read())
{
    foundAny = true;

    string projectId = reader.GetString(0);
    string visibleWbsId = reader.GetString(1);
    int duplicateCount = reader.GetInt32(2);

    Console.WriteLine("ProjectId: " + projectId + " | VisibleWbsId: " + visibleWbsId + " | Count: " + duplicateCount);
}

if (!foundAny)
{
    Console.WriteLine("Keine Duplikate gefunden.");
}

reader.Close();
connection.Close();