using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADONET
{
    class Program
    {

        static void Main(string[] args)
        {//Connectionstring som bör fungera i era system
         //....
            string conString = @"Data Source=ELEV\;Initial Catalog=NORTHWND;Integrated Security=True";
            SqlConnection con = new SqlConnection(conString);
            con.Open();

            //1. Write a program that retrieves from the Northwind sample database in MS SQL Server the number of  rows in the Categories table.

            //using(con)
            //{
            //    SqlCommand command = new SqlCommand("select count(*) from Categories", con);
            //    int amountCategories = (int)command.ExecuteScalar();
            //    Console.WriteLine("Det finns {0} kategorier",amountCategories);
            //    Console.Read();
            //}


            //6:
            GetEmployerProducts(con, "Gravad lax", "Mishi Kobe Niku");
        }

        //6. Give the title and name of employees who have sold at least one of the products ‘Gravad Lax’ or ‘Mishi Kobe Niku’. 
        public static void GetEmployerProducts(SqlConnection con, string productname1, string productname2)
        {
            SqlCommand cmd = new SqlCommand("select distinct e.Title, e.FirstName, e.LastName from Employees e join Orders o on e.EmployeeID = o.EmployeeID join [Order Details] od " +
                  "on o.OrderID = od.OrderID join Products p on p.ProductID = od.ProductID " +
                  "where p.ProductName = @productname1 or p.ProductName = @productname2; ", con);
            cmd.Parameters.AddWithValue("@productname1", productname1);
            cmd.Parameters.AddWithValue("@productname2", productname2);
            //cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    string title = (string)reader["Title"];
                    string firstName = (string)reader["FirstName"];
                    string lastName = (string)reader["LastName"];
                    Console.WriteLine("{0} - {1} {2}",
                      title, firstName, lastName);
                }
            }
            Console.Read();
        }

        //7. Give the name and title of employees and the name and title of the person to which they refer
        // (or null for the latter values if they don’t refer to another employee). 

    }
}
