namespace PakaUsers.Model
{
    public class Admin : User
    {
        public override UserType UserType => UserType.Admin;
    }
}