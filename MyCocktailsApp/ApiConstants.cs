namespace MyCocktailsApi
{
    public class ApiConstants
    {
        public class Account
        {
            public const string AtAccountPageMessage = "You are at Account Index page.";
            public const string AlreadyLoggedMessage = "User is already logged in.";
            public const string FailedSearcForUserMessage = "Failed to search for registerd user.";
            public const string NotExistingUserMessage = "User doesn't exist!";
            public const string FailedLogInUserMessage = "Failed to log in user";
            public const string RedirectAndLogedInMessage = "You are redirected and logged In as ";
            public const string InvalidEmailAndPasswordMessage = "Invalid email or password!";
            public const string RedirectAndLogedOutMessage = "You are redirected and logged Out!";
        }

        public class Cocktail
        {
            public const string UpdatedLikesMessage = "Likes were updated.";
            public const string UpdatedCocktailMessage = "The cocktail is updated.";
            public const string DeletedCocktailMessage = "The cocktail is deleted.";
            public const string NotExistingOrEmptyCollectionMessage = "Not existing or empty collection.";
            public const string RequiredIdMessage = "Id is required.";
            public const string NotExistingCocktailMessage = "The cocktail doesn't exist!";
            public const string RequriedNameMessage = "Name is required.";
            public const string NotExistingCocktailWithNameMessage = "Cocktail with name {0} doesn't exist";
            public const string RequiredCategoryMessage = "Category is required.";
            public const string InvalidModelMessage = "The model is invalid.";
            public const string UnauthorizedMessage = "Unauthorized! Please Login!";
            public const string TestUserEmail = "TestUser@av.bg";
        }

        public class Secured
        {
            public const string AtUserPageMessage = "You are at page for Users.";
            public const string AtAdminPageMessage = "You are at page for Admins.";
        }

        public class User
        {
            public const string AtUserIndexPageMessage = "You are at User Index page.";
            public const string UnableCheckForUserMessage = "Unable to check if there is currently logged user.";
            public const string UserAlreadyLoggedMessage = "User is already logged in.";
            public const string FailedSearchForUserMessage = "Failed to search for existing user by Email!";
            public const string AlreadyExistingUserMessage = "User with tha same Name or Email already exist.";
            public const string FailCreateUserMessage = "Failed to create user!";
            public const string FailAddRoleToUserMessage = "Failed to add role to user.";
            public const string SuccessfullyUserCreateMessage = "User is created!";
            public const string FailAddRoleMessage = "Failed to create Role.";
            public const string SuccessfullyRoleCreateMessage = "Role is created!";
        }

        public class CocktailService
        {
            public const string FailedByIdMessage = "Failed to get the cocktail by id!";

            public const string FailedByNameMessage = "Failed to get the cocktail by name!";
            public const string FailedByCategoryMessage = "Failed to get the cocktail by \"category\"!";
            public const string FailedCreateMessage = "Failed to create new cocktail!";
            public const string FailedUpdateCocktailMessage = "Failed to update the cocktail!";
            public const string FailedUpdateLikesMessage = "Failed to update likes!";
            public const string FailedUDeleteCocktailMessage = "Failed to delete the cocktail!";




        }
    }
}
