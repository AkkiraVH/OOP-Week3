/****************/
// Mã sinh viên: 202418910
// Họ tên: Đoàn Việt Hùng
/****************/

using System;
using System.Runtime.CompilerServices;

namespace Lab03_04_Overloading
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== BẮT ĐẦU KIỂM THỬ ===\n");

            // 1. Tạo hai Employee bằng hai constructor khác nhau
            Console.WriteLine("--- Bước 1-2: Tạo đối tượng ---");
            Employee e1 = new Employee("E01", "Nguyễn Văn A");
            Employee e2 = new Employee("E02", "Trần Thị B", 1000);

            // 2. Tạo hai SoftwareEngineer bằng hai constructor khác nhau
            SoftwareEngineer se1 = new SoftwareEngineer("SE01", "Lê Văn C", "C#");
            SoftwareEngineer se2 = new SoftwareEngineer("SE02", "Phạm Thị D", 1500, "Java", 500);
            e1.DisplayInfo(); e2.DisplayInfo(); se1.DisplayInfo(); se2.DisplayInfo();

            // 3. Tăng lương bằng số tiền cố định
            Console.WriteLine("\n--- Bước 3-4: Tăng lương ---");
            e1.IncreaseSalary(300);

            // 4. Tăng lương theo phần trăm
            e2.IncreaseSalary(20, true);

            // 5. Tạo nhóm dự án không có trưởng nhóm
            Console.WriteLine("\n--- Bước 5-8: Thao tác thêm thành viên ---");
            ProjectTeam team1 = new ProjectTeam("P01", "Dự án Alpha");

            // 6. Thêm một nhân sự vào nhóm
            team1.AddMember(e1);

            // 7. Thêm một kỹ sư và đặt làm trưởng nhóm
            team1.AddMember(se1, true);

            // 8. Thử thêm lại một thành viên đã tồn tại
            team1.AddMember(e1);

            // 9. Hiển thị danh sách bằng lời gọi đa hình
            Console.WriteLine("\n--- Bước 9-10: Hiển thị và tính chi phí ---");
            team1.DisplayTeam();

            // 10. Tính tổng chi phí nhân sự hằng tháng
            Console.WriteLine($"Tổng chi phí hằng tháng của {team1.ProjectCode}: {team1.CalculateTotalMonthlyCost()}");

            // 11. Thử xóa trưởng nhóm hiện tại (bị từ chối)
            Console.WriteLine("\n--- Bước 11-12: Xóa / đổi trưởng nhóm ---");
            team1.RemoveMember(se1.Id);

            // 12. Đổi trưởng nhóm rồi xóa người từng là trưởng nhóm
            team1.ChangeLeader(se2);
            team1.RemoveMember(se1.Id);
            team1.DisplayTeam();

            // 13-14. Nhóm thứ hai sống trong phạm vi một method riêng, method kết thúc thì nhóm bị huỷ
            RunSecondTeamScope(e1);

            // Ép GC chạy sau khi method đã return để finalizer của team2 được gọi
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // 15. Nhân sự của nhóm thứ hai vẫn tồn tại sau khi nhóm bị huỷ
            Console.WriteLine("\n--- Bước 15: Sau khi nhóm P02 bị huỷ ---");
            Console.WriteLine("Thông tin nhân sự e1 (từng thuộc nhóm P02, vẫn còn thuộc P01):");
            e1.DisplayInfo();
            Console.WriteLine($"P01 vẫn còn e1? {team1.Contains(e1.Id)}");

            // Các trường hợp biên
            RunEdgeCases(e1, se2, team1);

            Console.WriteLine("\n=== KẾT THÚC KIỂM THỬ ===");
            Console.ReadLine();
        }

        // NoInlining: tránh JIT gộp method vào Main làm team2 "sống" đến hết Main
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void RunSecondTeamScope(Employee sharedEmployee)
        {
            // 13. Tạo nhóm thứ hai, thêm nhân sự đã có ở nhóm thứ nhất (kết tập nhiều nhóm)
            Console.WriteLine("\n--- Bước 13: Tạo nhóm thứ hai ---");
            ProjectTeam team2 = new ProjectTeam("P02", "Dự án Beta");
            team2.AddMember(sharedEmployee);
            team2.DisplayTeam();

            // 14. Hết method => team2 không còn được tham chiếu => sẵn sàng bị huỷ
            Console.WriteLine("Kết thúc phạm vi của nhóm P02 (chuẩn bị huỷ)...");
        }

        static void RunEdgeCases(Employee e1, SoftwareEngineer se2, ProjectTeam team1)
        {
            Console.WriteLine("\n=== TRƯỜNG HỢP BIÊN ===");

            Try("Employee mã rỗng", () => new Employee("", "An"));
            Try("Employee họ tên toàn khoảng trắng", () => new Employee("E99", "   "));
            Try("Employee lương âm", () => new Employee("E99", "An", -1));
            Try("Gán BaseSalary âm", () => e1.BaseSalary = -50);
            Try("IncreaseSalary(0)", () => e1.IncreaseSalary(0));
            Try("IncreaseSalary(-5)", () => e1.IncreaseSalary(-5));
            Try("IncreaseSalary(-10, true)", () => e1.IncreaseSalary(-10, true));
            Try("IncreaseSalary(100, false) - số tiền cố định", () => e1.IncreaseSalary(100, false));
            Try("SoftwareEngineer ngôn ngữ rỗng", () => new SoftwareEngineer("SE99", "X", ""));
            Try("SoftwareEngineer phụ cấp âm", () => new SoftwareEngineer("SE99", "X", 100, "C#", -1));
            Try("AddMember(null)", () => team1.AddMember(null));
            Try("ChangeLeader(null)", () => team1.ChangeLeader(null));
            Try("ProjectTeam mã dự án rỗng", () => new ProjectTeam("", "Tên"));
            Try("ProjectTeam(constructor 3) với leader null", () => new ProjectTeam("P03", "Gamma", null));
            Try("RemoveMember mã không tồn tại", () => team1.RemoveMember("KHONG-CO"));
            Try("ChangeLeader sang chính trưởng nhóm hiện tại", () => team1.ChangeLeader(se2));
            Try("Thêm trùng mã (đối tượng khác, cùng Id)",
                () => team1.AddMember(new Employee("E01", "Người trùng mã")));
            Try("Xóa trưởng nhóm duy nhất khi chưa thay thế", () => team1.RemoveMember(team1.Leader.Id));
        }

        static void Try(string description, Action action)
        {
            Console.WriteLine($"\n[Test] {description}");
            try
            {
                action();
                Console.WriteLine("  -> Không ném exception (đã xử lý bằng thông báo hoặc hợp lệ).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  -> {ex.GetType().Name}: {ex.Message}");
            }
        }
    }
}
