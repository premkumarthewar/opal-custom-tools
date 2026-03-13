namespace CustomToolsAPI.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGoogleGeminiService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<string> GetResponseFromGoogleGeminiAsync(object request);
    }
}
