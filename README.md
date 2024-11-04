# BookManagement

## Mô tả
**BookManagement** là một trang web thương mại điện tử dành cho việc mua bán sách. Dự án cung cấp cho người dùng khả năng mua sản phẩm trực tuyến, trong khi admin có thể quản lý sản phẩm, loại sản phẩm, đơn hàng, và thực hiện các chức năng liên quan đến thanh toán. Dự án này phù hợp với những người muốn tạo ra một nền tảng bán sách trực tuyến với tính năng quản lý toàn diện.

## Yêu cầu hệ thống
- .NET (phiên bản 8 trở lên)
- Entity Framework Core
- Identity cho quản lý người dùng
- TempData để lưu trữ dữ liệu tạm thời
- Razor Pages để xây dựng giao diện người dùng

## Cài đặt và Chạy Dự án
### Bước 1: Clone dự án
```bash
git clone https://github.com/username/BookManagement.git
cd BookManagement
```

### Bước 2: Cài đặt các phụ thuộc
- Đảm bảo cài đặt .NET SDK.
- Thêm các package nuget cần thiết:
  ```bash
  dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.10
  dotnet add package Microsoft.AspNetCore.Identity.UI --version 8.0.10
  dotnet add package Microsoft.EntityFrameworkCore --version 8.0.10
  dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.10
  dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.10
  dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.10
  dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.10
  dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 8.0.6
  dotnet add package Stripe.net --version 46.3.0-beta.1
  dotnet add package Microsoft.AspNetCore.Mvc.ViewFeatures --version 2.2.0
  ```

### Bước 3: Cấu hình chuỗi kết nối
- Mở `appsettings.json` và cấu hình chuỗi kết nối đến cơ sở dữ liệu của bạn:
  ```json
  "ConnectionStrings": {
      "DefaultConnection": "Server=.;Database=BookManagementDB;Trusted_Connection=True;"
  }
  ```

### Bước 4: Khởi tạo cơ sở dữ liệu
- Chạy lệnh sau để áp dụng các migration và khởi tạo cơ sở dữ liệu:
  ```bash
  dotnet ef database update
  ```

### Bước 5: Chạy dự án
```bash
dotnet run
```

Truy cập trang web tại `http://localhost:5000`.

## Chức năng chính
1. **Quản lý Sản phẩm**:
   - Thêm, xóa, và sửa thông tin sản phẩm (bao gồm tên sách, giá, mô tả, và ảnh bìa).
   
2. **Quản lý Loại sản phẩm**:
   - Thêm, xóa, và sửa loại sản phẩm để phân loại sách theo thể loại hoặc chủ đề.

3. **Quản lý Đơn hàng**:
   - Thêm, xóa, và sửa đơn hàng; theo dõi trạng thái đơn hàng và quản lý lịch sử mua hàng.

4. **Thanh toán**:
   - Tính toán tổng tiền tự động cho đơn hàng.
   - Thanh toán trực tuyến qua **Stripe**.

5. **Quản lý Tài khoản Người dùng**:
   - Chức năng đăng ký, đăng nhập.
   - Phân quyền giữa người dùng và admin, đảm bảo chỉ admin có quyền truy cập các tính năng quản lý.

## Tác giả
Dự án được phát triển bởi KhảiBui. 
