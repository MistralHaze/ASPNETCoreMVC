public interface IUserService{
    public int GetUserId();
}

public class UserService:IUserService{
    public int GetUserId(){
        return 1; //TODO: Implement actual users
    }
}