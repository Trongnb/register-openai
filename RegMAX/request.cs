using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;

namespace RegMAX
{
    public class request
    {
        private static readonly string connString = "Server=103.183.121.44; Database=captcha; Uid=chau7tin; Pwd=0903648788";
        private static readonly string sql = "INSERT INTO hotmail (taikhoan,matkhau,create_at ) VALUES (@taikhoan, @matkhau ,current_timestamp())";
        public static void UploadAsync(string taikhoan, string matkhau)
        {


            // Tạo đối tượng MySqlConnection
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    // Mở kết nối
                    conn.Open();
                    Console.WriteLine("Kết nối thành công.");
                    // Dữ liệu mẫu, thay thế bằng dữ liệu thực từ ứng dụng của bạn


                    // Tạo đối tượng SqlCommand
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        // Thêm các tham số vào câu lệnh SQL
                        cmd.Parameters.AddWithValue("@taikhoan", taikhoan);
                        cmd.Parameters.AddWithValue("@matkhau", matkhau);

                        // Thực thi câu lệnh
                        int result = cmd.ExecuteNonQuery();

                        // Kiểm tra kết quả
                        if (result > 0)
                        {
                            Console.WriteLine("Dữ liệu đã được thêm thành công.");
                        }
                        else
                        {
                            Console.WriteLine("Không thêm được dữ liệu.");
                        }
                    }

                }
                catch (Exception ex)
                {
                    // Xử lý lỗi
                    Console.WriteLine("Đã xảy ra lỗi khi kết nối đến MySQL: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }


        }


        public static string get(string url)
        {
            try
            {
                var http = new HttpRequest();
                var res = http.Get(url).ToString();
                return res;
            }
            catch
            {
                return null;
            }
        }
    }
}
