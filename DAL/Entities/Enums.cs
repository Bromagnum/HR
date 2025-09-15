namespace DAL.Entities;

public enum JobApplicationStatus
{
    Draft = 0,
    Submitted = 1,
    UnderReview = 2,
    Interviewed = 3,
    Approved = 4,
    Rejected = 5,
    Withdrawn = 6
}

public enum JobPostingStatus
{
    Draft = 0,
    Active = 1,
    Expired = 2,
    Closed = 3,
    Suspended = 4
}

public enum EducationLevel
{
    None = 0,
    Primary = 1,
    Secondary = 2,
    HighSchool = 3,
    AssociateDegree = 4,
    BachelorsDegree = 5,
    MastersDegree = 6,
    Doctorate = 7
}

