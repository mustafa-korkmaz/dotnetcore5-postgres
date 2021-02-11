namespace dotnetpostgres
{
    public enum SortType
    {
        None,
        Ascending,
        Descending
    }

    public enum Status : byte
    {
        Active,
        Passive,
        Suspended,
        Deleted
    }

    public enum EmailConfirmationStatus
    {
        None,
        Confirmed,
        NotConfirmed
    }

    public enum Key
    {
        Tag,
        Discount,
        Image,
        Video
    }

    public enum ResponseType
    {
        InternalError = -3,
        ValidationError = -2,
        Fail = -1,
        Success = 0,
        Warning = 1,
        Info = 2,
        NoEffect = 3,
        DuplicateRecord = 4,
        RecordNotFound = 5,
    }

    public enum NotificationStatus : byte
    {
        Initial = 0,
        WaitingForSendingNotification = 1,
        NotificationSent = 2,
        NotificationRead = 3,
        HasError = 4
    }

    public enum DeviceType : byte
    {
        Ios = 0,
        Android = 1,
        None = 2
    }

    public enum LogType
    {
        /// <summary>
        /// use this type when you are initializing an object and want to log it.
        /// </summary>
        Initialize,
        /// <summary>
        /// use this type when you are creating an object and want to log it.
        /// </summary>
        Create,
        /// <summary>
        /// use this type when you are soft-deleting an object and want to log it.
        /// </summary>
        SoftDelete,
        /// <summary>
        /// use this type when you are hard-deleting an object and want to log it.
        /// </summary>
        HardDelete,
        /// <summary>
        /// use this type when you are editing an object and want to log it.
        /// </summary>
        Modify,
        /// <summary>
        /// use this type when you are logging a req & resp
        /// </summary>
        ReqAndResp,
        /// <summary>
        /// use this type when you are logging an exception
        /// </summary>
        Error
    }

}
