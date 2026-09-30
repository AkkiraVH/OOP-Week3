/****************/
// Mã sinh viên: 202418910
// Họ tên: Đoàn Việt Hùng
/****************/

using System;

namespace Lab03_04_Overloading
{
    /// <summary>
    /// Kỹ sư phần mềm: kế thừa Employee, thêm ngôn ngữ chính và phụ cấp kỹ thuật.
    /// Bất biến bổ sung: PrimaryLanguage không rỗng; TechnicalAllowance >= 0.
    /// </summary>
    public class SoftwareEngineer : Employee
    {
        private double technicalAllowance;

        public string PrimaryLanguage { get; private set; }

        public double TechnicalAllowance
        {
            get { return technicalAllowance; }
            set
            {
                if (value < 0 || double.IsNaN(value) || double.IsInfinity(value))
                    throw new ArgumentException("Phụ cấp kỹ thuật không được âm (hoặc không hợp lệ).");
                technicalAllowance = value;
            }
        }

        // ----- 2 constructor (nạp chồng) -----

        public SoftwareEngineer(string id, string fullName, string primaryLanguage)
            : this(id, fullName, 0, primaryLanguage, 0) { }

        public SoftwareEngineer(string id, string fullName, double baseSalary,
                                string primaryLanguage, double technicalAllowance)
            : base(id, fullName, baseSalary)
        {
            if (string.IsNullOrWhiteSpace(primaryLanguage))
                throw new ArgumentException("Ngôn ngữ lập trình chính không được để trống.");

            PrimaryLanguage = primaryLanguage.Trim();
            TechnicalAllowance = technicalAllowance;
        }

        // ----- Ghi đè -----

        /// <summary>Tổng chi phí = lương cơ bản + phụ cấp kỹ thuật.</summary>
        public override double CalculateMonthlyCost()
        {
            return BaseSalary + TechnicalAllowance;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Kỹ sư phần mềm - Mã: {Id}, Tên: {FullName}, NN chính: {PrimaryLanguage}, " +
                              $"Lương: {BaseSalary}, Phụ cấp: {TechnicalAllowance}, Tổng chi phí: {CalculateMonthlyCost()}");
        }

        ~SoftwareEngineer()
        {
            Console.WriteLine($"[Destructor] Đã hủy đối tượng SoftwareEngineer: {Id}");
        }
    }
}
