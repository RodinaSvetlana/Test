using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Data.Sqlite;


namespace test.controller
{
    public class UserController : Controller
    {

        private test.Pages.IndexModel.userContext db1;

        public string DbPath = $"Data Source={Environment.CurrentDirectory}{System.IO.Path.DirectorySeparatorChar}users.db"; //work

        public UserController(test.Pages.IndexModel.userContext _context)
        {
            test.Pages.IndexModel.userContext db1 = _context;
        }


        public async Task<IActionResult> Index()
        {
            //return View(await db1.employees.ToListAsync());
            return View(db1.employees.AsAsyncEnumerable());
            //return View(db1.employees.First());
        }
        
        [HttpPost]
        //public IActionResult Create(string subdivision_id,string org_id, string name, string position, string phone, string email)
        public object Create(string subdivision, string org, string name, string position, string phone, string email)
        {
                SqliteConnection connect = new SqliteConnection(DbPath);
            string commandText = @"INSERT INTO employees (subdivision_id, org_id, name,position,phone,email) 
                                 VALUES(@subdivision_id, @org_id, @name, @position,@phone,@email)";

            if (name != "")
            {
                SqliteCommand Command = new SqliteCommand(commandText, connect);
                Command.Parameters.AddWithValue("@subdivision_id", subdivision); 
                Command.Parameters.AddWithValue("@org_id", org);
                Command.Parameters.AddWithValue("@name", name);
                Command.Parameters.AddWithValue("@position", position);
                Command.Parameters.AddWithValue("@phone", phone);
                Command.Parameters.AddWithValue("@email", email);
                connect.Open();
                Command.ExecuteNonQuery();
                connect.Close();
            //db1.SaveChanges();
            }

            //return View();
            //return RedirectToAction("Edit");
            return false;
        }
        //[HttpDelete("{value}")]
        public object Delete(int id) //public async Task<IActionResult> Delete(int id)
        {
            SqliteConnection connect = new SqliteConnection(DbPath);
            string commandText = @"DELETE FROM employees WHERE id=@id";

            if (id>0)
            {
                SqliteCommand Command = new SqliteCommand(commandText, connect);
                Command.Parameters.AddWithValue("@id",id); // присваиваем переменной значение

                connect.Open();
                Command.ExecuteNonQuery();
                connect.Close();
            }

            //db1.employees.Remove(id);
            //db1.employees.SaveChanges(); //await db1.employees.SaveChangesAsync();*/

            return false;
        }
        [HttpPost]
        public object Update(int id,int subdivision, int org, string name, string position, string phone, string email) //public async Task<IActionResult> Delete(int id)
        {
            SqliteConnection connect = new SqliteConnection(DbPath);
            string commandText = @"UPDATE employees SET subdivision_id=@subdivision_id, org_id=@org_id, name=@name, position=@position, 
                                 phone=@phone, email=@email WHERE id=@id";

            if (name != "" && id>0)
            {
                SqliteCommand Command = new SqliteCommand(commandText, connect);
                Command.Parameters.AddWithValue("@id", id);
                Command.Parameters.AddWithValue("@subdivision_id", subdivision); 
                Command.Parameters.AddWithValue("@org_id", org);
                Command.Parameters.AddWithValue("@name", name);
                Command.Parameters.AddWithValue("@position", position);
                Command.Parameters.AddWithValue("@phone", phone);
                Command.Parameters.AddWithValue("@email", email);

                connect.Open();
                Command.ExecuteNonQuery();
                connect.Close();
            }

            return false;
        }

        /* [HttpPost]
         public async Task<IActionResult> Create(test.models.Employee user)
         {
             db1.employees.Add(user);
             await db1.SaveChangesAsync();
             return RedirectToAction("Index");
         }*/


        /*public IActionResult Index()
        {
            return View(db1.employees.AsAsyncEnumerable());
            //return View();
        }*/
    }
}
