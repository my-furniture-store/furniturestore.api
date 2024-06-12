namespace FurnitureStore.Domain.Users;

public class User
{
    #region Constructors
    public User(string username, string email, byte[] passwordHash, byte[] passwordSalt, Guid? id = null)
    {
        this.Id = id ?? Guid.NewGuid();
        this.Username = username;
        this.Email = email;
        this.PasswordHash = passwordHash;
        this.PasswordSalt = passwordSalt;
    }

    private User() { }
    #endregion Constructors

    #region Properties
    public Guid Id { get; }
    public string Username { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public byte[] PasswordHash { get; private set; } = new byte[32];
    public byte[] PasswordSalt { get; private set; } = new byte[32];
    public string? AccessToken { get; private set; }
    public DateTime? VerifiedAt { get; private set; }
    public string? PasswordResetToken { get; private set; }
    public DateTime? ResetTokenExpires { get; private set; }
    #endregion Properties

    #region Public Methods

    public void SetAccessToken(string accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
            return;
        this.AccessToken = accessToken;
    }

    public void Verify()
    {
        if (this.VerifiedAt.HasValue)
            return;

        this.VerifiedAt = DateTime.Now;
    }

    public void SetPasswordResetToken(string resetToken)
    {
        if(string.IsNullOrWhiteSpace(resetToken)) 
            return;

        this.PasswordResetToken = resetToken;
    }

    public void SetResetTokenExpiryDate()
    {
        this.ResetTokenExpires = DateTime.Now.AddHours(2);
    }

    #endregion Public Methods
}
