using System;

namespace Models;

public class User : IUser
{
    public virtual Guid UserId { get; set; }

    public virtual string UserName { get; set; }
    public virtual string UserEmail { get; set; }
    public virtual string UserPassword { get; set; }

    public virtual string UserRole { get; set; }
}