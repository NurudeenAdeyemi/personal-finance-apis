namespace Domain.Entities
{
    public class Account
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string ICNumber { get; private set; } = default!;
        public string MobileNumber { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public bool MobileNumberConfirmed { get; private set; }
        public bool EmailConfirmed { get; private set; }
        public string? PinHash { get; private set; }
        public bool PinSetup { get; private set; }
        public bool BiometricEnabled { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Account() { }

        public Account(string name, string icNumber, string mobileNumber, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            ICNumber = icNumber;
            MobileNumber = mobileNumber;
            Email = email;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ConfirmMobileNumber()
        {
            MobileNumberConfirmed = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ConfirmEmail()
        {
            EmailConfirmed = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPin(string pin)
        {
            PinHash = pin;
            PinSetup = true;
            UpdatedAt = DateTime.UtcNow;
        }
        public void UpdateBiometricSetting(bool enableBiometric)
        {
            BiometricEnabled = enableBiometric;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
