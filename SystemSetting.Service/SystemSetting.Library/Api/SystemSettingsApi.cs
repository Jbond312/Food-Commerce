using System;
using System.Collections.Generic;
using SystemSetting.Library.Client;
using RestSharp;
using SystemSetting.Library.Model;

namespace SystemSetting.Library.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ISystemSettingsApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <returns>List&lt;SystemSettingsDto&gt;</returns>
        List<SystemSettingsDto> ApiSystemSettingsGet();
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class SystemSettingsApi : ISystemSettingsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSettingsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public SystemSettingsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient;
            else
                this.ApiClient = apiClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSettingsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public SystemSettingsApi(String basePath)
        {
            this.ApiClient = new ApiClient(basePath);
        }

        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public void SetBasePath(String basePath)
        {
            this.ApiClient.BasePath = basePath;
        }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public String GetBasePath(String basePath)
        {
            return this.ApiClient.BasePath;
        }

        /// <summary>
        /// Gets or sets the API client.
        /// </summary>
        /// <value>An instance of the ApiClient</value>
        public ApiClient ApiClient { get; set; }

        /// <summary>
        ///  
        /// </summary>
        /// <returns>List&lt;SystemSettingsDto&gt;</returns>            
        public List<SystemSettingsDto> ApiSystemSettingsGet()
        {


            var path = "/api/SystemSettings";
            path = path.Replace("{format}", "json");

            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;


            // authentication setting, if any
            String[] authSettings = new String[] { };

            // make the HTTP request
            IRestResponse response = (IRestResponse)ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling ApiSystemSettingsGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling ApiSystemSettingsGet: " + response.ErrorMessage, response.ErrorMessage);

            return (List<SystemSettingsDto>)ApiClient.Deserialize(response.Content, typeof(List<SystemSettingsDto>), response.Headers);
        }

    }
}
