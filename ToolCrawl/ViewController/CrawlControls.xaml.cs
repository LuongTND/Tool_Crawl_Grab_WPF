using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ToolCrawl.Data;
using ToolCrawl.Services.Merchant_Grab;
using ToolCrawl.ViewModel;

namespace ToolCrawl.ViewController
{
    /// <summary>
    /// Interaction logic for CrawlControls.xaml
    /// </summary>
    public partial class CrawlControls : Window
    {
        private readonly DataGrabService _service;
        private readonly ApplicationDBContext _context;
        private CancellationTokenSource _cancellationTokenSource;

        public CrawlControls(ApplicationDBContext context)
        {
            InitializeComponent();
            _context = context; // Khởi tạo context
            _service = new DataGrabService(_context); // Khởi tạo service

        }
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
        private void UpdateClock(object sender, EventArgs e)
        {
            // Cập nhật thời gian hiển thị với giờ Việt Nam
            ClockLabel.Content = DateTime.Now.ToString("HH:mm:ss");
        }
        private async void Start(object sender, RoutedEventArgs e)
        {
            // Lấy dữ liệu từ ViewModel thông qua Binding
            string username = ((CrawlControlsViewModel)DataContext).UserName;
            string password = ((CrawlControlsViewModel)DataContext).Password;

            // Kiểm tra nếu có dữ liệu
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username or password cannot be empty.");
                return;
            }

            // Hiển thị thông báo cho người dùng trong khi đang xử lý
            //MessageBox.Show("Crawling started...");

            // Đổi màu và vô hiệu hóa nút Start
            ButtonStart.Background = Brushes.Red;
            ButtonStart.IsEnabled = false;

            // Đổi màu của nút Stop để phản hồi việc dừng
            ButtonStop.Background = Brushes.Red;
            ButtonStop.IsEnabled = true;

            //try
            //{
            //    // Gọi DataGrabService để xử lý
            //    string result = await _service.CrawlDataOrderGrabAsync(username, password);

            //    // Hiển thị kết quả trả về
            //    MessageBox.Show(result);
            //}
            //catch (Exception ex)
            //{
            //    // Xử lý ngoại lệ nếu có
            //    MessageBox.Show($"An error occurred: {ex.Message}");
            //}

            // Khởi tạo token hủy để dừng quá trình sau này
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;

            // Chạy một tác vụ bất đồng bộ
            await Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        // Gọi dịch vụ CrawlDataOrderGrabAsync
                        string result = await _service.CrawlDataOrderGrabAsync(username, password);

                        // Hiển thị kết quả trả về trong CustomMessageBox trong 2 giây
                        await Dispatcher.Invoke(async () =>
                        {
                            var messageBox = new CustomMessageBox(result);
                            await messageBox.ShowAndCloseAsync(2000); // Hiển thị trong 2 giây
                        });

                        // Chờ 3 phút (180000 ms) trước khi tiếp tục, kiểm tra token trước mỗi khoảng thời gian
                        await Task.Delay(3000, token);
                    }
                    catch (TaskCanceledException)
                    {
                        // Nếu bị hủy, thoát khỏi vòng lặp
                        break;
                    }
                    catch (Exception ex)
                    {
                        // Xử lý ngoại lệ nếu có
                       // Dispatcher.Invoke(() => MessageBox.Show($"An error occurred: {ex.Message}"));

                        // Thoát khỏi vòng lặp khi xảy ra lỗi để bắt đầu lại sau
                        break;
                    }
                }
            }, token);
        }

        private async void GetData(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy dữ liệu từ cơ sở dữ liệu và sắp xếp theo CreatedAt (giảm dần)
                var historyData = await _context.Historys
                                                .OrderByDescending(h => h.CreatedAt) // Sắp xếp theo CreatedAt, giảm dần
                                                .ToListAsync();

                // Bind dữ liệu vào DataGrid
                HistoryDataGrid.ItemsSource = historyData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching data: {ex.Message}");
            }
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            // Gọi phương thức Cancel để hủy tác vụ
            _cancellationTokenSource?.Cancel();

            // Đổi màu của nút Stop để phản hồi việc dừng
            ButtonStop.Background = Brushes.Gray;
            ButtonStop.IsEnabled = false;

            // Đặt lại trạng thái cho nút Start
            ButtonStart.Background = Brushes.Green;
            ButtonStart.IsEnabled = true;
        }


    }
}
