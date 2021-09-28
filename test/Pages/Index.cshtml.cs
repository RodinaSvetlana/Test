using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Data.Sqlite;
using test.controller;

namespace test.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        //public string Message { get; private set; } = "PageModel in C# Превед ";
        public List<int> id { get; set; }
        public List<int> subdivision_id { get; set; } //список с id
        public List<int> org_id { get; set; } //список с id 
        public List<string> Name { get; set; }
        public List<string> position { get; set; }
        
        public List<string> phone { get; set; }
        public List<string> email { get; set; }
        public List<string> subdivisionEmployee { get; set; } = new List<string>(); //вывод в таблицу сотрудники
        public List<string> orgEmployee { get; set; } = new List<string>(); //вывод в таблицу сотрудники
        //public List<KeyValuePair<int, string>> subdivisionEmployee=new List<KeyValuePair<int, string>>();//можно дублировать ключ-значение,set не работает
        //public List<string> orgstructureEmployee { get; set; }

        public List<string> listSubdivisions { get; set; }
        public List<string> listOrgstructure { get; set; }

        public Dictionary<int, string> subdivisionsDictionary { get; set; } = new Dictionary<int, string>();//нельзя дублировать ключ-зн.
        public Dictionary<int, string> orgDictionary { get; set; } = new Dictionary<int, string>();
        
        //public List<KeyValuePair<int, string>> subdivisionsDictionary = new List<KeyValuePair<int, string>>();//можно дублировать ключ-значение
        //public List<KeyValuePair<int, string>> orgDictionary = new List<KeyValuePair<int, string>>();

        //public EmployeeList employeeList = new EmployeeList();

        public  userContext db = new userContext();  //контекст базы данных

        public void loadEmployees()
        {
            //var employee = from b in db.Employees
            //           where b.Name.StartsWith('И')
            //           select b;
            //var employee = (from c in db.Employees where c.employee_id == userid select c).First();
            //var employee = (from c in db.employees select c).First(); //работает
            //var empls = (from c in db.employees select c).AsAsyncEnumerable(); //работает

            id = (from c in db.employees select c.id).ToList();
            Name = (from c in db.employees select c.Name).ToList();
            position = (from c in db.employees select c.position).ToList();
            phone = (from c in db.employees select c.phone).ToList();
            email = (from c in db.employees select c.email).ToList();
            subdivision_id = (from c in db.employees select c.subdivision_id).ToList();
            org_id = (from c in db.employees select c.org_id).ToList();

            //subdivisionEmployee = (from c in db.employees select c.subdivision_id)
            //from c in db.subdivisions select c.name).Where<>

            //keySubdivisions = (from c in db.subdivisions select c.id).ToList();
            //valSubdivisions = (from c in db.subdivisions select c.name).ToList();
            listSubdivisions = (from c in db.subdivisions select c.name).ToList(); //вывод в select
            listOrgstructure = (from c in db.orgstructure select c.name).ToList(); //вывод в select

            //subdivisionEmployee = ((from c in db.employees select c.subdivision_id).ToList());

            for (int i = 0; i < listSubdivisions.Count; i++)
            {
                /* int k = (from c in db.employees select c.subdivision_id).ToList()[i];
                 string name = (from b in db.subdivisions
                                where b.id == k
                                select b.name).First();*/

                int k = (from b in db.subdivisions select b.id).ToList()[i];
                //string name = (from b in db.subdivisions where b.id == k select b.name).ToList()[i];
                string name = (from b in db.subdivisions select b.name).ToList()[i];
                subdivisionsDictionary.Add(k, name);

                //subdivisionsDictionary.Add(new KeyValuePair<int, string>(k, name));

            }
            for (int i = 0; i < listOrgstructure.Count; i++)
            {
                int k = (from b in db.orgstructure select b.id).ToList()[i];
                //string orgname = (from b in db.orgstructure where b.id == k select b.name).ToList()[i];
                string orgname = (from b in db.orgstructure select b.name).ToList()[i];

                //orgDictionary.Add(new KeyValuePair<int, string>(k, orgname));
                orgDictionary.Add(k, orgname);
            }
            for (int i = 0; i < Name.Count; i++)
            {
                int k = subdivision_id[i];
                string name = (from b in db.subdivisions where b.id == k select b.name).First();
                subdivisionEmployee.Add(name); //в таблицу сотрудники лист с отделами
                
                int o = org_id[i];
                string org = (from b in db.orgstructure where b.id == o select b.name).First();
                orgEmployee.Add(org);
            }
        }
        public void OnGet()
        {
            loadEmployees();
            //string Message = $" Server time is { DateTime.Now }";
        }

        public class userContext : DbContext
        {
            public DbSet<test.models.Employee> employees { get; set; }
            public DbSet<test.models.Orgstructure> orgstructure { get; set; }
            public DbSet<test.models.Subdivisions> subdivisions { get; set; }

            public string DbPath { get; private set; }

            //public userContext(DbContextOptions<userContext> ctx):base(ctx)
            public userContext()
            {
               
                //var folder = Envi1ronment.SpecialFolder.LocalApplicationData;
                //var path = Environment.GetFolderPath(folder);
                
                var path = Environment.CurrentDirectory; //work
                DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}users.db"; //work

            }

            // The following configures EF to create a Sqlite database file in the
            // special "local" folder for your platform.
            protected override void OnConfiguring(DbContextOptionsBuilder options)
                => options.UseSqlite($"Data Source={DbPath}");

        }

        public void OnPost()
        {
            loadEmployees();

            //string headers = Request.QueryString.Value.ToString();
            //public List<KeyValuePair<object, object>> test = new List<KeyValuePair<object, object>>();
            //int k = Request.HttpContext.Items.Count;
            //var roomName = document.getElementByID('somevalue').value;

        }
        public void OnPostCreate(int subdivision_id, int org_id, string Name, string position, string phone, string email)
        {
            //string test = id.Count.ToString();
            if (id is null) loadEmployees();

            UserController myController = new test.controller.UserController(db);

            myController.Create(subdivision_id.ToString(), org_id.ToString(), Name, position, phone, email);

            Response.Redirect("/Index");
        }
        public void OnPostDelete(int id)
        {
            if (Name is null) loadEmployees();

             UserController myController = new test.controller.UserController(db);

            myController.Delete(id);

            Response.Redirect("/Index");
        }

        public void OnPostUpdate(int id, int subdivision_id, int org_id, string nameUpd, string posUpd, string phUpd, string emUpd)
        {

            if (Name is null) loadEmployees();

            UserController myController = new test.controller.UserController(db);

            myController.Update(id, subdivision_id, org_id, nameUpd, posUpd, phUpd, emUpd);

            Response.Redirect("/Index");
        }
        
        /* public class employeeParam 
          {
               public int id { get; set; }
               public int subdivision_id { get; set; }
              public string Name { get; set; }
               public string position { get; set; }

               public List<string> phone { get; set; }
               public List<string> email { get; set; }
           }*/
        //public class employeeList :List<EmployeeList>{ public List<string> Name { get; set; } }
    }
}
