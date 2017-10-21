using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Dapper.FluentMap;
using Blank.NancyCore.Abstract;
using Blank.NancyCore.Impl.DapperMaps;
using Blank.NancyCore.Abstract.Configurations;
using Blank.NancyCore.Abstract.DataModels;

namespace Blank.NancyCore.Impl
{
    public class MySqlMenuBuilder : IMenuBuilder
    {
        public string connStr;
        static MySqlMenuBuilder()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new MenuMap());
            });
        }

        public MySqlMenuBuilder()
        {
            connStr = SystemConfig.GetConfig<Dictionary<string,string>>("mysql")["ConnectionString"];
        }

        public MainMenu GetMenu()
        {
            string selectMenu = @"SELECT * FROM menu;";

            var mainMenu = new MainMenu();
            return mainMenu;
            //using (var conn = new MySqlConnection(connStr))
            //{
            //    conn.Open();
            //    var menu = conn.Query<Menu>(selectMenu);
            //    conn.Close();
            //    mainMenu.Menus = menu;
            //    return mainMenu;
            //}
        }
    }
}
