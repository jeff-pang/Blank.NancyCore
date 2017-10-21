using Blank.NancyCore.Abstract;
using Nancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blank.NancyCore.Core.RestApi
{
    public class MenuApi : NancyModule
    {
        IMenuBuilder _menuBuilder;
        public MenuApi(IMenuBuilder menuBuilder):base("/api")
        {
            _menuBuilder = menuBuilder;

            Get("/menu", p => {

                var menu = _menuBuilder.GetMenu();

                return Response.AsJson(menu);
            });
        }
    }
}