/****************/
// Mã sinh viên: 202418910
// Họ tên: Đoàn Việt Hùng
/****************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab03_04_Overloading
{
    /// <summary>
    /// Nhóm dự án. Quan hệ KẾT TẬP (aggregation) với Employee: nhóm chỉ giữ tham chiếu,
    /// không sở hữu nên huỷ nhóm không huỷ nhân sự.
    /// Bất biến:
    ///  (1) Không có hai thành viên cùng mã trong một nhóm.
    ///  (2) Trưởng nhóm (nếu có) luôn nằm trong danh sách thành viên, và chỉ xuất hiện một lần.
    /// </summary>
    public class ProjectTeam
    {
        private readonly List<Employee> members = new List<Employee>();

        public string ProjectCode { get; private set; }
        public string ProjectName { get; private set; }

        /// <summary>Trưởng nhóm (liên kết không sở hữu), null nếu chưa có.</summary>
        public Employee Leader { get; private set; }

        // ----- 2 constructor (nạp chồng) -----

        public ProjectTeam(string projectCode, string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectCode))
                throw new ArgumentException("Mã dự án không được để trống.");
            if (string.IsNullOrWhiteSpace(projectName))
                throw new ArgumentException("Tên dự án không được để trống.");

            ProjectCode = projectCode.Trim();
            ProjectName = projectName.Trim();
        }

        /// <summary>Thiết lập trưởng nhóm và tự đưa trưởng nhóm vào danh sách thành viên.</summary>
        public ProjectTeam(string projectCode, string projectName, Employee leader)
            : this(projectCode, projectName)
        {
            AddMember(leader, true); // ném ArgumentNullException nếu leader == null
        }

        // ----- Truy vấn -----

        public bool Contains(string employeeId)
        {
            return !string.IsNullOrWhiteSpace(employeeId) &&
                   members.Any(m => m.Id == employeeId.Trim());
        }

        public int MemberCount { get { return members.Count; } }

        // ----- Nạp chồng addMember -----

        /// <summary>Thêm thành viên. Trả về false nếu đã tồn tại (trùng mã).</summary>
        public bool AddMember(Employee employee)
        {
            if (employee == null) throw new ArgumentNullException("employee");

            if (Contains(employee.Id))
            {
                Console.WriteLine($"Thất bại: Nhân sự {employee.Id} đã tồn tại trong nhóm {ProjectCode}.");
                return false;
            }
            members.Add(employee);
            return true;
        }

        /// <summary>
        /// makeLeader = true: đảm bảo nhân sự có trong nhóm rồi đặt làm trưởng nhóm.
        /// Trưởng nhóm cũ vẫn là thành viên. Trả về true nếu có thay đổi trạng thái nhóm.
        /// </summary>
        public bool AddMember(Employee employee, bool makeLeader)
        {
            if (employee == null) throw new ArgumentNullException("employee");

            if (!makeLeader)
                return AddMember(employee);

            bool added = false;
            if (!Contains(employee.Id))
            {
                members.Add(employee);
                added = true;
            }

            // Nếu đã có người trùng mã trong nhóm, dùng đúng đối tượng đang nằm trong nhóm
            Employee inTeam = members.First(m => m.Id == employee.Id);
            bool leaderChanged = !ReferenceEquals(Leader, inTeam);
            Leader = inTeam;

            if (leaderChanged)
                Console.WriteLine($"Đã bổ nhiệm {inTeam.Id} làm trưởng nhóm {ProjectCode}.");
            else
                Console.WriteLine($"{inTeam.Id} đã là trưởng nhóm {ProjectCode} từ trước.");

            return added || leaderChanged;
        }

        // ----- Các phương thức khác -----

        public void RemoveMember(string employeeId)
        {
            if (!Contains(employeeId))
            {
                Console.WriteLine($"Lỗi: Không tìm thấy thành viên '{employeeId}' trong nhóm {ProjectCode}.");
                return;
            }

            if (Leader != null && Leader.Id == employeeId.Trim())
            {
                Console.WriteLine($"Từ chối: Không được xóa trưởng nhóm {employeeId} khi chưa chọn trưởng nhóm thay thế.");
                return;
            }

            members.RemoveAll(m => m.Id == employeeId.Trim());
            Console.WriteLine($"Đã xóa thành viên {employeeId} khỏi nhóm {ProjectCode}.");
        }

        public void ChangeLeader(Employee newLeader)
        {
            if (newLeader == null) throw new ArgumentNullException("newLeader");

            if (Leader != null && Leader.Id == newLeader.Id)
            {
                Console.WriteLine($"{newLeader.Id} đã là trưởng nhóm {ProjectCode}, không có thay đổi.");
                return;
            }

            // Trưởng nhóm mới phải thuộc nhóm; nếu chưa thì tự thêm vào.
            // Trưởng nhóm cũ vẫn giữ nguyên trong danh sách thành viên.
            AddMember(newLeader, true);
            Console.WriteLine($"Đã thay đổi trưởng nhóm {ProjectCode} thành {newLeader.Id}.");
        }

        public double CalculateTotalMonthlyCost()
        {
            double total = 0;
            foreach (Employee member in members)
                total += member.CalculateMonthlyCost(); // đa hình
            return total;
        }

        public void DisplayTeam()
        {
            Console.WriteLine($"\n--- Dự án: {ProjectName} ({ProjectCode}) ---");
            Console.WriteLine($"Trưởng nhóm: {(Leader != null ? Leader.FullName : "Chưa có")}");
            Console.WriteLine($"Số lượng thành viên: {members.Count}");
            Console.WriteLine("Danh sách thành viên:");
            foreach (Employee member in members)
                member.DisplayInfo(); // gọi đa hình
            Console.WriteLine("------------------------------------------");
        }

        /// <summary>
        /// Finalizer: chỉ dọn danh sách liên kết nội bộ, KHÔNG huỷ các đối tượng Employee.
        /// </summary>
        ~ProjectTeam()
        {
            members.Clear();
            Console.WriteLine($"[Destructor] Đã dọn dẹp danh sách nội bộ của nhóm dự án: {ProjectCode}");
        }
    }
}
