# 💎 FinTrack Pro — Quản lý Tài chính Thông minh

**FinTrack Pro** là một ứng dụng quản lý tài chính cá nhân và tài sản đầu tư hiện đại, được xây dựng trên nền tảng .NET 8. Ứng dụng giúp bạn kiểm soát dòng tiền, lập kế hoạch ngân sách và theo dõi danh mục đầu tư một cách chuyên nghiệp và trực quan.

## 🚀 Tính năng nổi bật

### 🏦 Quản lý Dòng tiền (Cashflow)
- **Đa ví (Multi-wallets):** Quản lý nhiều ví cùng lúc (Tiền mặt, Ngân hàng, Thẻ tín dụng).
- **Ghi chép giao dịch:** Theo dõi thu nhập, chi tiêu với ghi chú, phân loại và hình ảnh đính kèm.
- **Giao dịch định kỳ:** Tự động hóa các khoản chi hàng tháng như tiền nhà, Netflix, bảo hiểm.

### 📈 Đầu tư & Tích lũy
- **Danh mục đầu tư (Portfolio):** Theo dõi Cổ phiếu (VN/Global), Crypto, Vàng và Bất động sản.
- **Tính toán lãi lỗ:** Tự động tính giá vốn trung bình và theo dõi hiệu suất đầu tư.
- **Mục tiêu tiết kiệm:** Thiết lập lộ trình cho các mục tiêu lớn (Mua xe, Du lịch, Nghỉ hưu).

### ⚖️ Công nợ & Ngân sách
- **Quản lý nợ:** Theo dõi các khoản vay và cho vay, nhắc nhở lịch trả nợ.
- **Ngân sách (Budgeting):** Thiết lập hạn mức chi tiêu cho từng danh mục để tránh vung tay quá trán.
- **Cảnh báo thông minh:** Thông báo khi chi tiêu sắp chạm ngưỡng ngân sách.

### 📊 Báo cáo & Phân tích
- Biểu đồ phân tích chi tiêu trực quan theo hạng mục và thời gian.
- Tổng kết tài sản ròng (Net Worth) theo thời gian thực.

## 💻 Công nghệ sử dụng

- **Backend:** ASP.NET Core 8.0 (MVC)
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **UI/UX:** HTML5, CSS3 (Vanilla + Modern Gradients), Bootstrap 5, Bootstrap Icons
- **Typography:** Google Fonts (Outfit)
- **Authentication:** Cookie-based Authentication
- **Hệ điều hành hỗ trợ:** Windows, Linux, MacOS

## 🛠️ Hướng dẫn Cài đặt & Chạy ứng dụng

### 1. Yêu cầu hệ thống
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB hoặc SQL Server Management Studio)

### 2. Cấu hình Database
Mở file `appsettings.json` trong thư mục project và cập nhật chuỗi kết nối của bạn:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=ExpenseManagerDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

### 3. Chạy ứng dụng
Mở terminal tại thư mục gốc của project và chạy các lệnh sau:

```bash
# Khôi phục dependencies
dotnet restore

# Chạy ứng dụng
dotnet run --project quan_ly_chi_tieu/quan_ly_chi_tieu.csproj
```

Ứng dụng sẽ mặc định chạy tại: `http://localhost:5056`

## 🎨 Giao diện ứng dụng

Giao diện được thiết kế theo phong cách **Premium Minimalist** với:
- Chế độ hiển thị sạch sẽ, hiện đại.
- Hiệu ứng kính mờ (Glassmorphism).
- Màu sắc Gradient hài hòa hỗ trợ trải nghiệm người dùng tốt nhất.

## 📝 Giấy phép (License)
Dự án được phát triển cho mục đích quản lý tài chính cá nhân. Mọi hành vi sao chép vui lòng ghi rõ nguồn.

---
*Developed with ❤️ by manhchien*
