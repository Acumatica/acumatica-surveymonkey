namespace PX.SurveyMonkeyReader.Repository
{
    public interface IOAuthRepository
    {
        string GetAuthorizationPageUri();
        string GetAccessToken(string code);
    }
}
