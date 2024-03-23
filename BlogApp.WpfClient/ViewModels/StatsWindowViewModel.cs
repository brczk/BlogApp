using BlogApp.Models;
using BlogApp.Models.Helpers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BlogApp.WpfClient.ViewModels
{
    public class StatsWindowViewModel : ObservableRecipient
    {
        public StatsWindowViewModel()
        {
            var rest = new RestService("http://localhost:5828/");
            Stat1 = rest.Get<AvgNumberOfCommentsInfo>("Stats/GetAvgNumberOfComments");
            Stat2 = rest.Get<BlogRankingInfo>("Stats/GetBlogRankingsByPopularity");
            Stat3 = rest.Get<CategoryAvgPostRatingInfo>("Stats/GetAverageRatingOfPostsPerCategory");
            Stat4 = rest.Get<CategoryPostCountInfo>("Stats/GetPostsCountPerCategory");
            Stat5 = rest.Get<MostPopularPostInfo>("Stats/GetMostPopularPostPerBlog");
        }

        public List<AvgNumberOfCommentsInfo> Stat1
        {
            get; set;
        }
        public List<BlogRankingInfo> Stat2
        {
            get; set;
        }
        public List<CategoryAvgPostRatingInfo> Stat3
        {
            get; set;
        }
        public List<CategoryPostCountInfo> Stat4
        {
            get; set;
        }
        public List<MostPopularPostInfo> Stat5
        {
            get; set;
        }
    }
}
