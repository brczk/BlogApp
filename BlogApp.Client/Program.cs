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
        delegate void CRUD(GetPropValue method);

        static void Wrapper(CRUD action, GetPropValue method)
        {
            try
            {
                action(method);
            }
            catch (Exception ex) when (ex is TargetInvocationException || ex is FormatException)
            {
                Console.WriteLine("Parse error.");
            }
            catch (Exception ex) when (ex is ArgumentException || ex is NullReferenceException)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Press any key...");
                Console.ReadLine();
            }
        }

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

        #region Read
        static void Read<T>(GetPropValue method)
        {
            string id = method("ID").ToString();
            T item = rest.Get<T>(int.Parse(id), typeof(T).Name);
            Console.Clear();
            Console.WriteLine(item);
        }

        static void ReadAll<T>()
        {
            Console.Clear();
            List<T> items = rest.Get<T>(typeof(T).Name);
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Press any key...");
            Console.ReadLine();
        }

        static void ReadStat(string statEndpoint)
        {
            Console.Clear();
            List<dynamic> items = rest.Get<dynamic>($"Stats/{statEndpoint}");
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Press any key...");
            Console.ReadLine();
        }
        #endregion

        static void Update<T>(GetPropValue method)
        {
            Type t = typeof(T);
            string id = method("ID").ToString();
            T item = rest.Get<T>(int.Parse(id), t.Name);
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

        
        static void Main(string[] args)
        {
            rest = new RestService("http://localhost:5828/", "blog");

            var blogSubMenu = new ConsoleMenu(args, level: 1)
                .Add("Create", () => Wrapper(Create<Blog>,PropInput))
                .Add("Read", () => Wrapper(Read<Blog>, PropInput))
                .Add("ReadAll", () => ReadAll<Blog>())
                .Add("Update", () => Wrapper(Update<Blog>, PropInput))
                .Add("Delete", () => Wrapper(Delete<Blog>, PropInput))
                .Add("Exit", ConsoleMenu.Close);

            var postSubMenu = new ConsoleMenu(args, level: 1)
                .Add("Create", () => Wrapper(Create<Post>, PropInput))
                .Add("Read", () => Wrapper(Read<Post>, PropInput))
                .Add("ReadAll", () => ReadAll<Post>())
                .Add("Update", () => Wrapper(Update<Post>, PropInput))
                .Add("Delete", () => Wrapper(Delete<Post>, PropInput))
                .Add("Exit", ConsoleMenu.Close);

            var commentSubMenu = new ConsoleMenu(args, level: 1)
                .Add("Create", () => Wrapper(Create<Comment>, PropInput))
                .Add("Read", () => Wrapper(Read<Comment>, PropInput))
                .Add("ReadAll", () => ReadAll<Comment>())
                .Add("Update", () => Wrapper(Update<Comment>, PropInput))
                .Add("Delete", () => Wrapper(Delete<Comment>, PropInput))
                .Add("Exit", ConsoleMenu.Close);

            var statSubMenu = new ConsoleMenu(args, level: 1)
                .Add("GetAvgNumberOfComments", () => ReadStat("GetAvgNumberOfComments"))
                .Add("GetBlogRankingsByPopularity", () => ReadStat("GetBlogRankingsByPopularity"))
                .Add("GetMostPopularPostPerBlog", () => ReadStat("GetMostPopularPostPerBlog"))
                .Add("GetPostsCountPerCategory", () => ReadStat("GetPostsCountPerCategory"))
                .Add("GetAverageRatingOfPostsPerCategory", () => ReadStat("GetAverageRatingOfPostsPerCategory"))
                .Add("Exit", ConsoleMenu.Close);


            var menu = new ConsoleMenu(args, level: 0)
                .Add("Blogs", () => blogSubMenu.Show())
                .Add("Posts", () => postSubMenu.Show())
                .Add("Comments", () => commentSubMenu.Show())
                .Add("Stats", () => statSubMenu.Show())
                .Add("Exit", ConsoleMenu.Close);

            menu.Show();
        }

        static string PropInput(string propName)
        {
            Console.Clear();
            Console.Write($"{propName}: ");
            string value = Console.ReadLine();
            return value;
        }
    }
}
