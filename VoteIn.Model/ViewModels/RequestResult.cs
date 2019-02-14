namespace VoteIn.Model.ViewModels
{
    public class RequestResult
    {
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public RequestState State { get; set; }
        /// <summary>
        /// Gets or sets the MSG.
        /// </summary>
        /// <value>
        /// The MSG.
        /// </value>
        public string Msg { get; set; }
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public object Data { get; set; }
    }

    public enum RequestState
    {
        Failed = -1,
        NotAuth = 0,
        Success = 1
    }
}
