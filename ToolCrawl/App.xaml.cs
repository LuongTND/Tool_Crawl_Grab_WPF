using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using ToolCrawl.Data;
using Microsoft.Extensions.Logging;
using ToolCrawl.Services.Merchant_Grab;
using ToolCrawl.ViewController;
using ToolCrawl.ViewModel;
using System.IO;  // Thêm dòng này để sử dụng Directory
using System.Diagnostics; // Thêm để sử dụng Process

namespace ToolCrawl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            // Đăng ký các dịch vụ và ViewModels
            var services = new ServiceCollection();

            // Đăng ký DbContext với chuỗi kết nối từ appsettings.json
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer("workstation id=DigiFnb.mssql.somee.com;packet size=4096;user id=ducluong710_SQLLogin_1;pwd=dspaz2kvaa;data source=DigiFnb.mssql.somee.com;persist security info=False;initial catalog=DigiFnb;TrustServerCertificate=True"));

            services.AddScoped<IDataGrabService, DataGrabService>();  // Đăng ký dịch vụ lấy dữ liệu
            //services.AddScoped<ApplicationDBContext>();  // Đăng ký ApplicationDBContext
            services.AddScoped<CrawlControlsViewModel>();  // Đăng ký ViewModel
            services.AddSingleton<CrawlControls>();  // Đăng ký cửa sổ chính

            _serviceProvider = services.BuildServiceProvider();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Đặt mức độ ưu tiên CPU và RAM cho ứng dụng
            SetHighPriority();

            var crawlControls = _serviceProvider.GetService<CrawlControls>();
            crawlControls.Show();

        }

        /// <summary>
        /// Đặt mức độ ưu tiên CPU và RAM cho ứng dụng
        /// </summary>
        private void SetHighPriority()
        {
            try
            {
                // Lấy tiến trình hiện tại
                var currentProcess = Process.GetCurrentProcess();

                // Đặt mức độ ưu tiên cao
                currentProcess.PriorityClass = ProcessPriorityClass.High; // Hoặc ProcessPriorityClass.RealTime nếu cần
            }
            catch (Exception ex)
            {
                // Ghi log nếu có lỗi
                Debug.WriteLine($"Error setting process priority: {ex.Message}");
            }
        }

    }

}
