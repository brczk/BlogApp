using ConsoleTools;
using BlogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace BlogApp.Client
{
    internal class Program
    {
        static RestService rest;
        delegate object GetPropValue(string propName);

        static void Create<T>(GetPropValue method)
        {
            Type t = typeof(T);
            object instance = Activator.CreateInstance(t);
            foreach (var prop in t.GetProperties())
            {
                if(prop.GetAccessors().FirstOrDefault(t => t.IsVirtual) == null)
                {
                    object propValue = method(prop.Name);
                    Type propType = prop.PropertyType;

                    if (propType == typeof(string))
                    {
                        prop.SetValue(instance, propValue);
                    }
                    else
                    {
                        var parseMethod = propType.GetMethods().First(t => t.Name.Contains("Parse"));
                        var parsedValue = parseMethod.Invoke(null, new object[] { propValue });
                        prop.SetValue(instance, parsedValue);
                    }
                }
            }
            rest.Post(instance, t.Name);
        }

        static void ReadAll<T>()
        {
            List<T> items = rest.Get<T>(typeof(T).Name);
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }


        static void Update<T>(GetPropValue method)
        {
            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());
            Type t = typeof(T);
            T item = rest.Get<T>(id, t.Name);
            
            foreach (var prop in t.GetProperties())
            {
                if (prop.GetAccessors().FirstOrDefault(t => t.IsVirtual) == null)
                {
                    object propValue = method($"{prop.Name} (current: {prop.GetValue(item)})");
                    Type propType = prop.PropertyType;

                    if (propType == typeof(string))
                    {
                        prop.SetValue(item, propValue);
                    }
                    else
                    {
                        var parseMethod = propType.GetMethods().First(t => t.Name.Contains("Parse"));
                        var parsedValue = parseMethod.Invoke(null, new object[] { propValue });
                        prop.SetValue(item, parsedValue);
                    }
                }
            }

            rest.Put(item, t.Name);
        }

        static void Delete<T>()
        {
            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());
            rest.Delete(id, typeof(T).Name);
        }

        static string ReadProp(string propName)
        {
            Console.Write($"{propName}: ");
            string value = Console.ReadLine();
            return value;
        }


        static void Main(string[] args)
        {
            rest = new RestService("http://localhost:5828/", "blog");

            var blogSubMenu = new ConsoleMenu(args, level: 1)
                .Add("Create", () => Create<Blog>( ReadProp))
                .Add("ReadAll", () => ReadAll<Blog>())
                .Add("Update", () => Update<Blog>(ReadProp))
                .Add("Delete", () => Delete<Blog>())
                .Add("Exit", ConsoleMenu.Close);

            var postSubMenu = new ConsoleMenu(args, level: 1)
                .Add("Create", () => Create<Post>(ReadProp))
                .Add("ReadAll", () => ReadAll<Post>())
                .Add("Update", () => Update<Post>(ReadProp))
                .Add("Delete", () => Delete<Blog>())
                .Add("Exit", ConsoleMenu.Close);

            var commentSubMenu = new ConsoleMenu(args, level: 1)
                .Add("Create", () => Create<Comment>(ReadProp))
                .Add("ReadAll", () => ReadAll<Comment>())
                .Add("Update", () => Update<Comment>(ReadProp))
                .Add("Delete", () => Delete<Blog>())
                .Add("Exit", ConsoleMenu.Close);


            var menu = new ConsoleMenu(args, level: 0)
                .Add("Blogs", () => blogSubMenu.Show())
                .Add("Posts", () => postSubMenu.Show())
                .Add("Comments", () => commentSubMenu.Show())
                .Add("Exit", ConsoleMenu.Close);

            menu.Show();
        }
    }
}
