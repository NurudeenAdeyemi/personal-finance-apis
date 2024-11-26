namespace Domain.Entities
{
    public class Account
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string ICNumber { get; private set; } = default!;
        public string MobileNumber { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string PinHash { get; private set; } = default!;
        public bool IsBiometricEnabled { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Account() { }

        public Account(string name, string icNumber, string mobileNumber, string email, string pinHash)
        {
            Id = Guid.NewGuid();
            Name = name;
            ICNumber = icNumber;
            MobileNumber = mobileNumber;
            Email = email;
            PinHash = pinHash;
            IsBiometricEnabled = false;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateBiometricSetting(bool enableBiometric)
        {
            IsBiometricEnabled = enableBiometric;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
