//using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace test.models
{
    public class Employee //: PageModel 
    {
        [BindProperty(SupportsGet = true)]
        public int id { get; set; }
       [BindProperty(SupportsGet = true)]
        public int subdivision_id { get; set; }
        [BindProperty(SupportsGet = true)]
        public int org_id { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }
        [BindProperty]
        public string position { get; set; }
        [BindProperty]
        public string phone { get; set; }
        [BindProperty]
        public string email { get; set; }

    }
}
