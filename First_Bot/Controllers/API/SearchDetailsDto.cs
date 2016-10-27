namespace First_Bot.Controllers.API
{
    public class SearchDetailsDto
    {
        public int JobId { get; set; }
        public int EmployerId { get; set; }
        public string EmployerName { get; set; }
        public int? EmployerProfileId { get; set; }
        public string EmployerProfileName { get; set; }

        public string JobTitle { get; set; }

        //public int LocationId { get; set; }
        public string LocationName { get; set; }

        public decimal? MinimumSalary { get; set; }
        public decimal? MaximumSalary { get; set; }
        public string Currency { get; set; }

        public string ExpirationDate { get; set; }
        public string Date { get; set; }

        public string JobDescription { get; set; }
        public int Applications { get; set; }
        public string JobUrl { get; set; }
    }
}