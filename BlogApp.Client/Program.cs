using ConsoleTools;
using BlogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Xml.Linq;

namespace BlogApp.Client
{
    internal class Program
    {
        static RestService rest;
        delegate object GetPropValue(string propName);
        delegate void CUD(GetPropValue method);

        static void Create<T>(GetPropValue method)
        {
            Type t = typeof(T);
            object instance = Activator.CreateInstance(t);
            foreach (var prop in t.GetProperties())
            {
                if (prop.GetAccessors().FirstOrDefault(t => t.IsVirtual) == null)
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

        static void ReadAll<T>(string stat)
        {
            Console.Clear();
            List<T> items = rest.Get<T>($"Stats/{stat}");
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }

        static void CUDWrapper(CUD action, GetPropValue method)
        {
            try
            {
                action(method);
            }
            catch (Exception ex) when (ex is TargetInvocationException || ex is FormatException)
            {
                Console.WriteLine("Parse error.");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        static void Update<T>(GetPropValue method)
        {
            Type t = typeof(T);
            Console.Clear();
            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());
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

        static void Delete<T>(GetPropValue method)
        {
            string id = method("ID").ToString();
            rest.Delete(int.Parse(id), typeof(T).Name);
        }

        static string ReadProp(string propName)
        {
            Console.Clear();
            Console.Write($"{propName}: ");
            string value = Console.ReadLine();
            return value;
        }


        static void Main(string[] args)
        {
            rest = new RestService("http://localhost:5828/", "blog");

            var blogSubMenu = new ConsoleMenu(args, level: 1)
                .Add("Create", () => CUDWrapper(Create<Blog>,ReadProp))
                .Add("ReadAll", () => ReadAll<Blog>())
                .Add("Update", () => CUDWrapper(Update<Blog>, ReadProp))
                .Add("Delete", () => CUDWrapper(Delete<Blog>, ReadProp))
                .Add("Exit", ConsoleMenu.Close);

            var postSubMenu = new ConsoleMenu(args, level: 1)
                .Add("Create", () => CUDWrapper(Create<Post>, ReadProp))
                .Add("ReadAll", () => ReadAll<Post>())
                .Add("Update", () => CUDWrapper(Update<Post>, ReadProp))
                .Add("Delete", () => CUDWrapper(Delete<Post>, ReadProp))
                .Add("Exit", ConsoleMenu.Close);

            var commentSubMenu = new ConsoleMenu(args, level: 1)
                .Add("Create", () => CUDWrapper(Create<Comment>, ReadProp))
                .Add("ReadAll", () => ReadAll<Comment>())
                .Add("Update", () => CUDWrapper(Update<Comment>, ReadProp))
                .Add("Delete", () => CUDWrapper(Delete<Comment>, ReadProp))
                .Add("Exit", ConsoleMenu.Close);

            var statSubMenu = new ConsoleMenu(args, level: 1)
                .Add("GetAvgNumberOfComments", () => ReadAll<dynamic>("GetAvgNumberOfComments"))
                .Add("GetBlogRankingsByPopularity", () => ReadAll<dynamic>("GetBlogRankingsByPopularity"))
                .Add("GetMostPopularPostPerBlog", () => ReadAll<dynamic>("GetMostPopularPostPerBlog"))
                .Add("GetPostsCountPerCategory", () => ReadAll<dynamic>("GetPostsCountPerCategory"))
                .Add("GetAverageRatingOfPostsPerCategory", () => ReadAll<dynamic> ("GetAverageRatingOfPostsPerCategory"))
                .Add("Exit", ConsoleMenu.Close);


            var menu = new ConsoleMenu(args, level: 0)
                .Add("Blogs", () => blogSubMenu.Show())
                .Add("Posts", () => postSubMenu.Show())
                .Add("Comments", () => commentSubMenu.Show())
                .Add("Stats", () => statSubMenu.Show())
                .Add("Exit", ConsoleMenu.Close);

            menu.Show();
        }
    }
}
