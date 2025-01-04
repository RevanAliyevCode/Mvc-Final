using System;
using Business.ViewModels.News;
using Business.ViewModels.Product;

namespace Business.ViewModels.Home;

public class HomeVM
{
    public List<ProductVM> Products { get; set; }
    public List<NewsViewModel> News { get; set; }
}
