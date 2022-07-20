using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forest_Parser;
using System.Configuration;
namespace Forest_Parser.DB
{
    public class DBLayer
    {
        SqlConnection _connection;
        SqlCommand command;
        StringBuilder commandBody;

        public DBLayer()
        {
            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dealsDataBase"].ConnectionString);
            commandBody = new StringBuilder(
           @$"INSERT INTO [deals] 
            ([buyerINN]
           ,[buyerName]
           ,[dealNumber]
           ,[dealDate]
           ,[sellerINN]
           ,[sellerName]
           ,[woodVolumeBuyer]
           ,[woodVolumeSeller])
            VALUES");
        }

        public void AddValuesToCommand(Content content, char divider)
        {

            commandBody.Append($@"
            ('{content.buyerInn}',
            '{content.buyerName}',
            '{content.dealNumber}',
            '{content.dealDate}',
            '{content.sellerInn}',
            '{content.sellerName}',
            { content.woodVolumeBuyer},
            { content.woodVolumeSeller}){divider}");
        }
        public void ExecuteCommand()
        {

            try
            {
                command = new SqlCommand(commandBody.ToString(), _connection);
                _connection.Open();
                command.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                _connection.Close();
                commandBody = new StringBuilder(
           @$"INSERT INTO [deals] 
            ([buyerINN]
           ,[buyerName]
           ,[dealNumber]
           ,[dealDate]
           ,[sellerINN]
           ,[sellerName]
           ,[woodVolumeBuyer]
           ,[woodVolumeSeller])
            VALUES");
            }
        }
    }
}
