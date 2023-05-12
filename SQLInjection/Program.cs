// See https://aka.ms/new-console-template for more information
using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

Console.WriteLine("Start of Program");

// IConfigurationBuilder builder2 = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
// IConfigurationRoot config = builder2.Build();

var config = new ConfigurationBuilder()                    
    .AddJsonFile("appsettings.json", true, true)
    .AddUserSecrets<Program>()
    .Build(); 

var builder = new SqlConnectionStringBuilder(config["ConnectionStrings:defaultSQLConnection"]);       
builder.Password = config["SQLPWD"];
//Console.WriteLine(builder.ConnectionString);

List<rtItem> sqlrtItems = new();
 

string qry = String.Empty;
SqlDataReader dataReader;
SqlConnection SQLCn = new SqlConnection(builder.ConnectionString);
await SQLCn.OpenAsync();
SqlCommand command = new SqlCommand(qry, SQLCn);
        
        dataReader = await command.ExecuteReaderAsync();

        while (dataReader.Read())
        {
            sqlrtItems.Add(new rtItem()
            {
                rtId = dataReader.GetInt32(0),
                rtUserObjectId = dataReader.GetString(1),
                rtDescription = dataReader.GetString(2),
                rtLocation = dataReader.GetString(3),
                rtDateTime = dataReader.GetDateTime(4),
                rtImagePath = dataReader.GetString(5)
            });
        }

        // Tim beleives these are superfluous
        dataReader.Close();
        command.Dispose();
        SQLCn.Close();





Console.WriteLine("End of Program");

public class rtItem : IComparable<rtItem>, IEquatable<rtItem>
{
    public int rtId { get; set; }
    public string? rtUserObjectId { get; set; } //= null;
    public string? rtDescription { get; set; } //= null;
    public string? rtLocation { get; set; }
    public DateTime rtDateTime { get; set; }
    public string? rtImagePath { get; set; } //= string.Empty;        

    public bool Equals(rtItem? other)
    {
        if (other == null) return false;
        return (this.rtId.Equals(other.rtId));
    }
    public int CompareTo(rtItem? compareItem)
    {
        // A null value means that this object is greater.
        if (compareItem == null)
            return 1;

        else
            return this.rtId.CompareTo(compareItem.rtId);
    }

    public override string ToString()
    {
        return "Id: " + rtId + " File: " + rtImagePath + ", UserObjectId: " + rtUserObjectId + ", Desc: " + rtDescription + ", Location: " + rtLocation + ", When: " + rtDateTime;
    }

}
