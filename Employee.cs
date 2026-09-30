/****************/
// Mã sinh viên: 202418910
// Họ tên: Đoàn Việt Hùng
/****************/

using System;

namespace Lab03_04_Overloading
{
    /// <summary>
    /// Nhân sự thông thường. Tồn tại độc lập với dự án.
    /// Bất biến: Id và FullName không rỗng; BaseSalary >= 0.
    /// </summary>
    public class Employee
    {
        private double baseSalary;

        /// <summary>Mã nhân sự (không rỗng, không đổi sau khi tạo).</summary>
        public string Id { get; private set; }

        /// <summary>Họ tên (không rỗng, không đổi sau khi tạo).</summary>
        public string FullName { get; private set; }

        /// <summary>Lương cơ bản, luôn >= 0.</summary>
        public double BaseSalary
        {
            get { return baseSalary; }
            set
            {
                if (value < 0 || double.IsNaN(value) || double.IsInfinity(value))
                    throw new ArgumentException("Lương cơ bản không được âm (hoặc không hợp lệ).");
                baseSalary = value;
            }
        }

        // ----- 3 constructor (nạp chồng) -----

        /// <summary>Mặc định: UNKNOWN / Unnamed employee / 0.</summary>
        public Employee() : this("UNKNOWN", "Unnamed employee", 0) { }

        public Employee(string id, string fullName) : this(id, fullName, 0) { }

        public Employee(string id, string fullName, double baseSalary)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Mã nhân sự không được để trống.");
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Họ tên không được để trống.");

            Id = id.Trim();
            FullName = fullName.Trim();
            BaseSalary = baseSalary; // setter kiểm tra >= 0
        }

        // ----- Nạp chồng increaseSalary -----

        /// <summary>Tăng lương theo số tiền cố định.</summary>
        public void IncreaseSalary(double amount)
        {
            if (amount <= 0 || double.IsNaN(amount) || double.IsInfinity(amount))
                throw new ArgumentException("Giá trị tăng phải dương.");
            BaseSalary += amount;
            Console.WriteLine($"[{Id}] Đã tăng lương thêm {amount}. Lương mới: {BaseSalary}");
        }

        /// <summary>
        /// byPercentage = true: tăng theo phần trăm lương hiện tại;
        /// byPercentage = false: tăng theo số tiền cố định (dùng lại phiên bản 1).
        /// </summary>
        public void IncreaseSalary(double value, bool byPercentage)
        {
            if (!byPercentage)
            {
                IncreaseSalary(value); // đã kiểm tra value > 0
                return;
            }

            if (value <= 0 || double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Giá trị tăng phải dương.");
            BaseSalary += BaseSalary * (value / 100);
            Console.WriteLine($"[{Id}] Đã tăng lương thêm {value}%. Lương mới: {BaseSalary}");
        }

        // ----- Phương thức ảo (đa hình) -----

        /// <summary>Chi phí hằng tháng, mặc định bằng lương cơ bản.</summary>
        public virtual double CalculateMonthlyCost()
        {
            return BaseSalary;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Nhân sự - Mã: {Id}, Tên: {FullName}, Lương cơ bản: {BaseSalary}, Tổng chi phí: {CalculateMonthlyCost()}");
        }

        /// <summary>Finalizer (destructor trong C#) - in thông báo để quan sát vòng đời.</summary>
        ~Employee()
        {
            Console.WriteLine($"[Destructor] Đã hủy đối tượng Employee: {Id}");
        }
    }
}
