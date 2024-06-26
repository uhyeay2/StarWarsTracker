﻿namespace StarWarsTracker.Domain.Constants.Routes
{
    public static class EventRoute
    {
        public const string Insert = "Event/InsertEvent";

        public const string Delete = "Event/DeleteEventByGuid";

        public const string GetNotHavingDates = "Event/GetAllEventsNotHavingDates";

        public const string GetByGuid = "Event/GetEventByGuid";

        public const string GetByNameAndCanonType = "Event/GetEventByNameAndCanonType";

        public const string GetByNameLike = "Event/GetEventsByNameLike";

        public const string GetByYear = "Event/GetEventsByYear";
    }
}
