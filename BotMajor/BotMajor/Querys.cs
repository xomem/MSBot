using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BotMajor
{
    public static class Querys
    {
        const string ConnectionAdres = @"Data Source=192.168.2.42;Initial Catalog=Temp;Persist Security Info=True;User ID=autosalesadmin;Password=itdeveloper;";

        public static List<DataRow> cars = new List<DataRow>();
        public static string GetCarByChatId(string chatId)
        {
            string result = "";
            var query = $"SELECT c.manufacturer, c.model,c.gosNumber FROM Car AS c INNER JOIN Client AS cli ON cli.ID = c.client WHERE cli.ChatNumber = '{chatId}';";
            using (SqlConnection connection = new SqlConnection(ConnectionAdres))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    result = reader.GetString(0) + reader.GetString(1) + "(" +reader.GetString(2) + ")";
                }
                reader.Close();
                connection.Close();
            }
            return result;
        }
        public static bool MoreOneCar(string chatId)
        {
            bool result = false;
            var query = $"SELECT top 1 count(*) as count FROM dbo.Client AS c INNER JOIN dbo.car AS t ON t.client = c.ID WHERE c.ChatNumber = '{chatId}'; ";
            using (SqlConnection connection = new SqlConnection(ConnectionAdres))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    if(reader.GetInt32(0) > 1)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                reader.Close();
                connection.Close();
            }
            return result;
        }
        public static string GetTIDate(string carNumber)
        {
            string result = "";
            SqlConnection sqlConnection = new SqlConnection(ConnectionAdres);
            var query = $"SELECT ti.TechnicalInspection FROM TechnicalInspection AS ti INNER JOIN Car AS c ON c.Id = ti.Id WHERE c.gosNumber = '{carNumber}';";

            SqlCommand nameComand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader reader = nameComand.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
               result = reader.GetDateTime(0).Date.ToString();
            }
            reader.Close();
            sqlConnection.Close();
            return result;
        }
        public static IEnumerable<Cars> GetCarsByChatID(string chatId)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionAdres);
            var query = $"select  car.id, car.manufacturer, car.model, car.gosNumber, client.Name, client.Surname, client.Phonenumber from [Client] as client INNER JOIN Car as car on car.client = client.ID where client.ChatNumber = N'{chatId}';";

            SqlCommand nameComand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader reader = nameComand.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                do
                {
                    var carId = reader.GetInt32(0);
                    var carMark = reader.GetString(1);
                    var carModel = reader.GetString(2);
                    var carNumber = reader.GetString(3);
                    yield return new Cars { carId = carId, carMark = carMark, carModel = carModel, carNumber = carNumber };
                }
                while (reader.Read());
            }
            reader.Close();
            sqlConnection.Close();
        }
        public static bool FirstUser(string chatId)
        {
            bool result = false;
            SqlConnection sqlConnection = new SqlConnection(ConnectionAdres);
            var quary = $"SELECT* FROM Client AS c WHERE c.ChatNumber = '{chatId}'";
            SqlCommand roomCommand = new SqlCommand(quary, sqlConnection);
            sqlConnection.Open();
            SqlDataReader reader = roomCommand.ExecuteReader();
            result = !reader.HasRows;
            reader.Close();
            sqlConnection.Close();
            return result;
        }
        public static bool RegistUser(string phoneNumber, string chatId)
        {
            bool result = false;
            SqlConnection sqlConnection = new SqlConnection(ConnectionAdres);
            var quary = $"UPDATE Client SET ChatNumber = '{chatId}' WHERE Phonenumber = '{phoneNumber}';";
            SqlCommand roomCommand = new SqlCommand(quary, sqlConnection);
            sqlConnection.Open();
            SqlDataReader reader = roomCommand.ExecuteReader();
            result = !reader.HasRows;
            reader.Close();
            sqlConnection.Close();
            return result;
        }
        public static string TechnicalInspection(string carID)
        {
            string result = "";
            SqlConnection sqlConnection = new SqlConnection(ConnectionAdres);
            var query = $"SELECT TechnicalInspection FROM TechnicalInspection AS ti WHERE ti.IdCar = '{carID}';";

            SqlCommand nameComand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader reader = nameComand.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                result = reader.GetString(0);
            }
            reader.Close();
            sqlConnection.Close();
            return result;
        }
    }
}