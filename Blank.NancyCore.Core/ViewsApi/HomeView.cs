using Nancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blank.NancyCore.Core.ViewsApi
{
    public class HomeView : NancyModule
    {
        public HomeView()
        {
            Get("{message?}", p => {

                string message = "Hello";

                string m = p.message;
                if(!string.IsNullOrEmpty(m))
                {
                    message = m;
                }

                var model = new {
                    Message = message
                };

                return View["Home.html", model];
            });
        }
    }
}
