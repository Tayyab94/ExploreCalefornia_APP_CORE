using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCalefornia.Models;
using Microsoft.AspNetCore.Mvc;


namespace ExploreCalefornia.ViewComponents
{
    [ViewComponent]
    public class MonthlySpecialsViewComponent :ViewComponent
    {
        private readonly ExploreCaleforiniaContext context;

        public MonthlySpecialsViewComponent(ExploreCaleforiniaContext context)
        {
            this.context = context;
        }
        public IViewComponentResult Invoke()
        {
            var specials = context.MonthlySpecials.ToArray();
            return View(specials);
        }
    }
}
