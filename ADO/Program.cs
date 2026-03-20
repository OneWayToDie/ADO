using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ADO
{
	internal class Program
	{
		static void Main(string[] args)
		{
			string connection_string = "Data Source=(localdb)\\MSSQLLocalDB;" +
				"Initial Catalog=Movies_PV_521;" +
				"Integrated Security=True;" +
				"Connect Timeout=30;Encrypt=False;" +
				"TrustServerCertificate=False;" +
				"ApplicationIntent=ReadWrite;" +
				"MultiSubnetFailover=False";
			Console.WriteLine(connection_string);

			SqlConnection connection = new SqlConnection(connection_string);
			connection.Open();

			string selectQuery = "SELECT movie_id, title, release_date, first_name, last_name FROM Movies, Directors WHERE director = director_id";
			ExecuteSelectQuery(connection, selectQuery);

			string countQuery = "SELECT COUNT(*) FROM Movies";
			object recordCount = ExecuteScalarQuery(connection, countQuery);
			Console.WriteLine($"Количество записей:\t{recordCount}");
			connection.Close();
		}

		static void ExecuteSelectQuery(SqlConnection connection, string cmd)
		{
			SqlCommand command = new SqlCommand(cmd, connection);
			SqlDataReader reader = command.ExecuteReader();

			for (int i = 0; i < reader.FieldCount; i++)
				Console.Write(reader.GetName(i) + "\t");
			Console.WriteLine();

			while (reader.Read())
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					Console.Write($"{reader[i]}\t");
				}
				Console.WriteLine();
			}
			reader.Close();
		}

		static object ExecuteScalarQuery(SqlConnection connection, string cmd)
		{
			SqlCommand command = new SqlCommand(cmd, connection);
			object result = command.ExecuteScalar();

			if (result == null)
				return null;

			return result;

		}
	}
}
