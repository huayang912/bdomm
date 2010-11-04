
#region Enums for Send SMS Operation

    public enum MESSAGE_STATUSES
    {
        NOT_SENT = 0,
        SENT_UNDELIVERED = 1,
        SENT_DELIVERED = 2,
        SENT_DELIVERY_FAILED = 3
    }

    public enum MessageStatuss
    {
        NOT_SENT = 0,
        SENT_UNDELIVERED = 1,
        SENT_DELIVERED = 2,
        SENT_DELIVERY_FAILED = 3
    }

    public enum CONNECTION_TYPES
    {
        TEST = 1,
        LOW_VOLUME = 2,
        HIGH_VOLUME = 4
    }

    public enum ORIGINATOR_TYPES
    {
        NUMBER = 0,
        NAME = 1
    }

    public enum REPLY_TYPES
    {
        NONE = 0,
        WEB_SERVICE = 1,
        EMAIL = 2,
        URL = 3
    }

#endregion Enums for Send SMS Operation